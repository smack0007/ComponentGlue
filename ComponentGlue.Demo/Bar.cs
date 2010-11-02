using System;
using ComponentGlue.Framework;

namespace ComponentGlue.Demo
{
	[DefaultComponent(typeof(IBar))]
	public class Bar : IBar
	{
		[Inject]
		public Bar()
		{
		}
	}
}
