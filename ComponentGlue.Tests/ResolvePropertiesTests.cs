using System;
using System.Reflection;
using NUnit.Framework;
using ComponentGlue;
using ComponentGlue.Tests.Classes;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class ResolvePropertiesTests
	{
		[Test]
		public void With_Default_Bindings()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IFoo>().To<Foo>();
			container.Bind<IBar>().To<Bar2>();

			var instance = new PropertyInject();
			container.ResolveProperties(instance);

			Assert.NotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.NotNull(instance.Bar);
			Assert.IsInstanceOf<Bar2>(instance.Bar);
		}

		[Test]
        public void With_Specific_Bindings()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>();
			container.For<PropertyInject>().Bind<IFoo>().To<Foo>();
			container.For<PropertyInject>().Bind<IBar>().To<Bar2>();

			var instance = new PropertyInject();
			container.ResolveProperties(instance);

			Assert.NotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.NotNull(instance.Bar);
			Assert.IsInstanceOf<Bar2>(instance.Bar);
		}

		[Test]
		public void Specific_Bindings_Override_Default_Bindings()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IFoo>().To<Foo>();
			container.Bind<IBar>().To<Bar1>();
			container.For<PropertyInject>().Bind<IBar>().To<Bar2>();

			var instance = new PropertyInject();
			container.ResolveProperties(instance);

			Assert.NotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.NotNull(instance.Bar);
			Assert.IsInstanceOf<Bar2>(instance.Bar);
		}
	}
}