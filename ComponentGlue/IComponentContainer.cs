using System;
using System.Collections.Generic;

namespace ComponentGlue
{
	/// <summary>
	/// Interface for component repositories.
	/// </summary>
	public interface IComponentContainer : IComponentResolver, IBindingSyntaxRoot
	{
	}
}
