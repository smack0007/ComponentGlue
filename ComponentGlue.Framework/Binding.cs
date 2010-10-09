using System;
using ComponentGlue.Framework.BindingSyntax;

namespace ComponentGlue.Framework
{
	public class Binding : IBindingSyntaxTo, IBindingSyntaxAs
	{
		public Type InterfaceType
		{
			get;
			private set;
		}

		public Type ComponentType
		{
			get;
			private set;
		}

		public BindType Type
		{
			get;
			private set;
		}

		public object Constant
		{
			get;
			private set;
		}

		public Func<Type, Type, object> Method
		{
			get;
			private set;
		}

		public Binding(Type interfaceType)
		{
			this.InterfaceType = interfaceType;
			this.ComponentType = interfaceType;
			this.Type = BindType.Shared;
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

		public IBindingSyntaxAs To<TComponentType>()
		{
			return To(typeof(TComponentType));
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

			this.Type = BindType.Constant;
			this.Constant = value;
		}

		public void ToMethod(Func<Type, Type, object> factory)
		{
			if(factory == null)
				throw new ArgumentNullException("factory");

			this.Type = BindType.Method;
			this.Method = factory;
		}

		public void AsShared()
		{
			this.Type = BindType.Shared;
		}

		public void AsNew()
		{
			this.Type = BindType.New;
		}
	}
}
