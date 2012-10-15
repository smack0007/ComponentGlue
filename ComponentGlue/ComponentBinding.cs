using System;
using ComponentGlue.BindingSyntax;

namespace ComponentGlue
{
	public class ComponentBinding : IBindingSyntaxTo, IBindingSyntaxAs
	{
		/// <summary>
		/// The type of interface which is bound.
		/// </summary>
		public Type InterfaceType
		{
			get;
			private set;
		}

		/// <summary>
		/// The concrete type to which the interface is bound.
		/// </summary>
		public Type ComponentType
		{
			get;
			private set;
		}

		/// <summary>
		/// The type of binding.
		/// </summary>
		public ComponentBindType Type
		{
			get;
			private set;
		}

		/// <summary>
		/// The bound constant object.
		/// </summary>
		public object Constant
		{
			get;
			private set;
		}

		/// <summary>
		/// The factory method.
		/// </summary>
		public Func<IComponentResolver, object> FactoryMethod
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="interfaceType"></param>
		public ComponentBinding(Type interfaceType)
		{
			this.InterfaceType = interfaceType;
			this.ComponentType = interfaceType;
			this.Type = ComponentBindType.Transient;
		}

		private void EnsureComponentIsAssignableToInterface(Type componentType)
		{
			if (!this.InterfaceType.IsAssignableFrom(componentType))
				throw new BindingSyntaxException(string.Format("Type {0} is not assignable to type {1}.", componentType, this.InterfaceType));
		}

		public IBindingSyntaxAs To(Type componentType)
		{
			if (this.Constant != null || this.FactoryMethod != null)
				throw new InvalidOperationException("ComponentType may not be modified once a Constant or FactoryMethod is assigned.");

			this.EnsureComponentIsAssignableToInterface(componentType);
			
			this.ComponentType = componentType;
			return this;
		}
				
		public IBindingSyntaxAs ToSelf()
		{
			this.ComponentType = this.InterfaceType;
			return this;
		}

		public void ToConstant(object value)
		{
			if (value != null)
				this.EnsureComponentIsAssignableToInterface(value.GetType());

			this.Type = ComponentBindType.Constant;
			this.Constant = value;
		}

		public void ToFactoryMethod<T>(Func<IComponentResolver, T> factoryMethod)
		{
			if (factoryMethod == null)
				throw new ArgumentNullException("facotryMethod");

			this.EnsureComponentIsAssignableToInterface(typeof(T));

			this.Type = ComponentBindType.FactoryMethod;
			this.FactoryMethod = (container) => { return (object)factoryMethod(container); };
		}

		public void As(ComponentBindType bindType)
		{
			if (bindType == ComponentBindType.Constant)
				throw new BindingSyntaxException("ComponentBindType.Constant not valid for the As() method.");

			if (bindType == ComponentBindType.FactoryMethod)
				throw new BindingSyntaxException("ComponentBindType.FactoryMethod not valid for the As() method.");

			this.Type = bindType;
		}

		public void AsSingleton()
		{
			this.Type = ComponentBindType.Singleton;
		}

		public void AsTransient()
		{
			this.Type = ComponentBindType.Transient;
		}
	}
}
