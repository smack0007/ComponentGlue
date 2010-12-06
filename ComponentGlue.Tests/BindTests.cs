using System;
using System.Reflection;
using NUnit.Framework;
using ComponentGlue.Framework;
using ComponentGlue.Tests.Classes;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class BindTests
	{
		[Test]
		public void BindTypeAsOncePerRequestInjectsNewInstanceAlways()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().To<Bar1>().AsOncePerRequest();

			Foo foo1 = kernel.Get<Foo>();
			Foo foo2 = kernel.Get<Foo>();

			Assert.AreNotSame(foo1.Bar, foo2.Bar);
		}

		[Test]
		public void BindTypeAsSingletonInjectsSameInstanceAlways()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().To<Bar1>().AsSingleton();

			Foo foo1 = kernel.Get<Foo>();
			Foo foo2 = kernel.Get<Foo>();

			Assert.AreSame(foo1.Bar, foo2.Bar);
		}

		[Test, ExpectedException]
		public void BindTypeWhichDoesNotImplementInterfaceThrowsException()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().To<Foo>();
		}

		[Test]
		public void HasBindingReturnsTrueWhenBindingExists()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IFoo>().To<Foo>();

			Assert.IsTrue(kernel.HasBinding<IFoo>());
		}

		[Test]
		public void HasBindingReturnsFalseWhenBindingDoesNotExist()
		{
			Kernel kernel = new Kernel();

			Assert.IsFalse(kernel.HasBinding<IFoo>());
		}

		[Test]
		public void RebindAddsBindingWhenBindingDoesNotAlreadyExist()
		{
			Kernel kernel = new Kernel();
			kernel.Rebind<IFoo>().To<Foo>();

			Assert.IsTrue(kernel.HasBinding<IFoo>());
		}

		[Test]
		public void RebindDoesNotThrowExceptionWhenBindingAlreadyExists()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().To<Bar1>();
			kernel.Rebind<IBar>().To<Bar2>();
		}

		[Test]
		public void SpecificBindingOverridesDefaultBinding()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().To<Bar1>();
			kernel.For<Foo>().Bind<IBar>().To<Bar2>();

			Foo foo = kernel.Get<Foo>();

			Assert.IsInstanceOf<Bar2>(foo.Bar);
		}

		[Test]
		public void SpecificBindingWithDifferntBindTypeOverridesDefaultBinding()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().To<Bar1>().AsSingleton();
			kernel.For<Foo>().Bind<IBar>().To<Bar2>().AsOncePerRequest();

			Foo foo1 = kernel.Get<Foo>();
			Foo foo2 = kernel.Get<Foo>();

			Assert.AreNotSame(foo1.Bar, foo2.Bar);
		}

		[Test]
		public void BindToConstantDoesNotConstructNewInstance()
		{
			Kernel kernel = new Kernel();

			IBar bar = new Bar1();
			kernel.Bind<IBar>().ToConstant(bar);

			Foo foo = kernel.Get<Foo>();
			Assert.AreSame(bar, foo.Bar);
		}

		[Test, ExpectedException]
		public void BindToConstantWhereComponentNotNullAndIsNotInstanceOfTypeThrowsException()
		{
			Kernel kernel = new Kernel();

			IBar bar = new Bar1();
			IFoo foo = new Foo(bar);

			kernel.Bind<IBar>().ToConstant(foo);
		}

		[Test]
		public void BindToConstantWhereComponentIsNullDoesNotThrowException()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().ToConstant(null);
		}
	}
}