using System;
using ComponentGlue.Framework.BindingSyntax;

namespace ComponentGlue.Framework
{
	/// <summary>
	/// Interface for IoC Containers.
	/// </summary>
	public interface IComponentContainer : IComponentResolver, IBindingSyntaxRoot
	{
	}
}
