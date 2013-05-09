using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue
{
    public interface IBindingSyntaxWith<TReturn>
    {
        /// <summary>
        /// Adds a parameter to be used for constructing the component.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        TReturn WithConstructorParameter(string paramName, object paramValue);
    }

    public interface IBindingSyntaxWith : IBindingSyntaxWith<IBindingSyntaxWith>
    {
    }
}
