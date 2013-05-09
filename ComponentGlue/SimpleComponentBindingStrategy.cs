using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue
{
    internal class SimpleComponentBindingStrategy : IComponentBindingStrategy, IBindingSyntaxAs, IBindingSyntaxWith
    {
        ComponentBinding binding;
        Type type;
        bool isSingleton;
        object instance;
        Dictionary<string, object> constructorParameters;

        public SimpleComponentBindingStrategy(ComponentBinding binding, Type type)
        {
            if (binding == null)
                throw new ArgumentNullException("binding");

            if (type == null)
                throw new ArgumentNullException("type");

            this.binding = binding;
            this.type = type;
        }

        public object Resolve(IComponentContainer container)
        {
            if (!this.isSingleton || this.instance == null)
            {
                // The order here is important for resolving circular dependencies.
                this.instance = container.Construct(this.type, this.constructorParameters);
                container.ResolveProperties(this.instance);
            }

            return this.instance;
        }

        public IBindingSyntaxWith As(ComponentBindType bindType)
        {
            this.isSingleton = bindType == ComponentBindType.Singleton;
            return this;
        }

        public IBindingSyntaxWith AsTransient()
        {
            return this.As(ComponentBindType.Transient);
        }

        public IBindingSyntaxWith AsSingleton()
        {
            return this.As(ComponentBindType.Singleton);
        }

        public IBindingSyntaxWith WithConstructorParameter(string paramName, object paramValue)
        {
            if (this.constructorParameters == null)
                this.constructorParameters = new Dictionary<string, object>();

            if (this.constructorParameters.ContainsKey(paramName))
                throw new BindingSyntaxException(string.Format("Parameter \"{0}\" is already set for the component type \"{1}\".", paramName, this.binding.ComponentType));

            this.constructorParameters[paramName] = paramValue;

            return this;
        }
    }
}
