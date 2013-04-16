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

			IFoo foo = container.Resolve<IFoo>();

			Func<IFoo> factory = container.Resolve<Func<IFoo>>();
			IFoo foo2 = factory();
		}
	}
}
