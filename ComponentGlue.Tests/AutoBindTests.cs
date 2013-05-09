using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using ComponentGlue;
using ComponentGlue.Tests.Classes;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class AutoBindTests
	{
		[Test]
		public void AutoBind_Binds_Correctly_For_Interfaces_With_One_Implementor()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Transient);

			Assert.IsTrue(container.HasBinding<ISimple>());
			Assert.IsInstanceOf(typeof(ISimple), container.Resolve<ISimple>());
		}

		[Test]
		public void AutoBind_Binds_Correctly_For_Interfaces_With_Multiple_Implementors()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Transient);

			Assert.IsTrue(container.HasBinding<IBaz>());
			Assert.IsInstanceOf(typeof(Baz1), container.Resolve<IBaz>());
		}

		[Test]
		public void After_AutoBind_IOnlyExistsOnce_Can_Be_Resolved()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Transient);

            IOnlyExistsOnce obj = container.Resolve<IOnlyExistsOnce>();
			Assert.IsInstanceOf(typeof(OnlyExistsOnce), obj);
		}

		[Test]
        public void After_AutoBind_With_BindType_Singleton_IOnlyExistsOnce_Resolves_As_Singleton()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Singleton);

            IOnlyExistsOnce obj1 = container.Resolve<IOnlyExistsOnce>();
            IOnlyExistsOnce obj2 = container.Resolve<IOnlyExistsOnce>();

			Assert.AreSame(obj1, obj2);
		}

		[Test]
		public void After_AutoBind_ConcreteAB_Can_Be_Resolved()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Transient);

			ConcreteAB ab = container.Resolve<ConcreteAB>();
			Assert.NotNull(ab.A);
			Assert.NotNull(ab.B);
		}

		[Test]
		public void After_AutoBind_As_Singleton_ConcreteAB_Is_Resolved_As_Singleton()
		{
			ComponentContainer container = new ComponentContainer();
			container.AutoBind(Assembly.GetExecutingAssembly(), ComponentBindType.Singleton);

			ConcreteAB ab1 = container.Resolve<ConcreteAB>();
			ConcreteAB ab2 = container.Resolve<ConcreteAB>();

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
