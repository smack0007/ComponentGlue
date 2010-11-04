using System;
using ComponentGlue.Framework;

namespace ComponentGlue.Demo
{
	[DefaultComponent(typeof(IBaz))]
	public class Baz1 : IBaz
	{
		public Baz1()
		{
		}
	}
}
