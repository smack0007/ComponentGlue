using System;

namespace ComponentGlue.BindingSyntax
{
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
}
