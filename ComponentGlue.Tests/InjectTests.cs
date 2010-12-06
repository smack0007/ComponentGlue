using System;
using System.Reflection;
using NUnit.Framework;
using ComponentGlue.Framework;
using ComponentGlue.Tests.Classes;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class InjectsTests
	{
		[Test]
		public void InjectPropertiesWithDefaultBindings()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IFoo>().To<Foo>();
			kernel.Bind<IBar>().To<Bar2>();

			var instance = new PropertyInject();
			kernel.Inject(instance);

			Assert.NotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.NotNull(instance.Bar);
			Assert.IsInstanceOf<Bar2>(instance.Bar);
		}

		[Test]
		public void InjectPropertiesWithSpecificBindings()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().To<Bar1>();
			kernel.For<PropertyInject>().Bind<IFoo>().To<Foo>();
			kernel.For<PropertyInject>().Bind<IBar>().To<Bar2>();

			var instance = new PropertyInject();
			kernel.Inject(instance);

			Assert.NotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.NotNull(instance.Bar);
			Assert.IsInstanceOf<Bar2>(instance.Bar);
		}

		[Test]
		public void InjectPropertiesWithSpecificBindingsOverrideDefaultBindings()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IFoo>().To<Foo>();
			kernel.Bind<IBar>().To<Bar1>();
			kernel.For<PropertyInject>().Bind<IBar>().To<Bar2>();

			var instance = new PropertyInject();
			kernel.Inject(instance);

			Assert.NotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.NotNull(instance.Bar);
			Assert.IsInstanceOf<Bar2>(instance.Bar);
		}
	}
}