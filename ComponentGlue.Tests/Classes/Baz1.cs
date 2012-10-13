using System;
using ComponentGlue;

namespace ComponentGlue.Tests.Classes
{
	[DefaultComponent(typeof(IBaz))]
	public class Baz1 : IBaz
	{
		public Baz1()
		{
		}
	}
}
