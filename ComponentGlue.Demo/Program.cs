using System;
using ComponentGlue.Framework;

namespace ComponentGlue.Demo
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind();

			IFoo foo = container.Get<IFoo>();
		}
	}
}
