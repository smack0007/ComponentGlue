using System;
using System.Reflection;
using NUnit.Framework;
using ComponentGlue.Framework;
using ComponentGlue.Tests.Classes;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class KernelTests
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

		[Test]
		public void BindTypeAsNewInjectsNewInstanceAlways()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().To<Bar1>().AsNew();

			Foo foo1 = kernel.Construct<Foo>();
			Foo foo2 = kernel.Construct<Foo>();

			Assert.AreNotSame(foo1.Bar, foo2.Bar);
		}

		[Test]
		public void BindTypeAsSharedInjectsSameInstanceAlways()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().To<Bar1>().AsShared();

			Foo foo1 = kernel.Construct<Foo>();
			Foo foo2 = kernel.Construct<Foo>();

			Assert.AreSame(foo1.Bar, foo2.Bar);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void BindTypeWhichDoesNotImplementInterfaceThrowsException()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().To<Foo>();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ConstructAbstractClassThrowsException()
		{
			Kernel kernel = new Kernel();
			kernel.Construct<AbstractClass>();
		}

		[Test]
		public void KernelIsBoundToIKernalByDefault()
		{
			Kernel kernel = new Kernel();
			var instance = kernel.Construct<NeedsKernel>();
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
		public void ConstructClassWithUnmarkedDefaultConstructor()
		{
			Kernel kernel = new Kernel();
			var instance = kernel.Construct<DefaultConstructor>();
			Assert.IsInstanceOf<DefaultConstructor>(instance);
		}

		[Test]
		public void MultipleCallsToGetWithSameTypeReturnsSameInstance()
		{
			Kernel kernel = new Kernel();
			
			var instance1 = kernel.Get<DefaultConstructor>();
			Assert.IsInstanceOf<DefaultConstructor>(instance1);

			var instance2 = kernel.Get<DefaultConstructor>();
			Assert.IsInstanceOf<DefaultConstructor>(instance2);

			Assert.AreSame(instance1, instance2);
		}


		[Test]
		public void SpecificBindingOverridesDefaultBinding()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().To<Bar1>();
			kernel.For<Foo>().Bind<IBar>().To<Bar2>();

			Foo foo = kernel.Construct<Foo>();

			Assert.IsInstanceOf<Bar2>(foo.Bar);
		}

		[Test]
		public void SpecificBindingWithDifferntBindTypeOverridesDefaultBinding()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().To<Bar1>().AsShared();
			kernel.For<Foo>().Bind<IBar>().To<Bar2>().AsNew();

			Foo foo1 = kernel.Construct<Foo>();
			Foo foo2 = kernel.Construct<Foo>();

			Assert.AreNotSame(foo1.Bar, foo2.Bar);
		}

		[Test]
		public void InjectPropertiesWithDefaultBindings()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IFoo>().To<Foo>();
			kernel.Bind<IBar>().To<Bar2>();

			var instance = new PropertyInject();
			kernel.Inject(instance);

			Assert.NotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.NotNull(instance.Bar);
			Assert.IsInstanceOf<Bar2>(instance.Bar);
		}

		[Test]
		public void InjectPropertiesWithSpecificBindings()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().To<Bar1>();
			kernel.For<PropertyInject>().Bind<IFoo>().To<Foo>();
			kernel.For<PropertyInject>().Bind<IBar>().To<Bar2>();

			var instance = new PropertyInject();
			kernel.Inject(instance);

			Assert.NotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.NotNull(instance.Bar);
			Assert.IsInstanceOf<Bar2>(instance.Bar);
		}

		[Test]
		public void InjectPropertiesSpecificBindingsOverrideDefaultBindings()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IFoo>().To<Foo>();
			kernel.Bind<IBar>().To<Bar1>();
			kernel.For<PropertyInject>().Bind<IBar>().To<Bar2>();

			var instance = new PropertyInject();
			kernel.Inject(instance);

			Assert.NotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.NotNull(instance.Bar);
			Assert.IsInstanceOf<Bar2>(instance.Bar);
		}

		[Test]
		public void BindToConstantDoesNotConstructNewInstance()
		{
			Kernel kernel = new Kernel();

			IBar bar = new Bar1();
			kernel.Bind<IBar>().ToConstant(bar);

			Foo foo = kernel.Construct<Foo>();
			Assert.AreSame(bar, foo.Bar);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
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

		[Test]
		public void BindToMethodCallsFactoryMethod()
		{
			bool check = false;

			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().ToMethod((c, i) => { check = true; return new Bar1(); });

			kernel.Construct<Foo>();

			Assert.IsTrue(check);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void BindToMethodThrowsExceptionWhenFactoryMethodDoesNotReturnCorrectInstance()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().ToMethod((c, i) => { return new Foo(new Bar1()); });

			kernel.Construct<Foo>();
		}

		[Test]
		public void FactoryMethodCalledForEveryNewInstance()
		{
			int factoryCount = 0;
			Func<Type, Type, object> factory = (c, i) => { factoryCount++; return new Bar1(); };

			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().ToMethod(factory);

			kernel.Construct<Foo>();
			kernel.Construct<Foo>();

			Assert.AreEqual(2, factoryCount);
		}

		[Test]
		public void FactoryMethodReceivesCorrectTypeInformation()
		{
			Type constructedType = null, interfaceType = null;
			Func<Type, Type, object> factory = (c, i) => { constructedType = c; interfaceType = i; return new Bar1(); };

			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().ToMethod(factory);
			kernel.Construct<Foo>();
			
			Assert.AreEqual(typeof(Foo), constructedType);
			Assert.AreEqual(typeof(IBar), interfaceType);
		}

		[Test]
		public void ChildKernelProxiesToParentKernelWhenChildKernelHasNoBinding()
		{
			Kernel parent = new Kernel();
			parent.Bind<IBaz>().To<Baz1>().AsShared();

			Kernel child = new Kernel(parent);
			child.Bind<IBar>().To<Bar3>().AsShared();

			Foo foo = child.Get<Foo>();

			Assert.IsInstanceOf(typeof(Foo), foo);
			Assert.IsInstanceOf(typeof(Bar3), foo.Bar);
			Assert.IsInstanceOf(typeof(Baz1), ((Bar3)foo.Bar).Baz);
		}

		[Test]
		public void ChildKernelDoesNotProxyToParentKernelWhenChildKernelHasBinding()
		{
			Kernel parent = new Kernel();
			parent.Bind<IBaz>().To<Baz1>().AsShared();

			Kernel child = new Kernel(parent);
			child.Bind<IBar>().To<Bar3>().AsShared();
			child.Bind<IBaz>().To<Baz2>().AsShared();

			Foo foo = child.Get<Foo>();

			Assert.IsInstanceOf(typeof(Foo), foo);
			Assert.IsInstanceOf(typeof(Bar3), foo.Bar);
			Assert.IsInstanceOf(typeof(Baz2), ((Bar3)foo.Bar).Baz);
		}

		[Test]
		public void ConstructClassByInterfaceUsesDefaultBindings()
		{
			Kernel kernel = new Kernel();
			kernel.Bind<IBar>().To<Bar1>();

			IBar bar = kernel.Get<IBar>();

			Assert.IsInstanceOf(typeof(Bar1), bar);
		}
	}
}
