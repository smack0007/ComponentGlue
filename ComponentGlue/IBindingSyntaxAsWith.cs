using System;

namespace ComponentGlue
{
	public interface IBindingSyntaxAsWith
	{
		/// <summary>
		/// Sets the bind type.
		/// </summary>
		/// <param name="bindType"></param>
        IBindingSyntaxAsWith As(ComponentBindType bindType);

		/// <summary>
		/// Marks a binding as Transient. A new component will always be constructed.
		/// </summary>
        IBindingSyntaxAsWith AsTransient();

		/// <summary>
		/// Marks a binding as Singleton. Component will be pulled from cache if available.
		/// </summary>
        IBindingSyntaxAsWith AsSingleton();

        /// <summary>
        /// Adds a parameter to be used for constructing the component.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        IBindingSyntaxAsWith WithConstructorParameter(string paramName, object paramValue);
	}
}
