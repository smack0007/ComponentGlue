using System;
using System.Collections.Generic;

namespace ComponentGlue
{
    internal class ComponentBinding : IBindingSyntaxTo, IDisposable
	{
        IComponentBindingStrategy strategy;
        
		/// <summary>
		/// The type of component which is bound.
		/// </summary>
		public Type ComponentType
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
		}
                
        public void Dispose()
        {
            if (this.strategy != null && this.strategy is IDisposable)
            {
                ((IDisposable)this.strategy).Dispose();
            }
        }

        public object Resolve(ComponentContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            if (this.strategy == null)
                throw new ComponentResolutionException("Binding has a null strategy.");

            object concrete = this.strategy.Resolve(container);

            if (concrete == null)
                throw new ComponentResolutionException(string.Format("Strategy type {0} returned null.", this.strategy.GetType()));

            if (!this.ComponentType.IsAssignableFrom(concrete.GetType()))
                throw new ComponentResolutionException(string.Format("Strategy type {0} returned type {1} which is not assignable to {2}.", this.strategy.GetType(), concrete.GetType(), this.ComponentType));

            return concrete;
        }
        
		private void EnsureComponentTypeIsAssignableFromConcreteType(Type concrete)
		{
			if (!this.ComponentType.IsAssignableFrom(concrete))
				throw new BindingSyntaxException(string.Format("Type {0} is not assignable to type {1}.", concrete, this.ComponentType));
		}

        private IBindingSyntaxAs<IBindingSyntaxWith> ConstructSimpleStrategy(Type concreteType)
        {
            this.EnsureComponentTypeIsAssignableFromConcreteType(concreteType);

            this.strategy = new SimpleComponentBindingStrategy(this, concreteType);

            return (IBindingSyntaxAs<IBindingSyntaxWith>)this.strategy;
        }

        public IBindingSyntaxAs<IBindingSyntaxWith> To(Type concreteType)
		{
            return this.ConstructSimpleStrategy(concreteType);
		}

        public IBindingSyntaxAs<IBindingSyntaxWith> ToSelf()
		{
            return this.ConstructSimpleStrategy(this.ComponentType);
		}

		public void ToConstant(object value)
		{
			if (value != null)
				this.EnsureComponentTypeIsAssignableFromConcreteType(value.GetType());

            this.strategy = new ConstantComponentBindingStrategy(value);
		}

		public void ToFactoryMethod<T>(Func<IComponentResolver, T> factoryMethod)
		{
			if (factoryMethod == null)
				throw new ArgumentNullException("facotryMethod");

			this.EnsureComponentTypeIsAssignableFromConcreteType(typeof(T));

            this.strategy = new FactoryMethodComponentBindingStrategy<T>(factoryMethod);
		}

        public IBindingSyntaxAdd ToMultiple()
        {
            if (!this.ComponentType.IsArray)
                throw new BindingSyntaxException("Component type must be an array type in order to use ToMultiple().");

            this.strategy = new MultiComponentBindingStrategy(this);

            return (IBindingSyntaxAdd)this.strategy;
        }

        public void ToStrategy(IComponentBindingStrategy strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException("strategy");

            this.strategy = strategy;
        }
    }
}
