using System;
using ComponentGlue.BindingSyntax;
using System.Collections.Generic;

namespace ComponentGlue
{
    internal class ComponentBinding : IBindingSyntaxTo, IBindingSyntaxAsWithSingle, IBindingSyntaxAsWithMultiple
	{
		/// <summary>
		/// The type of component which is bound.
		/// </summary>
		public Type ComponentType
		{
			get;
			private set;
		}

		/// <summary>
		/// The concrete type to which the component is bound.
		/// </summary>
        public Type ConcreteType
		{
			get;
			private set;
		}

		/// <summary>
		/// The type of binding.
		/// </summary>
        public ComponentBindType BindType
		{
			get;
			private set;
		}

		/// <summary>
		/// Data object related to the binding.
		/// </summary>
        public object Data
		{
			get;
			private set;
		}
        		
        public Dictionary<string, object> ConstructorParameters
        {
            get;
            private set;
        }

        /// <summary>
        /// Used if the binding is a singleton to store the result of the resolution.
        /// </summary>
        public object SingletonInstance
        {
            get;
            set;
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="componentType"></param>
		public ComponentBinding(Type componentType)
		{
			this.ComponentType = componentType;
			this.ConcreteType = componentType;
			this.BindType = ComponentBindType.Transient;
		}
                
		private void EnsureComponentTypeIsAssignableFromConcreteType(Type concrete)
		{
			if (!this.ComponentType.IsAssignableFrom(concrete))
				throw new BindingSyntaxException(string.Format("Type {0} is not assignable to type {1}.", concrete, this.ComponentType));
		}

		public IBindingSyntaxAsWithSingle To(Type componentType)
		{
			this.EnsureComponentTypeIsAssignableFromConcreteType(componentType);
			
			this.ConcreteType = componentType;
            this.BindType = ComponentBindType.Transient;

			return this;
		}

        public IBindingSyntaxAsWithSingle ToSelf()
		{
            // TODO: Ensure bind interface / abstract class to self is not allowed.
            
			this.ConcreteType = this.ComponentType;
            this.BindType = ComponentBindType.Transient;

			return this;
		}

		public void ToConstant(object value)
		{
			if (value != null)
				this.EnsureComponentTypeIsAssignableFromConcreteType(value.GetType());

			this.BindType = ComponentBindType.Constant;
			
            this.Data = value;
		}

		public void ToFactoryMethod<T>(Func<IComponentResolver, T> factoryMethod)
		{
			if (factoryMethod == null)
				throw new ArgumentNullException("facotryMethod");

			this.EnsureComponentTypeIsAssignableFromConcreteType(typeof(T));

			this.BindType = ComponentBindType.FactoryMethod;
			
            this.Data = new Func<IComponentContainer, object>((container) => { return (object)factoryMethod(container); });
		}

        public IBindingSyntaxAdd ToMultiple()
        {
            if (!this.ComponentType.IsArray)
                throw new BindingSyntaxException("Component type must be an array type in order to use ToMultiple().");

            this.BindType = ComponentBindType.Multiple;
            
            this.Data = new List<ComponentBinding>();

            return this;
        }

        IBindingSyntaxAsWithMultiple IBindingSyntaxAdd.Add(Type type)
        {
            ComponentBinding binding = new ComponentBinding(this.ComponentType);
            binding.ConcreteType = type;

            ((List<ComponentBinding>)this.Data).Add(binding);

            return this;
        }

        private ComponentBinding GetCurrentSubBinding()
        {
            var bindings = (List<ComponentBinding>)this.Data;
            return bindings[bindings.Count - 1];
        }

        private void AsInternal(ComponentBindType bindType)
        {
            if (this.BindType == ComponentBindType.Multiple)
            {
                this.GetCurrentSubBinding().BindType = bindType;
            }
            else
            {
                this.BindType = bindType;
            }
        }

        private void As(ComponentBindType bindType)
		{
            if (bindType == ComponentBindType.Singleton ||
                bindType == ComponentBindType.Transient)
            {
                this.AsInternal(bindType);
            }
            else
            {
                throw new BindingSyntaxException(string.Format("ComponentBindType.{0} not valid for the As() method.", bindType.ToString()));
            }
		}

        IBindingSyntaxAsWithSingle IBindingSyntaxAsWith<IBindingSyntaxAsWithSingle>.As(ComponentBindType bindType)
        {
            this.As(bindType);
            return this;
        }

        IBindingSyntaxAsWithMultiple IBindingSyntaxAsWith<IBindingSyntaxAsWithMultiple>.As(ComponentBindType bindType)
        {
            this.As(bindType);
            return this;
        }
                
        private void AsSingleton()
		{
            this.AsInternal(ComponentBindType.Singleton);
		}

        IBindingSyntaxAsWithSingle IBindingSyntaxAsWith<IBindingSyntaxAsWithSingle>.AsSingleton()
        {
            this.AsSingleton();
            return this;
        }

        IBindingSyntaxAsWithMultiple IBindingSyntaxAsWith<IBindingSyntaxAsWithMultiple>.AsSingleton()
        {
            this.AsSingleton();
            return this;
        }

        private void AsTransient()
		{
            this.AsInternal(ComponentBindType.Transient);
		}

        IBindingSyntaxAsWithSingle IBindingSyntaxAsWith<IBindingSyntaxAsWithSingle>.AsTransient()
        {
            this.AsTransient();
            return this;
        }

        IBindingSyntaxAsWithMultiple IBindingSyntaxAsWith<IBindingSyntaxAsWithMultiple>.AsTransient()
        {
            this.AsTransient();
            return this;
        }

        private void WithConstructorParameter(string paramName, object paramValue)
        {
            if (this.BindType == ComponentBindType.Multiple)
            {
                this.GetCurrentSubBinding().WithConstructorParameter(paramName, paramValue);
            }
            else
            {
                if (this.ConstructorParameters == null)
                    this.ConstructorParameters = new Dictionary<string, object>();

                if (this.ConstructorParameters.ContainsKey(paramName))
                    throw new BindingSyntaxException(string.Format("Parameter \"{0}\" is already set for the component type \"{1}\".", paramName, this.ComponentType));

                this.ConstructorParameters[paramName] = paramValue;
            }
        }

        IBindingSyntaxAsWithSingle IBindingSyntaxAsWith<IBindingSyntaxAsWithSingle>.WithConstructorParameter(string paramName, object paramValue)
        {
            this.WithConstructorParameter(paramName, paramValue);
            return this;
        }
       
        IBindingSyntaxAsWithMultiple IBindingSyntaxAsWith<IBindingSyntaxAsWithMultiple>.WithConstructorParameter(string paramName, object paramValue)
        {
            this.WithConstructorParameter(paramName, paramValue);
            return this;
        }
    }
}
