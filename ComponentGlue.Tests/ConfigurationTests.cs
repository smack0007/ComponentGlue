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
			container.InjectAttributeType = null;
		}

		[Test, ExpectedException]
		public void Assigning_InjectAttributeType_To_Non_Attribute_Type_Throws_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			container.InjectAttributeType = typeof(ConfigurationTests);
		}

		[Test]
		public void Properties_Can_Be_Injected_When_Using_Custom_InjectAttribute()
		{
			ComponentContainer container = new ComponentContainer();
			container.InjectAttributeType = typeof(CustomInjectAttribute);
			container.Bind<IFoo>().To<Foo>();
			container.Bind<IBar>().To<Bar1>();

			CustomAttributePropertyInject instance = container.Resolve<CustomAttributePropertyInject>();

			Assert.IsNotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.IsNotNull(instance.Bar);
			Assert.IsInstanceOf<Bar1>(instance.Bar);
		}

		[Test, ExpectedException]
		public void Assigning_DefaultComponentAttributeType_To_Null_Throws_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			container.DefaultComponentAttributeType = null;
		}

		[Test, ExpectedException]
		public void Assigning_DefaultComponentAttributeType_To_Non_Attribute_Type_Throws_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			container.DefaultComponentAttributeType = typeof(ConfigurationTests);
		}

		[Test, ExpectedException]
		public void Assigning_DefaultComponentAttributeType_Where_Type_Does_Not_Implement_IDefaultComponentAttribute_Throws_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			container.DefaultComponentAttributeType = typeof(CustomInjectAttribute);
		}

		[Test]
		public void Components_Can_Be_Resolved_When_Using_Custom_DefaultComponentAttribute()
		{
			ComponentContainer container = new ComponentContainer();
			container.DefaultComponentAttributeType = typeof(CustomDefaultComponentAttribute);
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Transient);

			IBar bar = container.Resolve<IBar>();
			Assert.IsInstanceOf<BarWithCustomDefaultComponentAttribte>(bar);
		}
	}
}