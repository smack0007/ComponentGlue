using System;
using ComponentGlue.Framework;

namespace ComponentGlue.Tests.Classes
{
	public class PropertyInject
	{
		[Inject]
		public IFoo Foo
		{
			get;
			private set;
		}

		[Inject]
		public IBar Bar
		{
			get;
			private set;
		}
	}
}
