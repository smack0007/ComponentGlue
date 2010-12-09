using System;
using System.Collections.Generic;
using System.Reflection;
using ComponentGlue.Framework.BindingSyntax;

namespace ComponentGlue.Framework
{
	public class ComponentContainer : IComponentContainer, IDisposable
	{
		ComponentContainer parent;

		Dictionary<Type, object> components;
		ComponentBindingCollection defaultBindings;
		Dictionary<Type, ComponentBindingCollection> componentBindings;

		Stack<Type> constructStack;

		/// <summary>
		/// Constructor.
		/// </summary>
		public ComponentContainer()
			: this(null)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent">A parent container.</param>
		public ComponentContainer(ComponentContainer parent)
		{
			this.parent = parent;

			this.components = new Dictionary<Type, object>();
			this.defaultBindings = new ComponentBindingCollection();
			this.componentBindings = new Dictionary<Type, ComponentBindingCollection>();

			this.constructStack = new Stack<Type>();

			this.Bind(typeof(ComponentContainer)).ToConstant(this);
			this.Bind(typeof(IComponentContainer)).ToConstant(this);
		}

		/// <summary>
		/// Frees all references to constructed objects and bindings.
		/// </summary>
		public void Dispose()
		{
			this.components = null;
			this.defaultBindings = null;
			this.componentBindings = null;
		}
				
		/// <summary>
		/// Constructs a new instance of a type.
		/// </summary>
		/// <param name="componentType"></param>
		/// <returns></returns>
		private object Construct(Type componentType)
		{
			if(componentType.IsAbstract)
				throw new InvalidOperationException(componentType + " is abstract.");

			if(this.constructStack.Contains(componentType))
				throw new InvalidOperationException("Possible infinite construction loop detected.");

			this.constructStack.Push(componentType);

			ConstructorInfo injectableConstructor = null;
			ConstructorInfo[] constructors = componentType.GetConstructors();

			if(constructors.Length == 1)
			{
				injectableConstructor = constructors[0];
			}
			else
			{
				foreach(ConstructorInfo constructor in constructors)
				{
					if(constructor.GetParameters().Length == 0)
						injectableConstructor = constructor;

					foreach(Attribute attribute in constructor.GetCustomAttributes(true))
					{
						if(attribute is InjectAttribute)
						{
							if(injectableConstructor != null)
								throw new InvalidOperationException("Multiple injectable constructors found for type " + componentType + ".");

							injectableConstructor = constructor;
						}
					}
				}
			}

			if(injectableConstructor == null)
				throw new InvalidOperationException("No injectable or default constructor found for type " + componentType + ".");

			ParameterInfo[] parameters = injectableConstructor.GetParameters();
			object[] injectComponents = new object[parameters.Length];

			int i = 0;
			foreach(ParameterInfo parameter in parameters)
				injectComponents[i++] = this.GetComponentForInjection(componentType, parameter.ParameterType);

			object obj = injectableConstructor.Invoke(injectComponents);

			this.constructStack.Pop();

			return obj;
		}
								
		/// <summary>
		/// Gets an instance of a type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public object Get(Type type)
		{
			object component = null;

			if(type.IsInterface)
			{
				// Default bindings
				if(this.defaultBindings.HasBinding(type))
					component = GetComponentByBinding(this.defaultBindings.GetBinding(type));
				else if(this.parent != null) // Proxy to parent container if available
					component = this.parent.Get(type);
			}
			else
			{
				if(this.defaultBindings.HasBinding(type))
				{
					component = this.GetComponentByBinding(this.defaultBindings.GetBinding(type));
				}
				else
				{
					component = this.Construct(type);
					this.Inject(component);
				}
			}

			if(component == null)
				throw new InvalidOperationException("Unable to reslove " + type + ".");

			return component;
		}
				
		/// <summary>
		/// Gets a component instance based on a binding.
		/// </summary>
		/// <param name="interfaceType"></param>
		/// <param name="binding"></param>
		/// <returns></returns>
		private object GetComponentByBinding(ComponentBinding binding)
		{
			object component = null;

			switch(binding.Type)
			{
				case ComponentBindType.Transient:
					component = this.Construct(binding.ComponentType);
					this.Inject(component);
					break;

				case ComponentBindType.Singleton:
					if(!this.components.ContainsKey(binding.InterfaceType))
					{
						component = this.Construct(binding.ComponentType);
						this.components[binding.InterfaceType] = component;
						this.Inject(component);
					}
					else
					{
						component = this.components[binding.InterfaceType];
					}
					break;
									
				case ComponentBindType.Constant:
					component = binding.Constant;
					break;
			}
			
			return component;
		}

		/// <summary>
		/// Gets a component instance for injection.
		/// </summary>
		/// <param name="constructedType"></param>
		/// <param name="interfaceType"></param>
		/// <returns></returns>
		private object GetComponentForInjection(Type constructedType, Type interfaceType)
		{
			object component = null;

			// Specific bindings
			if(this.componentBindings.ContainsKey(constructedType) && this.componentBindings[constructedType].HasBinding(interfaceType))
				component = this.GetComponentByBinding(this.componentBindings[constructedType].GetBinding(interfaceType));
			
			// Default bindings
			if(component == null && this.defaultBindings.HasBinding(interfaceType))
				component = this.GetComponentByBinding(this.defaultBindings.GetBinding(interfaceType));

			// Proxy to parent container if available
			if(component == null && this.parent != null)
				component = this.parent.GetComponentForInjection(constructedType, interfaceType);

			// Component not found
			if(component == null)
				component = this.Construct(interfaceType);

			return component;
		}
		
		/// <summary>
		/// Injects components into the properties of the instance marked with an Inject attribute.
		/// </summary>
		/// <param name="instance"></param>
		public void Inject(object instance)
		{
			Type type = instance.GetType();

			foreach(PropertyInfo property in instance.GetType().GetProperties())
			{
				foreach(Attribute attribute in property.GetCustomAttributes(true))
				{
					if(attribute is InjectAttribute)
					{
						if(!property.CanWrite)
							throw new InvalidOperationException(property.Name + " is marked as Inject but not writable.");

						property.SetValue(instance, GetComponentForInjection(type, property.PropertyType), null);
					}
				}
			}
		}

		public IBindingSyntaxBind For(Type constructedType)
		{
			if(!this.componentBindings.ContainsKey(constructedType))
				this.componentBindings.Add(constructedType, new ComponentBindingCollection());

			return this.componentBindings[constructedType];
		}

		public IBindingSyntaxTo Bind(Type interfaceType)
		{
			return this.defaultBindings.Bind(interfaceType);
		}
						
		public bool HasBinding(Type interfaceType)
		{
			return this.defaultBindings.HasBinding(interfaceType);
		}

		public IBindingSyntaxTo Rebind(Type interfaceType)
		{
			return this.defaultBindings.Rebind(interfaceType);
		}
				
		public void AutoBind()
		{
			AutoBind(Assembly.GetEntryAssembly());
		}

		public void AutoBind(Assembly assembly)
		{
			Dictionary<Type, List<Type>> implementors = new Dictionary<Type,List<Type>>();

			foreach(Type componentType in assembly.GetTypes())
			{
				if(componentType.IsClass)
				{
					foreach(Type interfaceType in componentType.GetInterfaces())
					{
						if(!implementors.ContainsKey(interfaceType))
							implementors.Add(interfaceType, new List<Type>());

						if(!implementors[interfaceType].Contains(componentType))
							implementors[interfaceType].Add(componentType);
					}
				}
			}

			foreach(Type interfaceType in implementors.Keys)
			{
				if(!HasBinding(interfaceType))
				{
					if(implementors[interfaceType].Count == 1) // One implementor so we have the default binding
					{
						this.Bind(interfaceType).To(implementors[interfaceType][0]);
					}
					else
					{
						Type defaultComponent = null;

						foreach(Type componentType in implementors[interfaceType])
						{
							foreach(DefaultComponentAttribute attribute in componentType.GetCustomAttributes(typeof(DefaultComponentAttribute), false))
							{
								if(attribute.InterfaceType == interfaceType)
								{
									if(defaultComponent != null)
										throw new InvalidOperationException("More than one component is marked as DefaultComponent for " + interfaceType + ".");

									defaultComponent = componentType;
								}
							}
						}
						
						if(defaultComponent != null)
							this.Bind(interfaceType).To(defaultComponent);
					}
				}
			}
		}
	}
}
