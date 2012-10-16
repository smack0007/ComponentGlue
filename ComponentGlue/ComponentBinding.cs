using System;
using ComponentGlue.BindingSyntax;

namespace ComponentGlue
{
	public class ComponentBinding : IBindingSyntaxTo, IBindingSyntaxAs
	{
		/// <summary>
		/// The type of component which is bound.
		/// </summary>
		internal Type ComponentType
		{
			get;
			private set;
		}

		/// <summary>
		/// The concrete type to which the component is bound.
		/// </summary>
		internal Type ConcreteType
		{
			get;
			private set;
		}

		/// <summary>
		/// The type of binding.
		/// </summary>
		internal ComponentBindType BindType
		{
			get;
			private set;
		}

		/// <summary>
		/// The bound constant object.
		/// </summary>
		internal object Constant
		{
			get;
			private set;
		}

		/// <summary>
		/// The factory method.
		/// </summary>
		internal Func<IComponentResolver, object> FactoryMethod
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

		private void EnsureComponentIsAssignableToInterface(Type componentType)
		{
			if (!this.ComponentType.IsAssignableFrom(componentType))
				throw new BindingSyntaxException(string.Format("Type {0} is not assignable to type {1}.", componentType, this.ComponentType));
		}

		public IBindingSyntaxAs To(Type componentType)
		{
			if (this.Constant != null || this.FactoryMethod != null)
				throw new InvalidOperationException("ComponentType may not be modified once a Constant or FactoryMethod is assigned.");

			this.EnsureComponentIsAssignableToInterface(componentType);
			
			this.ConcreteType = componentType;
			return this;
		}
				
		public IBindingSyntaxAs ToSelf()
		{
			this.ConcreteType = this.ComponentType;
			return this;
		}

		public void ToConstant(object value)
		{
			if (value != null)
				this.EnsureComponentIsAssignableToInterface(value.GetType());

			this.BindType = ComponentBindType.Constant;
			this.Constant = value;
		}

		public void ToFactoryMethod<T>(Func<IComponentResolver, T> factoryMethod)
		{
			if (factoryMethod == null)
				throw new ArgumentNullException("facotryMethod");

			this.EnsureComponentIsAssignableToInterface(typeof(T));

			this.BindType = ComponentBindType.FactoryMethod;
			this.FactoryMethod = (container) => { return (object)factoryMethod(container); };
		}

		public void As(ComponentBindType bindType)
		{
			if (bindType == ComponentBindType.Constant)
				throw new BindingSyntaxException("ComponentBindType.Constant not valid for the As() method.");

			if (bindType == ComponentBindType.FactoryMethod)
				throw new BindingSyntaxException("ComponentBindType.FactoryMethod not valid for the As() method.");

			this.BindType = bindType;
		}

		public void AsSingleton()
		{
			this.BindType = ComponentBindType.Singleton;
		}

		public void AsTransient()
		{
			this.BindType = ComponentBindType.Transient;
		}
	}
}
