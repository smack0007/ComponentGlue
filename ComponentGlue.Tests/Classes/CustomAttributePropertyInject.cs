using System;
using ComponentGlue.Tests.Attributes;

namespace ComponentGlue.Tests.Classes
{
	public class CustomAttributePropertyInject
	{
		[CustomInject]
		public IFoo Foo
		{
			get;
			private set;
		}

		[CustomInject]
		public IBar Bar
		{
			get;
			private set;
		}
	}
}
