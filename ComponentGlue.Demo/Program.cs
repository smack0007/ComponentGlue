using System;
using ComponentGlue.Framework;

namespace ComponentGlue.Demo
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			Kernel kernel = new Kernel();
			kernel.AutoBind();

			IFoo foo = kernel.Get<IFoo>();
		}
	}
}
