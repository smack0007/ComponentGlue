using System;

namespace ComponentGlue.BindingSyntax
{
	public interface IBindingSyntaxBind
	{
		/// <summary>
		/// Adds a binding.
		/// </summary>
		/// <param name="interfaceType"></param>
		/// <returns></returns>
		IBindingSyntaxTo Bind(Type interfaceType);

		/// <summary>
		/// Returns whether or not a binding has already been set.
		/// </summary>
		/// <param name="interfaceType"></param>
		/// <returns></returns>
		bool HasBinding(Type interfaceType);

		/// <summary>
		/// Adds a binding, overriding any existing binding.
		/// </summary>
		/// <param name="interfaceType"></param>
		/// <returns></returns>
		IBindingSyntaxTo Rebind(Type interfaceType);
	}
}
