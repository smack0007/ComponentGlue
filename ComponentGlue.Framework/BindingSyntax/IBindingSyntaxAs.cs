using System;

namespace ComponentGlue.Framework.BindingSyntax
{
	public interface IBindingSyntaxAs
	{
		/// <summary>
		/// Sets the bind type.
		/// </summary>
		/// <param name="bindType"></param>
		void As(ComponentBindType bindType);

		/// <summary>
		/// Marks a binding as Transient. A new component will always be constructed.
		/// </summary>
		void AsTransient();

		/// <summary>
		/// Marks a binding as Singleton. Component will be pulled from cache if available.
		/// </summary>
		void AsSingleton();
	}
}
