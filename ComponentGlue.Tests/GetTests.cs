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
			Kernel kernel = new Kernel();
			kernel.Get<AbstractClass>();
		}
		
		[Test]
		public void GetClassWithUnmarkedDefaultConstructor()
		{
			Kernel kernel = new Kernel();
			var instance = kernel.Get<DefaultConstructor>();
			Assert.IsInstanceOf<DefaultConstructor>(instance);
		}

		[Test]
		public void MultipleCallsToGetWithTypeOncePerRequestReturnsDifferntInstance()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<DefaultConstructor>().ToSelf().AsOncePerRequest();

			var instance1 = kernel.Get<DefaultConstructor>();
			Assert.IsInstanceOf<DefaultConstructor>(instance1);

			var instance2 = kernel.Get<DefaultConstructor>();
			Assert.IsInstanceOf<DefaultConstructor>(instance2);

			Assert.AreNotSame(instance1, instance2);
		}

		[Test]
		public void MultipleCallsToGetWithTypeSingletonReturnsSameInstance()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<DefaultConstructor>().ToSelf().AsSingleton();

			var instance1 = kernel.Get<DefaultConstructor>();
			Assert.IsInstanceOf<DefaultConstructor>(instance1);

			var instance2 = kernel.Get<DefaultConstructor>();
			Assert.IsInstanceOf<DefaultConstructor>(instance2);

			Assert.AreSame(instance1, instance2);
		}
		
		[Test]
		public void GetInterfaceUsesDefaultBindings()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().To<Bar1>();

			IBar bar = kernel.Get<IBar>();

			Assert.IsInstanceOf(typeof(Bar1), bar);
		}

		[Test, ExpectedException]
		public void GetInterfaceWithNoBindingThrowsException()
		{
			Kernel kernel = new Kernel();

			IBar bar = kernel.Get<IBar>();
		}
	}
}
