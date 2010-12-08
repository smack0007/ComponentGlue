using System;
using System.Reflection;
using NUnit.Framework;
using ComponentGlue.Framework;
using ComponentGlue.Tests.Classes;

namespace ComponentGlue.Tests
{
	[TestFixture, Description("Auto Bind")]
	public class AutoBindTests
	{
		[Test]
		public void AutoBindBindsCorrectlyForInterfacesWithOneImplementor()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly());

			Assert.IsTrue(container.HasBinding<ISimple>());
			Assert.IsInstanceOf(typeof(ISimple), container.Get<ISimple>());
		}

		[Test]
		public void AutoBindBindsCorrectlyForInterfacesWithMultipleImplementors()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly());

			Assert.IsTrue(container.HasBinding<IBaz>());
			Assert.IsInstanceOf(typeof(Baz1), container.Get<IBaz>());
		}

		[Test]
		public void AfterAutoBindFooCanBeResolved()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly());

			IFoo foo = container.Get<IFoo>();
			Assert.IsInstanceOf(typeof(Foo), foo);
			Assert.IsInstanceOf(typeof(Bar3), ((Foo)foo).Bar);
			Assert.IsInstanceOf(typeof(Baz1), ((Bar3)((Foo)foo).Bar).Baz);
		}

		[Test]
		public void AfterAutoBindConcreteABCanBeResolved()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly());

			ConcreteAB ab = container.Get<ConcreteAB>();
			Assert.NotNull(ab.A);
			Assert.NotNull(ab.B);
		}
	}
}
