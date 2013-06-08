using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ComponentGlue
{
	public class ComponentContainer : IComponentContainer, IDisposable
	{
		ComponentContainer parent;

		ComponentBindingCollection globalBindings;
		Dictionary<Type, ComponentBindingCollection> componentBindings;

		/// <summary>
		/// Contains a list of components currently being constructed by the container. Used to detect circular dependencies.
		/// </summary>
		Stack<Type> constructStack;

		Type resolveAttributeType;
		
		/// <summary>
		/// The attribute type which indicates injection.
		/// </summary>
		public Type ResolveAttributeType
		{
			get { return this.resolveAttributeType; }

			set
			{
                if (!typeof(Attribute).IsAssignableFrom(value))
                    throw new InvalidOperationException(string.Format("{0} is not an Attribute type.", value));
				
				this.resolveAttributeType = value;
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

			this.globalBindings = new ComponentBindingCollection();
			this.componentBindings = new Dictionary<Type, ComponentBindingCollection>();

			this.constructStack = new Stack<Type>();

			this.Bind(typeof(ComponentContainer)).ToConstant(this);
			this.Bind(typeof(IComponentContainer)).ToConstant(this);

			this.resolveAttributeType = typeof(ResolveAttribute);
		}

		/// <summary>
		/// Frees all references to constructed objects and bindings.
		/// </summary>
		public void Dispose()
		{
            foreach (ComponentBinding binding in this.globalBindings)
            {
                binding.Dispose();
            }

			this.globalBindings = null;

            foreach (var specificComponentBindings in this.componentBindings.Values)
            {
                foreach (ComponentBinding binding in specificComponentBindings)
                {
                    binding.Dispose();
                }
            }

			this.componentBindings = null;
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

                    foreach (Attribute attribute in constructor.GetCustomAttributes(this.resolveAttributeType, true))
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
        private object[] BuildConstructorParameters(Type type, ConstructorInfo constructor, IDictionary<string, object> parameters)
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

        private void ApplyPropertyValues(object component, IDictionary<string, object> propertyValues)
        {
            Type type = component.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (KeyValuePair<string, object> pair in propertyValues)
            {
                PropertyInfo property = properties.Where(x => x.Name == pair.Key).SingleOrDefault();

                if (property == null)
                    throw new ComponentResolutionException(string.Format("Type \"{0}\" does not have a property named \"{1}\".", type, pair.Key));

                try
                {
                    property.SetValue(component, pair.Value, null);
                }
                catch (Exception ex)
                {
                    throw new ComponentResolutionException(string.Format("Failed while setting property value \"{0}\" for type \"{1}\".", pair.Key, type), ex);
                }
            }
        }

		/// <summary>
		/// Constructs a new instance of a type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		internal object Construct(
            Type type,
            IDictionary<string, object> constructorParameters,
            IDictionary<string, object> propertyValues)
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

                object[] componentConstructorParameters = this.BuildConstructorParameters(type, constructor, constructorParameters);
                		
				try
				{
                    component = constructor.Invoke(componentConstructorParameters);
				}
				catch (TargetInvocationException ex)
				{
					throw new ComponentResolutionException(string.Format("Failed while constructing type {0}.", type), ex);
				}
			}

			this.constructStack.Pop();

            if (propertyValues != null)
                this.ApplyPropertyValues(component, propertyValues);

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
			if (this.globalBindings.HasBinding(type))
			{
				component = this.globalBindings.GetBinding(type).Resolve(this);
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
                    component = this.Construct(type, null, null);
					this.ResolveProperties(component);
                }
			}
			
			if (component == null)
				throw new ComponentResolutionException(string.Format("Unable to resolve type {0}.", type));

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
				component = this.componentBindings[constructedType].GetBinding(componentType).Resolve(this);
			
			// Default bindings
			if (component == null && this.globalBindings.HasBinding(componentType))
				component = this.globalBindings.GetBinding(componentType).Resolve(this);

			// Proxy to parent container if available
			if (component == null && this.parent != null)
				component = this.parent.ResolveComponentForInjection(constructedType, componentType);

			// Component not found
			if (component == null)
				component = this.Construct(componentType, null, null);

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
					if (this.resolveAttributeType.IsInstanceOfType(attribute))
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
			return this.globalBindings.Bind(type);
		}
						
		public bool HasBinding(Type type)
		{
			return this.globalBindings.HasBinding(type);
		}

		public IBindingSyntaxTo Rebind(Type type)
		{
			return this.globalBindings.Rebind(type);
		}

        /// <summary>
        /// Performs auto binding on types from the entry assembly using the default AutoBindStrategy.
        /// </summary>
        public void AutoBind()
        {
            this.AutoBind(Assembly.GetEntryAssembly(), new AutoBindStrategy());
        }

        /// <summary>
        /// Performs auto binding on types from the given assembly using the default AutoBindStrategy.
        /// </summary>
        public void AutoBind(Assembly assembly)
        {
            this.AutoBind(assembly, new AutoBindStrategy());
        }

		/// <summary>
		/// Performs auto binding on types from the given assembly using the given strategy.
		/// </summary>
		public void AutoBind(Assembly assembly, IAutoBindStrategy strategy)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

            if (strategy == null)
                throw new ArgumentNullException("strategy");

			Dictionary<Type, List<Type>> interfacesToTypesMap = new Dictionary<Type, List<Type>>();

			IEnumerable<Type> types = assembly.GetTypes();
            			
			foreach (Type type in types)
			{
				if (type.IsClass)
				{					
					foreach (Type interfaceType in type.GetInterfaces())
					{
						if (!interfacesToTypesMap.ContainsKey(interfaceType))
							interfacesToTypesMap.Add(interfaceType, new List<Type>());

						if (!interfacesToTypesMap[interfaceType].Contains(type))
							interfacesToTypesMap[interfaceType].Add(type);
					}
				}
                else if (type.IsInterface)
                {
                    if (!interfacesToTypesMap.ContainsKey(type))
                        interfacesToTypesMap.Add(type, new List<Type>());
                }
			}

			foreach (KeyValuePair<Type, List<Type>> interfaceToTypes in interfacesToTypesMap)
			{
                strategy.Bind(this, interfaceToTypes.Key, interfaceToTypes.Value);
			}
		}
	}
}
