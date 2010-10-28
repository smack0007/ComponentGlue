using System;
using System.Collections.Generic;
using System.Reflection;
using ComponentGlue.Framework.BindingSyntax;

namespace ComponentGlue.Framework
{
	public class Kernel : IKernel, IDisposable, IBindingSyntaxRoot
	{
		Dictionary<Type, object> components;
		BindingCollection defaultBindings;
		Dictionary<Type, BindingCollection> componentBindings;

		/// <summary>
		/// Constructor.
		/// </summary>
		public Kernel()
		{
			this.components = new Dictionary<Type, object>();
			this.defaultBindings = new BindingCollection();
			this.componentBindings = new Dictionary<Type, BindingCollection>();

			Bind<IKernel>().ToConstant(this);
			Bind<Kernel>().ToConstant(this);
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
		public object Construct(Type componentType)
		{
			ConstructorInfo defaultConstructor = null;
			ConstructorInfo injectableConstructor = null;
			foreach(ConstructorInfo constructor in componentType.GetConstructors())
			{
				if(constructor.GetParameters().Length == 0)
					defaultConstructor = constructor;

				foreach(Attribute attribute in constructor.GetCustomAttributes(true))
				{
					if(attribute is InjectAttribute)
					{
						if(injectableConstructor != null)
							throw new InvalidOperationException("Multiple injectable constructors found for type " + componentType);

						injectableConstructor = constructor;
					}
				}
			}

			if(defaultConstructor == null && injectableConstructor == null)
				throw new InvalidOperationException("No injectable or default constructor found for type " + componentType);

			if(injectableConstructor != null)
			{
				ParameterInfo[] parameters = injectableConstructor.GetParameters();
				object[] injectComponents = new object[parameters.Length];

				int i = 0;
				foreach(ParameterInfo parameter in parameters)
					injectComponents[i++] = GetComponentForInjection(componentType, parameter.ParameterType);

				return injectableConstructor.Invoke(injectComponents);
			}
			else
			{
				return defaultConstructor.Invoke(null);
			}
		}

		public TComponentType Construct<TComponentType>()
		{
			return (TComponentType)Construct(typeof(TComponentType));
		}
				
		/// <summary>
		/// Gets an instance of a type.
		/// </summary>
		/// <param name="componentType"></param>
		/// <returns></returns>
		public object Get(Type componentType)
		{
			if(this.components.ContainsKey(componentType))
				return this.components[componentType];

			object component = Construct(componentType);
			this.components[componentType] = component;
			return component;
		}

		public TComponentType Get<TComponentType>()
		{
			return (TComponentType)Get(typeof(TComponentType));
		}

		/// <summary>
		/// Gets a component instance based on a binding.
		/// </summary>
		/// <param name="interfaceType"></param>
		/// <param name="binding"></param>
		/// <returns></returns>
		private object GetComponentByBinding(Type constructedType, Binding binding)
		{
			object component = null;

			switch(binding.Type)
			{
				case BindType.Shared:
					component = Get(binding.ComponentType);
					break;

				case BindType.New:
					component = Construct(binding.ComponentType);
					break;

				case BindType.Constant:
					component = binding.Constant;
					break;

				case BindType.Method:
					component = binding.Method(constructedType, binding.InterfaceType);

					if(!(binding.InterfaceType.IsAssignableFrom(component.GetType())))
						throw new InvalidOperationException("Factory method did not produce and instance of " + binding.InterfaceType);

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
				component = GetComponentByBinding(constructedType, this.componentBindings[constructedType].GetBinding(interfaceType));
			
			// Default bindings
			if(component == null && this.defaultBindings.HasBinding(interfaceType))
				component = GetComponentByBinding(constructedType, this.defaultBindings.GetBinding(interfaceType));

			// Component not found
			if(component == null)
				throw new InvalidOperationException("Failed to get component of type " + interfaceType + " for injection into " + constructedType);

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
				this.componentBindings.Add(constructedType, new BindingCollection());

			return this.componentBindings[constructedType];
		}

		public IBindingSyntaxBind For<TConstructedType>()
		{
			return For(typeof(TConstructedType));
		}

		public IBindingSyntaxTo Bind(Type interfaceType)
		{
			return this.defaultBindings.Bind(interfaceType);
		}

		public IBindingSyntaxTo Bind<TInterfaceType>()
		{
			return Bind(typeof(TInterfaceType));
		}

		public bool HasBinding(Type interfaceType)
		{
			return this.defaultBindings.HasBinding(interfaceType);
		}

		public bool HasBinding<TInterfaceType>()
		{
			return HasBinding(typeof(TInterfaceType));
		}

		public IBindingSyntaxTo Rebind(Type interfaceType)
		{
			return this.defaultBindings.Rebind(interfaceType);
		}

		public IBindingSyntaxTo Rebind<TInterfaceType>()
		{
			return Rebind(typeof(TInterfaceType));
		}
	}
}
