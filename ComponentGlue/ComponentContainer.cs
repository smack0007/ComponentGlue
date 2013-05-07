﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ComponentGlue.BindingSyntax;

namespace ComponentGlue
{
	public class ComponentContainer : IComponentContainer, IDisposable
	{
		ComponentContainer parent;

		ComponentBindingCollection defaultBindings;
		Dictionary<Type, ComponentBindingCollection> componentBindings;

		/// <summary>
		/// Contains a list of components currently being constructed by the container. Used to detect circular dependencies.
		/// </summary>
		Stack<Type> constructStack;

		Type injectAttributeType;
		Type defaultComponentAttributeType;

		/// <summary>
		/// The attribute type which indicates injection.
		/// </summary>
		public Type InjectAttributeType
		{
			get { return this.injectAttributeType; }

			set
			{
				this.EnsureTypeIsAttribute(value);

				this.injectAttributeType = value;
			}
		}

		/// <summary>
		/// The attribute type which indicates injection.
		/// </summary>
		public Type DefaultComponentAttributeType
		{
			get { return this.defaultComponentAttributeType; }

			set
			{
				this.EnsureTypeIsAttribute(value);

				if (!typeof(IDefaultComponentAttribute).IsAssignableFrom(value))
					throw new InvalidOperationException("DefaultComponentAttributeType must be assignable from IDefaultComponentAttribute.");

				this.defaultComponentAttributeType = value;
			}
		}

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

			this.defaultBindings = new ComponentBindingCollection();
			this.componentBindings = new Dictionary<Type, ComponentBindingCollection>();

			this.constructStack = new Stack<Type>();

			this.Bind(typeof(ComponentContainer)).ToConstant(this);
			this.Bind(typeof(IComponentContainer)).ToConstant(this);

			this.injectAttributeType = typeof(InjectAttribute);
			this.defaultComponentAttributeType = typeof(DefaultComponentAttribute);
		}

		/// <summary>
		/// Frees all references to constructed objects and bindings.
		/// </summary>
		public void Dispose()
		{
			this.defaultBindings = null;
			this.componentBindings = null;
		}

		private void EnsureTypeIsAttribute(Type type)
		{
			if (!typeof(Attribute).IsAssignableFrom(type))
				throw new InvalidOperationException(string.Format("{0} is not an Attribute type.", type));
		}

        private bool IsAutoFactoryType(Type type)
        {
            if (typeof(MulticastDelegate).IsAssignableFrom(type))
            {
                MethodInfo method = type.GetMethod("Invoke");
                if (method.GetParameters().Length == 0)
                {
                    return true;
                }
            }

            return false;
        }

		private Func<T> ConstructAutoFactory<T>()
		{
			return () => { return (T)this.Resolve(typeof(T)); };
		}

        /// <summary>
        /// Examines the given type and constructs an AutoFactory if the type is correct.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private object TryConstructAutoFactory(Type type)
        {
            if (typeof(MulticastDelegate).IsAssignableFrom(type))
            {
                MethodInfo method = type.GetMethod("Invoke");
                if (method.GetParameters().Length == 0)
                {
                    MethodInfo constructMethod = this.GetType().GetMethod("ConstructAutoFactory", BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(method.ReturnType);
                    return constructMethod.Invoke(this, null);
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the constructor which should be used for the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private ConstructorInfo GetConstructorToInject(Type type)
        {
            ConstructorInfo injectableConstructor = null;
            ConstructorInfo[] constructors = type.GetConstructors();

            if (constructors.Length == 1)
            {
                injectableConstructor = constructors[0];
            }
            else
            {
                foreach (ConstructorInfo constructor in constructors)
                {
                    if (constructor.GetParameters().Length == 0)
                        injectableConstructor = constructor;

                    foreach (Attribute attribute in constructor.GetCustomAttributes(this.injectAttributeType, true))
                    {
                        if (injectableConstructor != null)
                            throw new ComponentResolutionException(string.Format("Multiple injectable constructors found for type {0}.", type));

                        injectableConstructor = constructor;
                    }
                }
            }

            if (injectableConstructor == null)
                throw new ComponentResolutionException(string.Format("No injectable or default constructor found for type {0}.", type));

            return injectableConstructor;
        }
        
        /// <summary>
        /// Builds an array of parameters to be injected into a constructor.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constructor"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private object[] BuildConstructorParameters(Type type, ConstructorInfo constructor, Dictionary<string, object> parameters)
        {
            ParameterInfo[] constructorParameters = constructor.GetParameters();
            object[] parametersToInject = new object[constructorParameters.Length];

            for (int i = 0; i < constructorParameters.Length; i++)
            {
                if (parameters != null && parameters.ContainsKey(constructorParameters[i].Name))
                {
                    parametersToInject[i] = parameters[constructorParameters[i].Name];
                }
                else
                {
                    try
                    {
                        parametersToInject[i] = this.ResolveComponentForInjection(type, constructorParameters[i].ParameterType);
                    }
                    catch (Exception ex)
                    {
                        throw new ComponentResolutionException(string.Format("Failed while resolving constructor parameter \"{0}\" for type \"{1}\".", constructorParameters[i].Name, type), ex);
                    }
                }
            }

            return parametersToInject;
        }

		/// <summary>
		/// Constructs a new instance of a type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private object ConstructComponent(Type type, Dictionary<string, object> parameters)
		{
			if (type.IsInterface)
				throw new ComponentResolutionException(string.Format("{0} is an interface and cannot be constructed.", type));

			if (type.IsAbstract)
				throw new ComponentResolutionException(string.Format("{0} is abstract and cannot be constructed.", type));

			if (this.constructStack.Contains(type))
				throw new ComponentResolutionException("Possible infinite construction loop detected.");

			this.constructStack.Push(type);
						
			object component = null;

            component = this.TryConstructAutoFactory(type);

			if (component == null)
			{
                ConstructorInfo constructor = this.GetConstructorToInject(type);

                object[] constructorParameters = this.BuildConstructorParameters(type, constructor, parameters);
                		
				try
				{
					component = constructor.Invoke(constructorParameters);
				}
				catch (TargetInvocationException ex)
				{
					throw new ComponentResolutionException(string.Format("Failed while constructing type {0}.", type), ex);
				}
			}

			this.constructStack.Pop();

			return component;
		}
								
		/// <summary>
		/// Gets an instance of a type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public object Resolve(Type type)
		{
			object component = null;
            			
			// Default bindings
			if (this.defaultBindings.HasBinding(type))
			{
				component = this.ResolveComponentByBinding(this.defaultBindings.GetBinding(type));
			}
			else
			{
                if (type.IsInterface) 
                {
                    // Proxy to parent container if available
                    if (this.parent != null)
				        component = this.parent.Resolve(type);
                }
                else
                {
                    component = this.ConstructComponent(type, null);
					this.ResolveProperties(component);
                }
			}
			
			if (component == null)
				throw new ComponentResolutionException(string.Format("Unable to resolve type {0}.", type));

			return component;
		}
				
		/// <summary>
		/// Gets a component instance based on a binding.
		/// </summary>
		/// <param name="binding"></param>
		/// <returns></returns>
		private object ResolveComponentByBinding(ComponentBinding binding)
		{
			object component = null;

			switch (binding.BindType)
			{
				case ComponentBindType.Transient:
					component = this.ConstructComponent(binding.ConcreteType, binding.ConstructorParameters);
					this.ResolveProperties(component);
					break;

				case ComponentBindType.Singleton:
					if(binding.SingletonInstance == null)
					{
                        // The order here is important for resolving circular dependencies.
                        binding.SingletonInstance = this.ConstructComponent(binding.ConcreteType, binding.ConstructorParameters);
                        this.ResolveProperties(binding.SingletonInstance);

                        component = binding.SingletonInstance;
					}
					else
					{
                        component = binding.SingletonInstance;
					}
					break;
									
				case ComponentBindType.Constant:
					component = binding.Data;
					break;

				case ComponentBindType.FactoryMethod:
					component = ((Func<IComponentContainer, object>)binding.Data)(this);
					break;

                case ComponentBindType.Multiple:
                    var bindings = ((List<ComponentBinding>)binding.Data);
                    var components = Array.CreateInstance(binding.ComponentType.GetElementType(), bindings.Count);

                    for (int i = 0; i < bindings.Count; i++)
                    {
                        components.SetValue(this.ResolveComponentByBinding(bindings[i]), i);
                    }

                    component = components;
                    break;
			}
			
			return component;
		}

		/// <summary>
		/// Gets a component instance for injection.
		/// </summary>
		/// <param name="constructedType">The type which is currently being constructed.</param>
		/// <param name="componentType">The requested component type.</param>
		/// <returns></returns>
		private object ResolveComponentForInjection(Type constructedType, Type componentType)
		{
			object component = null;

			// Specific bindings
			if (this.componentBindings.ContainsKey(constructedType) && this.componentBindings[constructedType].HasBinding(componentType))
				component = this.ResolveComponentByBinding(this.componentBindings[constructedType].GetBinding(componentType));
			
			// Default bindings
			if (component == null && this.defaultBindings.HasBinding(componentType))
				component = this.ResolveComponentByBinding(this.defaultBindings.GetBinding(componentType));

			// Proxy to parent container if available
			if (component == null && this.parent != null)
				component = this.parent.ResolveComponentForInjection(constructedType, componentType);

			// Component not found
			if (component == null)
				component = this.ConstructComponent(componentType, null);

			return component;
		}
		
		/// <summary>
		/// Injects components into the properties of the instance marked with an Inject attribute.
		/// </summary>
		/// <param name="instance"></param>
		public void ResolveProperties(object instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			Type type = instance.GetType();

			foreach (PropertyInfo property in instance.GetType().GetProperties())
			{
				foreach (Attribute attribute in property.GetCustomAttributes(true))
				{
					if (this.injectAttributeType.IsInstanceOfType(attribute))
					{
						if (!property.CanWrite)
							throw new ComponentResolutionException(string.Format("Property \"{0}\" of type \"{1}\" is marked as Inject but not writable.", property.Name, type));

						property.SetValue(instance, ResolveComponentForInjection(type, property.PropertyType), null);
					}
				}
			}
		}

		public IBindingSyntaxBind For(Type type)
		{
			if (!this.componentBindings.ContainsKey(type))
				this.componentBindings.Add(type, new ComponentBindingCollection());

			return this.componentBindings[type];
		}

		public IBindingSyntaxTo Bind(Type type)
		{
			return this.defaultBindings.Bind(type);
		}
						
		public bool HasBinding(Type type)
		{
			return this.defaultBindings.HasBinding(type);
		}

		public IBindingSyntaxTo Rebind(Type type)
		{
			return this.defaultBindings.Rebind(type);
		}

		/// <summary>
		/// Performs auto binding on all the types in the entry assembly as Transient.
		/// </summary>
		public void AutoBind()
		{
			this.DoAutoBind(Assembly.GetEntryAssembly(), default(ComponentBindType), null);
		}

		/// <summary>
		/// Performs auto binding on all the types under the given namespace in the given assemly as the given bind type.
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="namespace"></param>
		/// <param name="bindType"></param>
		public void AutoBind(Assembly assembly, ComponentBindType bindType)
		{
			this.DoAutoBind(assembly, bindType, null);
		}

		/// <summary>
		/// Performs auto binding on all the types under the given namespace in the given assemly as the given bind type.
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="namespace"></param>
		/// <param name="bindType"></param>
		public void AutoBind(Assembly assembly, string @namespace, ComponentBindType bindType)
		{
			this.DoAutoBind(assembly, bindType, x => x.Namespace == @namespace);
		}

		/// <summary>
		/// Performs auto binding on all the types which pass the where clause in the given assemly as the given bind type.
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="bindType"></param>
		public void AutoBind(Assembly assembly, ComponentBindType bindType, Func<Type, bool> whereFunc)
		{
			if (whereFunc == null)
				throw new ArgumentNullException("whereFunc");

			this.DoAutoBind(assembly, bindType, whereFunc);
		}

		private void DoAutoBind(Assembly assembly, ComponentBindType bindType, Func<Type, bool> whereFunc)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			Dictionary<Type, List<Type>> implementors = new Dictionary<Type, List<Type>>();

			IEnumerable<Type> types = assembly.GetTypes();

			if (whereFunc != null)
				types = types.Where(whereFunc);

			foreach (Type componentType in types)
			{
				if (componentType.IsClass && !componentType.IsAbstract)
				{
					if (!this.HasBinding(componentType))
						this.Bind(componentType).ToSelf().As(bindType);

					foreach (Type interfaceType in componentType.GetInterfaces())
					{
						if (!implementors.ContainsKey(interfaceType))
							implementors.Add(interfaceType, new List<Type>());

						if (!implementors[interfaceType].Contains(componentType))
							implementors[interfaceType].Add(componentType);
					}
				}
			}

			foreach (Type interfaceType in implementors.Keys)
			{
				if (!this.HasBinding(interfaceType))
				{
					if (implementors[interfaceType].Count == 1) // One implementor so we have the default binding
					{
						this.Bind(interfaceType).To(implementors[interfaceType][0]).As(bindType);
					}
					else
					{
						Type defaultComponent = null;

						foreach (Type componentType in implementors[interfaceType])
						{
							foreach (IDefaultComponentAttribute attribute in componentType.GetCustomAttributes(this.defaultComponentAttributeType, false))
							{
								if(attribute.ComponentType == interfaceType)
								{
									if (defaultComponent != null)
										throw new ComponentResolutionException(string.Format("More than one component is marked as DefaultComponent for type {0}.", interfaceType));

									defaultComponent = componentType;
								}
							}
						}

						if (defaultComponent != null)
							this.Bind(interfaceType).To(defaultComponent).As(bindType);
					}
				}
			}
		}
	}
}
