using System;
using ComponentGlue;

namespace ComponentGlue.Demo
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind();

			IFoo foo = container.Get<IFoo>();

			Func<IFoo> factory = container.Get<Func<IFoo>>();
			IFoo foo2 = factory();
		}
	}
}
