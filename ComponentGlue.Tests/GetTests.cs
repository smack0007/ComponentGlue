using System;
using System.Reflection;
using NUnit.Framework;
using ComponentGlue.Framework;
using ComponentGlue.Tests.Classes;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class GetTests
	{
		[Test, ExpectedException]
		public void GetAbstractClassThrowsException()
		{
			ComponentContainer container = new ComponentContainer();
			container.Get<AbstractClass>();
		}
		
		[Test]
		public void GetClassWithUnmarkedDefaultConstructor()
		{
			ComponentContainer container = new ComponentContainer();
			var instance = container.Get<DefaultConstructor>();
			Assert.IsInstanceOf<DefaultConstructor>(instance);
		}

		[Test]
		public void MultipleCallsToGetWithTypeTransientReturnsDifferntInstance()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<DefaultConstructor>().ToSelf().AsTransient();

			var instance1 = container.Get<DefaultConstructor>();
			Assert.IsInstanceOf<DefaultConstructor>(instance1);

			var instance2 = container.Get<DefaultConstructor>();
			Assert.IsInstanceOf<DefaultConstructor>(instance2);

			Assert.AreNotSame(instance1, instance2);
		}

		[Test]
		public void MultipleCallsToGetWithTypeSingletonReturnsSameInstance()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<DefaultConstructor>().ToSelf().AsSingleton();

			var instance1 = container.Get<DefaultConstructor>();
			Assert.IsInstanceOf<DefaultConstructor>(instance1);

			var instance2 = container.Get<DefaultConstructor>();
			Assert.IsInstanceOf<DefaultConstructor>(instance2);

			Assert.AreSame(instance1, instance2);
		}
		
		[Test]
		public void GetInterfaceUsesDefaultBindings()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>();

			IBar bar = container.Get<IBar>();

			Assert.IsInstanceOf(typeof(Bar1), bar);
		}

		[Test, ExpectedException]
		public void GetInterfaceWithNoBindingThrowsException()
		{
			ComponentContainer container = new ComponentContainer();

			IBar bar = container.Get<IBar>();
		}

		[Test, ExpectedException]
		public void GetWithCircularDependencyThrowsException()
		{
			ComponentContainer container = new ComponentContainer();
			container.Get<CircularDependency1>();
		}
	}
}
