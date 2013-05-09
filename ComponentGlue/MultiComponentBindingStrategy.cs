﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue
{
    internal class MultiComponentBindingStrategy : IComponentBindingStrategy, IBindingSyntaxAdd, IBindingSyntaxAs<IBindingSyntaxAddOrWith>, IBindingSyntaxAddOrWith
    {
        ComponentBinding binding;
        List<IComponentBindingStrategy> strategies;

        public MultiComponentBindingStrategy(ComponentBinding binding)
        {
            if (binding == null)
                throw new ArgumentNullException("binding");

            this.binding = binding;
            this.strategies = new List<IComponentBindingStrategy>();
        }

        public object Resolve(IComponentContainer container)
        {
            var components = Array.CreateInstance(this.binding.ComponentType.GetElementType(), this.strategies.Count);

            for (int i = 0; i < this.strategies.Count; i++)
            {
                components.SetValue(this.strategies[i].Resolve(container), i);
            }

            return components;
        }

        private IComponentBindingStrategy GetCurrentStrategy()
        {          
            return this.strategies[this.strategies.Count - 1];
        }

        public IBindingSyntaxAs<IBindingSyntaxAddOrWith> Add(Type type)
        {
            this.strategies.Add(new SimpleComponentBindingStrategy(this.binding, type));
            return this;
        }

        public IBindingSyntaxAddOrWith As(ComponentBindType bindType)
        {
            ((IBindingSyntaxAs)this.GetCurrentStrategy()).As(bindType);
            return this;
        }

        public IBindingSyntaxAddOrWith AsTransient()
        {
            ((IBindingSyntaxAs)this.GetCurrentStrategy()).AsTransient();
            return this;
        }

        public IBindingSyntaxAddOrWith AsSingleton()
        {
            ((IBindingSyntaxAs)this.GetCurrentStrategy()).AsSingleton();
            return this;
        }

        public IBindingSyntaxAddOrWith WithConstructorParameter(string paramName, object paramValue)
        {
            ((IBindingSyntaxWith)this.GetCurrentStrategy()).WithConstructorParameter(paramName, paramValue);
            return this;
        }
    }
}
