using System;
using ComponentGlue.Tests.Attributes;

namespace ComponentGlue.Tests.Classes
{
	public class CustomAttributePropertyInject
	{
		[CustomResolve]
		public IFoo Foo
		{
			get;
			private set;
		}

		[CustomResolve]
		public IBar Bar
		{
			get;
			private set;
		}
	}
}
