using System;
using ComponentGlue;

namespace ComponentGlue.Tests.Classes
{
	[DefaultComponent(typeof(IBar))]
	public class Bar3 : IBar
	{
		public IBaz Baz
		{
			get;
			private set;
		}

		[Resolve]
		public Bar3(IBaz baz)
		{
			this.Baz = baz;
		}
	}
}
