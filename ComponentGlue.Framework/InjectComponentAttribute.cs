using System;

namespace ComponentGlue.Framework
{
	/// <summary>
	/// Marks a constructor or property as injected.
	/// </summary>
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class InjectComponentAttribute : Attribute
	{
	}
}
