using System;
using ComponentGlue.Framework.BindingSyntax;

namespace ComponentGlue.Framework
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
		public Func<object> Method
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

		public IBindingSyntaxAs To(Type componentType)
		{
			if(this.Constant != null)
				throw new InvalidOperationException("ComponentType may not be modified once a Component is assigned.");

			if(!this.InterfaceType.IsAssignableFrom(componentType))
				throw new InvalidOperationException(componentType + " does not implement " + this.InterfaceType);
			
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
			if(value != null && !this.InterfaceType.IsAssignableFrom(value.GetType()))
				throw new InvalidOperationException("Value is not an instance of " + this.ComponentType);

			this.Type = ComponentBindType.Constant;
			this.Constant = value;
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
