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
			Kernel kernel = new Kernel();
			kernel.AutoBind(Assembly.GetExecutingAssembly());

			Assert.IsTrue(kernel.HasBinding<ISimple>());
			Assert.IsInstanceOf(typeof(ISimple), kernel.Get<ISimple>());
		}

		[Test]
		public void AutoBindBindsCorrectlyForInterfacesWithMultipleImplementors()
		{
			Kernel kernel = new Kernel();
			kernel.AutoBind(Assembly.GetExecutingAssembly());

			Assert.IsTrue(kernel.HasBinding<IBaz>());
			Assert.IsInstanceOf(typeof(Baz1), kernel.Get<IBaz>());
		}

		[Test]
		public void AfterAutoBindFooCanBeResolved()
		{
			Kernel kernel = new Kernel();
			kernel.AutoBind(Assembly.GetExecutingAssembly());

			IFoo foo = kernel.Get<IFoo>();
			Assert.IsInstanceOf(typeof(Foo), foo);
			Assert.IsInstanceOf(typeof(Bar3), ((Foo)foo).Bar);
			Assert.IsInstanceOf(typeof(Baz1), ((Bar3)((Foo)foo).Bar).Baz);
		}
	}
}
