using System;
using System.Collections.Generic;
using System.Reflection;

namespace ComponentGlue
{
	internal class ComponentBindingCollection : IBindingSyntaxBind, IEnumerable<ComponentBinding>
	{
		Dictionary<Type, ComponentBinding> bindings;

		public ComponentBindingCollection()
		{
			this.bindings = new Dictionary<Type, ComponentBinding>();
		}
		
		public ComponentBinding GetBinding(Type type)
		{
			if (!this.bindings.ContainsKey(type))
				throw new InvalidOperationException(string.Format("No binding for type {0}.", type));

			return this.bindings[type];
		}
				
		public IBindingSyntaxTo Bind(Type type)
		{
			if (this.bindings.ContainsKey(type))
				throw new BindingSyntaxException(string.Format("A binding has already been provided for the type {0}.", type));

			ComponentBinding binding = new ComponentBinding(type);
			this.bindings.Add(type, binding);

			return binding;
		}

		public bool HasBinding(Type type)
		{
			return this.bindings.ContainsKey(type);
		}
		
		public IBindingSyntaxTo Rebind(Type type)
		{
			if (!this.bindings.ContainsKey(type))
                throw new BindingSyntaxException(string.Format("No binding has been provided for the type {0}.", type));

            // Dispose of old binding.
            this.bindings[type].Dispose();

            ComponentBinding binding = new ComponentBinding(type);
            this.bindings[type] = binding;
            return binding;
		}

        public IEnumerator<ComponentBinding> GetEnumerator()
        {
            return this.bindings.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
