using System;
using ComponentGlue;

namespace ComponentGlue.Tests.Classes
{
	public class Bar1 : IBar
	{
		[Resolve]
		public Bar1()
		{
		}
	}
}
