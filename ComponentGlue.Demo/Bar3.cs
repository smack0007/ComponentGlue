using System;
using ComponentGlue.Framework;

namespace ComponentGlue.Demo
{
	[DefaultComponent(typeof(IBar))]
	public class Bar3 : IBar
	{
		public IBaz Baz
		{
			get;
			private set;
		}

		[InjectComponent]
		public Bar3(IBaz baz)
		{
			this.Baz = baz;
		}
	}
}
