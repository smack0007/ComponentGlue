using System;

namespace ComponentGlue.Framework.BindingSyntax
{
	public interface IBindingSyntaxAs
	{
		/// <summary>
		/// Marks a binding as shared. Component will be pulled from cache if available.
		/// </summary>
		void AsShared();

		/// <summary>
		/// Marks a binding as new. A new component will always be constructed.
		/// </summary>
		void AsNew();
	}
}
