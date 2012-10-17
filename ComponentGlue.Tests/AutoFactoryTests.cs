using System;
using ComponentGlue.Tests.Classes;
using NUnit.Framework;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class AutoFactoryTests
	{
		[Test]
		public void AutoFactoryMethodCallsBackToContainerFactoryMethod()
		{
			bool containerFactoryMethodInvoked = false;

			ComponentContainer container = new ComponentContainer();
			container.Bind<IBaz>().ToFactoryMethod((c) =>
			{
				containerFactoryMethodInvoked = true;
				return new Baz2();
			});

			AutoFactoryClass<IBaz> autoFactoryClass = container.Get<AutoFactoryClass<IBaz>>();
			IBaz baz = autoFactoryClass.InvokeFactory();

			Assert.IsTrue(containerFactoryMethodInvoked);
		}
	}
}
