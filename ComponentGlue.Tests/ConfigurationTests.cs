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
		public void AssigningInjectAttributeTypeToNullThrowsException()
		{
			ComponentContainer container = new ComponentContainer();
			container.InjectAttributeType = null;
		}

		[Test, ExpectedException]
		public void AssigningInjectAttributeTypeToNonAttributeTypeThrowsException()
		{
			ComponentContainer container = new ComponentContainer();
			container.InjectAttributeType = typeof(ConfigurationTests);
		}

		[Test]
		public void PropertiesCanBeInjectedWhenUsingCustomInjectAttribute()
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

		[Test, ExpectedException]
		public void AssigningDefaultComponentAttributeTypeToNullThrowsException()
		{
			ComponentContainer container = new ComponentContainer();
			container.DefaultComponentAttributeType = null;
		}

		[Test, ExpectedException]
		public void AssigningDefaultComponentAttributeTypeToNonAttributeTypeThrowsException()
		{
			ComponentContainer container = new ComponentContainer();
			container.DefaultComponentAttributeType = typeof(ConfigurationTests);
		}

		[Test, ExpectedException]
		public void AssigningDefaultComponentAttributeTypeWhichDoesNotImplementIDefaultComponentAttributeThrowsException()
		{
			ComponentContainer container = new ComponentContainer();
			container.DefaultComponentAttributeType = typeof(CustomInjectAttribute);
		}

		[Test]
		public void ComponentsCanBeResolvedWhenUsingCustomDefaultComponentAttribute()
		{
			ComponentContainer container = new ComponentContainer();
			container.DefaultComponentAttributeType = typeof(CustomDefaultComponentAttribute);
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Transient);

			IBar bar = container.Get<IBar>();
			Assert.IsInstanceOf<BarWithCustomDefaultComponentAttribte>(bar);
		}
	}
}