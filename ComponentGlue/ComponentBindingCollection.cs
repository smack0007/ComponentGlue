using System;
using System.Collections.Generic;
using System.Reflection;
using ComponentGlue.BindingSyntax;

namespace ComponentGlue
{
	public class ComponentBindingCollection : IBindingSyntaxBind
	{
		Dictionary<Type, ComponentBinding> bindings;

		public ComponentBindingCollection()
		{
			this.bindings = new Dictionary<Type, ComponentBinding>();
		}
		
		public ComponentBinding GetBinding(Type interfaceType)
		{
			if(!this.bindings.ContainsKey(interfaceType))
				throw new InvalidOperationException("No binding for interface type " + interfaceType);

			return this.bindings[interfaceType];
		}
				
		public IBindingSyntaxTo Bind(Type interfaceType)
		{
			if(this.bindings.ContainsKey(interfaceType))
				throw new InvalidOperationException("A binding has already been provided for the interface type " + interfaceType);

			ComponentBinding binding = new ComponentBinding(interfaceType);
			this.bindings.Add(interfaceType, binding);

			return binding;
		}

		public bool HasBinding(Type interfaceType)
		{
			return this.bindings.ContainsKey(interfaceType);
		}
		
		public IBindingSyntaxTo Rebind(Type interfaceType)
		{
			if(!this.bindings.ContainsKey(interfaceType))
			{
				ComponentBinding binding = new ComponentBinding(interfaceType);
				this.bindings.Add(interfaceType, binding);
			}

			return this.bindings[interfaceType];
		}
	}
}
