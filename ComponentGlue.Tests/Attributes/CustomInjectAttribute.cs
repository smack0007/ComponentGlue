using System;

namespace ComponentGlue.Tests.Attributes
{
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class CustomInjectAttribute : Attribute
	{
	}
}
