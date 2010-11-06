using System;

namespace ComponentGlue.Framework.BindingSyntax
{
	public interface IBindingSyntaxAs
	{
		/// <summary>
		/// Marks a binding as transient. A new component will always be constructed.
		/// </summary>
		void AsTransient();

		/// <summary>
		/// Marks a binding as a singleton. Component will be pulled from cache if available.
		/// </summary>
		void AsSingleton();
	}
}
