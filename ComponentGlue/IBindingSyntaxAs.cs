using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue
{
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

    public interface IBindingSyntaxAs : IBindingSyntaxAs<IBindingSyntaxWith>
    {
    }
}
