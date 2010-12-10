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
		public void BindTypeAsTransientInjectsNewInstanceAlways()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>().AsTransient();

			Foo foo1 = container.Get<Foo>();
			Foo foo2 = container.Get<Foo>();

			Assert.AreNotSame(foo1.Bar, foo2.Bar);
		}

		[Test]
		public void BindTypeAsSingletonInjectsSameInstanceAlways()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>().AsSingleton();

			Foo foo1 = container.Get<Foo>();
			Foo foo2 = container.Get<Foo>();

			Assert.AreSame(foo1.Bar, foo2.Bar);
		}

		[Test, ExpectedException]
		public void BindTypeWhichDoesNotImplementInterfaceThrowsException()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Foo>();
		}

		[Test]
		public void HasBindingReturnsTrueWhenBindingExists()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IFoo>().To<Foo>();

			Assert.IsTrue(container.HasBinding<IFoo>());
		}

		[Test]
		public void HasBindingReturnsFalseWhenBindingDoesNotExist()
		{
			ComponentContainer container = new ComponentContainer();

			Assert.IsFalse(container.HasBinding<IFoo>());
		}

		[Test]
		public void RebindAddsBindingWhenBindingDoesNotAlreadyExist()
		{
			ComponentContainer container = new ComponentContainer();
			container.Rebind<IFoo>().To<Foo>();

			Assert.IsTrue(container.HasBinding<IFoo>());
		}

		[Test]
		public void RebindDoesNotThrowExceptionWhenBindingAlreadyExists()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>();
			container.Rebind<IBar>().To<Bar2>();
		}

		[Test]
		public void SpecificBindingOverridesDefaultBinding()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>();
			container.For<Foo>().Bind<IBar>().To<Bar2>();

			Foo foo = container.Get<Foo>();

			Assert.IsInstanceOf<Bar2>(foo.Bar);
		}

		[Test]
		public void SpecificBindingWithDifferntBindTypeOverridesDefaultBinding()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>().AsSingleton();
			container.For<Foo>().Bind<IBar>().To<Bar2>().AsTransient();

			Foo foo1 = container.Get<Foo>();
			Foo foo2 = container.Get<Foo>();

			Assert.AreNotSame(foo1.Bar, foo2.Bar);
		}

		[Test, ExpectedException]
		public void AsBindTypeConstantThrowsException()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>().As(ComponentBindType.Constant);
		}

		[Test]
		public void BindToConstantDoesNotConstructNewInstance()
		{
			ComponentContainer container = new ComponentContainer();

			IBar bar = new Bar1();
			container.Bind<IBar>().ToConstant(bar);

			Foo foo = container.Get<Foo>();
			Assert.AreSame(bar, foo.Bar);
		}

		[Test, ExpectedException]
		public void BindToConstantWhereComponentNotNullAndIsNotInstanceOfTypeThrowsException()
		{
			ComponentContainer container = new ComponentContainer();

			IBar bar = new Bar1();
			IFoo foo = new Foo(bar);

			container.Bind<IBar>().ToConstant(foo);
		}

		[Test]
		public void BindToConstantWhereComponentIsNullDoesNotThrowException()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().ToConstant(null);
		}
	}
}