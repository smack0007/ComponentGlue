using System;
using ComponentGlue.BindingSyntax;
using System.Collections.Generic;

namespace ComponentGlue
{
	internal class ComponentBinding : IBindingSyntaxTo, IBindingSyntaxAsWith
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

		public IBindingSyntaxAsWith To(Type componentType)
		{
			this.EnsureComponentTypeIsAssignableFromConcreteType(componentType);
			
			this.ConcreteType = componentType;
			return this;
		}
				
		public IBindingSyntaxAsWith ToSelf()
		{
			this.ConcreteType = this.ComponentType;
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

		public IBindingSyntaxAsWith As(ComponentBindType bindType)
		{
            if (bindType == ComponentBindType.Singleton ||
                bindType == ComponentBindType.Transient)
            {
                this.BindType = bindType;
            }
            else
            {
                throw new BindingSyntaxException(string.Format("ComponentBindType.{0} not valid for the As() method.", bindType.ToString()));
            }

            return this;
		}

        public IBindingSyntaxAsWith AsSingleton()
		{
			this.BindType = ComponentBindType.Singleton;
            return this;
		}

        public IBindingSyntaxAsWith AsTransient()
		{
			this.BindType = ComponentBindType.Transient;
            return this;
		}

        public IBindingSyntaxAsWith WithConstructorParameter(string paramName, object paramValue)
        {
            if (this.ConstructorParameters == null)
                this.ConstructorParameters = new Dictionary<string, object>();

            if (this.ConstructorParameters.ContainsKey(paramName))
                throw new BindingSyntaxException(string.Format("Parameter \"{0}\" is already set for the component type \"{1\".", paramName, this.ComponentType));

            this.ConstructorParameters[paramName] = paramValue;

            return this;
        }
	}
}
