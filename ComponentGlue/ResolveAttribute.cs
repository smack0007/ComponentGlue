using System;

namespace ComponentGlue
{
	/// <summary>
	/// Marks a constructor or property as injected.
	/// </summary>
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class ResolveAttribute : Attribute
	{
	}
}
