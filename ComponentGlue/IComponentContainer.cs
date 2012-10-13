using System;
using ComponentGlue.BindingSyntax;

namespace ComponentGlue
{
	/// <summary>
	/// Interface for component repositories.
	/// </summary>
	public interface IComponentContainer : IComponentResolver, IBindingSyntaxRoot
	{
	}
}
