using System;
using System.Reflection;
using NUnit.Framework;
using ComponentGlue;
using ComponentGlue.Tests.Attributes;
using ComponentGlue.Tests.Classes;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class ConfigurationTests
	{
		[Test, ExpectedException]
		public void AssigningInjectAttributeTypeNonAttributeTypeThrowsException()
		{
			ComponentContainer container = new ComponentContainer();
			container.InjectAttributeType = typeof(ConfigurationTests);
		}

		[Test]
		public void PropertiesCanBeInjectedWhenUsingCustomAttribute()
		{
			ComponentContainer container = new ComponentContainer();
			container.InjectAttributeType = typeof(CustomInjectAttribute);
			container.Bind<IFoo>().To<Foo>();
			container.Bind<IBar>().To<Bar1>();

			CustomAttributePropertyInject instance = container.Get<CustomAttributePropertyInject>();

			Assert.IsNotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.IsNotNull(instance.Bar);
			Assert.IsInstanceOf<Bar1>(instance.Bar);
		}
	}
}