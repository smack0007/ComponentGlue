using System;
using ComponentGlue.Tests.Classes;
using NUnit.Framework;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class AutoFactoryTests
	{
		[Test, ExpectedException(typeof(ComponentResolutionException))]
		public void AutoFactoryNotCunstructedForFuncWithParams()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBaz>().To<Baz2>();
			container.Get<FuncClassWithOneParam<string, IBaz>>();
		}

		[Test]
		public void AutoFactoryCallsBackToContainerFactoryMethod()
		{
			bool containerFactoryMethodInvoked = false;

			ComponentContainer container = new ComponentContainer();
			container.Bind<IBaz>().ToFactoryMethod((c) =>
			{
				containerFactoryMethodInvoked = true;
				return new Baz2();
			});

			FuncClass<IBaz> funcClass = container.Get<FuncClass<IBaz>>();
			IBaz baz = funcClass.InvokeFunc();

			Assert.IsTrue(containerFactoryMethodInvoked);
		}

		[Test]
		public void AutoFactoryDoesNotOverrideBindings()
		{
			bool funcInvoked = false;

			ComponentContainer container = new ComponentContainer();
			container.Bind<Func<IBaz>>().ToConstant(new Func<IBaz>(() =>
			{
				funcInvoked = true;
				return new Baz2();
			}));

			FuncClass<IBaz> funcClass = container.Get<FuncClass<IBaz>>();
			IBaz baz = funcClass.InvokeFunc();

			Assert.IsTrue(funcInvoked);
		}
	}
}
