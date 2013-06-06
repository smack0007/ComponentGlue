using System;
using ComponentGlue;

namespace ComponentGlue.Tests.Classes
{
	public class Bar2 : IBar
	{
		[Resolve]
		public Bar2()
		{
		}
	}
}
