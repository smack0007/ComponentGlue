using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue
{
    public interface IBindingSyntaxRoot : IBindingSyntaxFor, IBindingSyntaxBind
    {
    }

    public interface IBindingSyntaxAdd
    {
        /// <summary>
        /// Adds a type to a multiple component bind.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IBindingSyntaxAs<IBindingSyntaxAddOrWith> Add(Type type);
    }
        
    public interface IBindingSyntaxAddOrWith : IBindingSyntaxAdd, IBindingSyntaxWith<IBindingSyntaxAddOrWith>
    {
    }

    public interface IBindingSyntaxAs<TReturn>
    {
        /// <summary>
        /// Sets the bind type.
        /// </summary>
        /// <param name="bindType"></param>
        TReturn As(ComponentBindType bindType);

        /// <summary>
        /// Marks a binding as Transient. A new component will always be constructed.
        /// </summary>
        TReturn AsTransient();

        /// <summary>
        /// Marks a binding as Singleton. Component will be pulled from cache if available.
        /// </summary>
        TReturn AsSingleton();
    }
    
    public interface IBindingSyntaxBind
    {
        /// <summary>
        /// Adds a binding.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IBindingSyntaxTo Bind(Type type);

        /// <summary>
        /// Returns whether or not a binding has already been set.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool HasBinding(Type type);

        /// <summary>
        /// Adds a binding, overriding any existing binding.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IBindingSyntaxTo Rebind(Type type);
    }

    public interface IBindingSyntaxFor
    {
        /// <summary>
        /// Begins a specific binding.
        /// </summary>
        /// <param name="constructedType"></param>
        /// <returns></returns>
        IBindingSyntaxBind For(Type constructedType);
    }

    public interface IBindingSyntaxTo
    {
        /// <summary>
        /// Sets the component type.
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        IBindingSyntaxAs<IBindingSyntaxWith> To(Type componentType);

        /// <summary>
        /// Sets the component type to the interface type.
        /// </summary>
        /// <returns></returns>
        IBindingSyntaxAs<IBindingSyntaxWith> ToSelf();

        /// <summary>
        /// Sets the component as an already existing instance.
        /// </summary>
        /// <param name="component"></param>
        void ToConstant(object component);

        /// <summary>
        /// Sets the component as a factory method.
        /// </summary>
        /// <param name="component"></param>
        void ToFactoryMethod<T>(Func<IComponentResolver, T> factoryMethod);

        /// <summary>
        /// Sets the component as a multi component.
        /// </summary>
        /// <returns></returns>
        IBindingSyntaxAdd ToMultiple();

        /// <summary>
        /// Sets the component as a custom binding strategy.
        /// </summary>
        /// <param name="strategy"></param>
        void ToStrategy(IComponentBindingStrategy strategy);
    }

    public interface IBindingSyntaxWith<TReturn>
    {
        /// <summary>
        /// Adds a parameter to be used for constructing the component.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        TReturn WithConstructorParameter(string paramName, object paramValue);

        /// <summary>
        /// Adds a property value to be used for the component.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        TReturn WithPropertyValue(string propertyName, object propertyValue);

        /// <summary>
        /// Sets the component to have it's properties which are marked with the ResolveAttribute to be automatically resolved on construction.
        /// </summary>
        /// <returns></returns>
        TReturn WithPropertyResolution();
    }

    public interface IBindingSyntaxWith : IBindingSyntaxWith<IBindingSyntaxWith>
    {
    }
}
