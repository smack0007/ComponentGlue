using System;
using ComponentGlue;

namespace ComponentGlue.Demo
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			ComponentContainer container = new ComponentContainer();
            container.Bind<IFoo>().To<Foo>().WithConstructorParameter("name", "Fred");
            container.Bind<IBar>().To<Bar1>();

			IFoo foo = container.Resolve<IFoo>();
		}
	}
}
