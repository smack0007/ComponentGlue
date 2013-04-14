using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using ComponentGlue;
using ComponentGlue.Tests.Classes;

namespace ComponentGlue.Tests
{
	[TestFixture, Description("Auto Bind")]
	public class AutoBindTests
	{
		[Test]
		public void AutoBind_Binds_Correctly_For_Interfaces_With_One_Implementor()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Transient);

			Assert.IsTrue(container.HasBinding<ISimple>());
			Assert.IsInstanceOf(typeof(ISimple), container.Get<ISimple>());
		}

		[Test]
		public void AutoBind_Binds_Correctly_For_Interfaces_With_Multiple_Implementors()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Transient);

			Assert.IsTrue(container.HasBinding<IBaz>());
			Assert.IsInstanceOf(typeof(Baz1), container.Get<IBaz>());
		}

		[Test]
		public void After_AutoBind_IFoo_Can_Be_Resolved()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Transient);

			IFoo foo = container.Get<IFoo>();
			Assert.IsInstanceOf(typeof(Foo), foo);
			Assert.IsInstanceOf(typeof(Bar3), ((Foo)foo).Bar);
			Assert.IsInstanceOf(typeof(Baz1), ((Bar3)((Foo)foo).Bar).Baz);
		}

		[Test]
		public void After_AutoBind_With_BindType_Singleton_IFoo_Resolves_As_Singleton()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Singleton);

			IFoo foo1 = container.Get<IFoo>();
			IFoo foo2 = container.Get<IFoo>();

			Assert.AreSame(foo1, foo2);
		}

		[Test]
		public void After_AutoBind_ConcreteAB_Can_Be_Resolved()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Transient);

			ConcreteAB ab = container.Get<ConcreteAB>();
			Assert.NotNull(ab.A);
			Assert.NotNull(ab.B);
		}

		[Test]
		public void After_AutoBind_As_Singleton_ConcreteAB_Is_Resolved_As_Singleton()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Singleton);

			ConcreteAB ab1 = container.Get<ConcreteAB>();
			ConcreteAB ab2 = container.Get<ConcreteAB>();

			Assert.AreSame(ab1, ab2);
		}

		[Test]
		public void AutoBind_With_Where_Func_Filters_Types_Properly()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Singleton, x => x.GetInterfaces().Contains(typeof(IBar)));

			Assert.IsTrue(container.HasBinding<IBar>());
			Assert.IsFalse(container.HasBinding<IFoo>());
		}

		[Test]
		public void Multiple_Calls_To_AutoBind_Does_Not_Throw_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Transient);
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Transient);
		}
	}
}
