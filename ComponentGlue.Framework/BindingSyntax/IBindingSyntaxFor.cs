using System;

namespace ComponentGlue.Framework.BindingSyntax
{
	public interface IBindingSyntaxFor
	{
		/// <summary>
		/// Begins a specific binding.
		/// </summary>
		/// <param name="constructedType"></param>
		/// <returns></returns>
		IBindingSyntaxBind For(Type constructedType);

		/// <summary>
		/// Begins a specific binding.
		/// </summary>
		/// <typeparam name="TConstructedType"></typeparam>
		/// <returns></returns>
		IBindingSyntaxBind For<TConstructedType>();
	}
}
