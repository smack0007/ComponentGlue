using System;
using System.Collections.Generic;

namespace ComponentGlue
{
	/// <summary>
	/// Interface for component repositories.
	/// </summary>
	public interface IComponentContainer : IComponentResolver, IBindingSyntaxRoot
	{
        object Construct(Type type, IDictionary<string, object> parameters);
	}
}
