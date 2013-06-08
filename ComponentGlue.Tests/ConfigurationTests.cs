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
		public void Assigning_InjectAttributeType_To_Null_Throws_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			container.ResolveAttributeType = null;
		}

		[Test, ExpectedException]
		public void Assigning_InjectAttributeType_To_Non_Attribute_Type_Throws_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			container.ResolveAttributeType = typeof(ConfigurationTests);
		}

		[Test]
		public void Properties_Can_Be_Injected_When_Using_Custom_InjectAttribute()
		{
			ComponentContainer container = new ComponentContainer();
			container.ResolveAttributeType = typeof(CustomResolveAttribute);
			container.Bind<IFoo>().To<Foo1>();
			container.Bind<IBar>().To<Bar1>();

			CustomAttributePropertyInject instance = container.Resolve<CustomAttributePropertyInject>();

			Assert.IsNotNull(instance.Foo);
			Assert.IsInstanceOf<Foo1>(instance.Foo);

			Assert.IsNotNull(instance.Bar);
			Assert.IsInstanceOf<Bar1>(instance.Bar);
		}
    }
}