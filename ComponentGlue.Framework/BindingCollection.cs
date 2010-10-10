using System;
using System.Collections.Generic;
using System.Reflection;
using ComponentGlue.Framework.BindingSyntax;

namespace ComponentGlue.Framework
{
	public class BindingCollection : IBindingSyntaxBind
	{
		Dictionary<Type, Binding> bindings;

		public BindingCollection()
		{
			this.bindings = new Dictionary<Type, Binding>();
		}
		
		public Binding GetBinding(Type interfaceType)
		{
			if(!this.bindings.ContainsKey(interfaceType))
				throw new InvalidOperationException("No binding for interface type " + interfaceType);

			return this.bindings[interfaceType];
		}
				
		public IBindingSyntaxTo Bind(Type interfaceType)
		{
			if(this.bindings.ContainsKey(interfaceType))
				throw new InvalidOperationException("A binding has already been provided for the interface type " + interfaceType);

			Binding binding = new Binding(interfaceType);
			this.bindings.Add(interfaceType, binding);

			return binding;
		}

		public IBindingSyntaxTo Bind<TInterfaceType>()
		{
			return Bind(typeof(TInterfaceType));
		}

		public bool HasBinding(Type interfaceType)
		{
			return this.bindings.ContainsKey(interfaceType);
		}

		public bool HasBinding<TInterfaceType>()
		{
			return HasBinding(typeof(TInterfaceType));
		}

		public IBindingSyntaxTo Rebind(Type interfaceType)
		{
			if(!this.bindings.ContainsKey(interfaceType))
			{
				Binding binding = new Binding(interfaceType);
				this.bindings.Add(interfaceType, binding);
			}

			return this.bindings[interfaceType];
		}

		public IBindingSyntaxTo Rebind<TInterfaceType>()
		{
			return Rebind(typeof(TInterfaceType));
		}
	}
}
