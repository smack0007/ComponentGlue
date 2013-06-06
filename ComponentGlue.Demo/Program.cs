﻿using System;
using ComponentGlue;

namespace ComponentGlue.Demo
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			ComponentContainer container = new ComponentContainer();
            
            container.Bind<IBar>().To<Bar>().AsTransient();

            container.Bind<Foo>().ToSelf().AsSingleton();

            container.For<Foo>().Bind<IBar>().To<OtherBar>().AsSingleton();
                        
			Foo foo = container.Resolve<Foo>();
            Foo foo2 = container.Resolve<Foo>();
            IBar bar = container.Resolve<IBar>();

            Console.WriteLine("typeof(foo.Bar) : {0}", foo.Bar.GetType());
            Console.WriteLine("foo == foo2 : {0}", foo == foo2);
            Console.WriteLine("typeof(bar) : {0}", bar.GetType());
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
		}
	}
}
