using System;
using ComponentGlue.Tests.Classes;
using NUnit.Framework;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class AutoFactoryTests
	{
		[Test, ExpectedException(typeof(ComponentResolutionException))]
		public void AutoFactory_Not_Constructed_For_Func_With_Params()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBaz>().To<Baz2>();
			container.Get<FuncClassWithOneParam<string, IBaz>>();
		}

		[Test]
		public void AutoFactory_Calls_Back_To_Container_Factory_Method()
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
		public void AutoFactory_Does_Not_Override_Bindings()
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
