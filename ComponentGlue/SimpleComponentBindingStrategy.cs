using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue
{
    internal class SimpleComponentBindingStrategy : IComponentBindingStrategy, IBindingSyntaxAs<IBindingSyntaxWith>, IBindingSyntaxWith, IDisposable
    {
        ComponentBinding binding;
        Type type;
        
        bool isSingleton;
        object instance;
        
        Dictionary<string, object> constructorParameters;
        Dictionary<string, object> propertyValues;

        bool doResolveProperties;

        public SimpleComponentBindingStrategy(ComponentBinding binding, Type type)
        {
            if (binding == null)
                throw new ArgumentNullException("binding");

            if (type == null)
                throw new ArgumentNullException("type");

            this.binding = binding;
            this.type = type;
        }

        public void Dispose()
        {
            if (this.instance != null && this.instance is IDisposable)
            {
                ((IDisposable)this.instance).Dispose();
            }
        }

        public object Resolve(ComponentContainer container)
        {
            if (!this.isSingleton || this.instance == null)
            {
                // The order here is important for resolving circular dependencies.
                this.instance = container.Construct(this.type, this.constructorParameters, this.propertyValues);
                
                if (this.doResolveProperties)
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

        public IBindingSyntaxWith WithPropertyValue(string propertyName, object propertyValue)
        {
            if (this.propertyValues == null)
                this.propertyValues = new Dictionary<string, object>();

            if (this.propertyValues.ContainsKey(propertyName))
                throw new BindingSyntaxException(string.Format("Property value \"{0}\" is already set for the component type \"{1}\".", propertyName, this.binding.ComponentType));

            this.propertyValues[propertyName] = propertyValue;

            return this;
        }

        public IBindingSyntaxWith WithPropertyResolution()
        {
            this.doResolveProperties = true;
            return this;
        }

        public IBindingSyntaxWith WithoutPropertyResolution()
        {
            this.doResolveProperties = false;
            return this;
        }
    }
}
