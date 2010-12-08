using System;
using System.Reflection;
using NUnit.Framework;
using ComponentGlue.Framework;
using ComponentGlue.Tests.Classes;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class InjectTests
	{
		[Test]
		public void InjectPropertiesWithDefaultBindings()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IFoo>().To<Foo>();
			container.Bind<IBar>().To<Bar2>();

			var instance = new PropertyInject();
			container.Inject(instance);

			Assert.NotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.NotNull(instance.Bar);
			Assert.IsInstanceOf<Bar2>(instance.Bar);
		}

		[Test]
		public void InjectPropertiesWithSpecificBindings()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>();
			container.For<PropertyInject>().Bind<IFoo>().To<Foo>();
			container.For<PropertyInject>().Bind<IBar>().To<Bar2>();

			var instance = new PropertyInject();
			container.Inject(instance);

			Assert.NotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.NotNull(instance.Bar);
			Assert.IsInstanceOf<Bar2>(instance.Bar);
		}

		[Test]
		public void InjectPropertiesWithSpecificBindingsOverrideDefaultBindings()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IFoo>().To<Foo>();
			container.Bind<IBar>().To<Bar1>();
			container.For<PropertyInject>().Bind<IBar>().To<Bar2>();

			var instance = new PropertyInject();
			container.Inject(instance);

			Assert.NotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.NotNull(instance.Bar);
			Assert.IsInstanceOf<Bar2>(instance.Bar);
		}
	}
}