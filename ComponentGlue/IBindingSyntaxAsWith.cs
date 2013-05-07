using System;

namespace ComponentGlue
{
	public interface IBindingSyntaxAsWith<TReturn>
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

        /// <summary>
        /// Adds a parameter to be used for constructing the component.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        TReturn WithConstructorParameter(string paramName, object paramValue);
	}
}
