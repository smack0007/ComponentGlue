using System;
using System.Reflection;
using NUnit.Framework;
using ComponentGlue;
using ComponentGlue.Tests.Classes;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class BindTests
	{
		[Test]
		public void Bind_Type_As_Transient_Injects_New_Instance_Always()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>().AsTransient();

			Foo1 foo1 = container.Resolve<Foo1>();
			Foo1 foo2 = container.Resolve<Foo1>();

			Assert.AreNotSame(foo1.Bar, foo2.Bar);
		}

		[Test]
		public void Bind_Type_As_Singleton_Injects_Same_Instance_Always()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>().AsSingleton();

			Foo1 foo1 = container.Resolve<Foo1>();
			Foo1 foo2 = container.Resolve<Foo1>();

			Assert.AreSame(foo1.Bar, foo2.Bar);
		}

        [Test]
        public void Bind_Type_As_Singleton_For_Different_Components_Injects_Different_Instances()
        {
            ComponentContainer container = new ComponentContainer();
            container.For<Foo1>().Bind<IBar>().To<Bar1>().AsSingleton();
            container.For<Foo2>().Bind<IBar>().To<Bar1>().AsSingleton();

            Foo1 foo1 = container.Resolve<Foo1>();
            Foo2 foo2 = container.Resolve<Foo2>();

            Assert.AreNotSame(foo1.Bar, foo2.Bar);
        }

		[Test, ExpectedException(typeof(BindingSyntaxException))]
		public void Bind_Type_Which_Does_Not_Implement_Interface_Throws_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Foo1>();
		}

		[Test]
		public void HasBinding_Returns_True_When_Binding_Exists()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IFoo>().To<Foo1>();

			Assert.IsTrue(container.HasBinding<IFoo>());
		}

		[Test]
		public void HasBinding_Returns_False_When_Binding_Does_Not_Exist()
		{
			ComponentContainer container = new ComponentContainer();

			Assert.IsFalse(container.HasBinding<IFoo>());
		}

		[Test]
		public void Rebind_Adds_Binding_When_Binding_Does_Not_Already_Exist()
		{
			ComponentContainer container = new ComponentContainer();
			container.Rebind<IFoo>().To<Foo1>();

			Assert.IsTrue(container.HasBinding<IFoo>());
		}

		[Test]
		public void Rebind_Does_Not_Throw_Exception_When_Binding_Already_Exists()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>();
			container.Rebind<IBar>().To<Bar2>();
		}

		[Test]
		public void Specific_Binding_Overrides_Global_Binding()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>();
			container.For<Foo1>().Bind<IBar>().To<Bar2>();

			Foo1 foo = container.Resolve<Foo1>();

			Assert.IsInstanceOf<Bar2>(foo.Bar);
		}

		[Test]
		public void Specific_Binding_With_Differnt_Type_Overrides_Global_Binding()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>().AsSingleton();
			container.For<Foo1>().Bind<IBar>().To<Bar2>().AsTransient();

			Foo1 foo1 = container.Resolve<Foo1>();
			Foo1 foo2 = container.Resolve<Foo1>();

			Assert.AreNotSame(foo1.Bar, foo2.Bar);
		}

		[Test]
		public void Bind_To_Constant_Does_Not_Construct_New_Instance()
		{
			ComponentContainer container = new ComponentContainer();

			IBar bar = new Bar1();
			container.Bind<IBar>().ToConstant(bar);

			Foo1 foo = container.Resolve<Foo1>();
			Assert.AreSame(bar, foo.Bar);
		}

		[Test, ExpectedException(typeof(BindingSyntaxException))]
		public void Bind_ToConstant_Where_Component_Is_Not_Instance_Of_Type_Throws_Exception()
		{
			ComponentContainer container = new ComponentContainer();

			IBar bar = new Bar1();
			IFoo foo = new Foo1(bar);

			container.Bind<IBar>().ToConstant(foo);
		}

		[Test]
		public void Bind_ToConstant_Where_Component_Is_Null_Does_Not_Throw_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().ToConstant(null);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void Bind_ToFactoryMethod_Where_Method_Is_Null_Throws_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().ToFactoryMethod<Bar1>(null);
		}

		[Test, ExpectedException(typeof(BindingSyntaxException))]
		public void Bind_ToFactoryMethod_Where_Return_Value_Is_Not_Assignable_To_Interface_Throws_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().ToFactoryMethod((x) => new Foo1(new Bar1()));
		}
                
		[Test]
		public void Bind_ToFactoryMethod_Calls_FactoryMethod_When_Resolving()
		{
			bool factoryMethodCalled = false;

			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().ToFactoryMethod((x) =>
			{
				factoryMethodCalled = true;
				return new Bar1();
			});

			IBar bar = container.Resolve<IBar>();
			Assert.IsTrue(factoryMethodCalled);
		}

        class TestStrategy<T> : IComponentBindingStrategy, IDisposable
        {
            Func<T> factory;
            public bool ResolveWasCalled = false;
            public bool DisposeWasCalled = false;

            public TestStrategy(Func<T> factory)
            {
                this.factory = factory;
            }

            public object Resolve(ComponentContainer container)
            {
                ResolveWasCalled = true;
                return this.factory();
            }

            public void Dispose()
            {
                DisposeWasCalled = true;
            }
        }

        [Test]
        public void Bind_ToStrategy_Calls_Strategy()
        {
            var strategy = new TestStrategy<Bar1>(() => new Bar1());

            ComponentContainer container = new ComponentContainer();
            container.Bind<IBar>().ToStrategy(strategy);

            var bar = container.Resolve<IBar>();

            Assert.IsTrue(strategy.ResolveWasCalled);
        }

        [Test, ExpectedException(typeof(ComponentResolutionException))]
        public void Bind_ToStrategy_Where_Strategy_Returns_Wrong_Type_Throws_Exception()
        {
            var strategy = new TestStrategy<Simple>(() => new Simple());

            ComponentContainer container = new ComponentContainer();
            container.Bind<IBar>().ToStrategy(strategy);

            var bar = container.Resolve<IBar>();
        }

        [Test, ExpectedException(typeof(ComponentResolutionException))]
        public void Bind_ToStrategy_Where_Strategy_Returns_Null_Throws_Exception()
        {
            var strategy = new TestStrategy<Bar1>(() => null);

            ComponentContainer container = new ComponentContainer();
            container.Bind<IBar>().ToStrategy(strategy);

            var bar = container.Resolve<IBar>();
        }

        [Test]
        public void Bind_ToStrategy_Strategy_Is_Disposed_With_Container()
        {
            var strategy = new TestStrategy<Bar1>(() => new Bar1());

            ComponentContainer container = new ComponentContainer();
            container.Bind<IBar>().ToStrategy(strategy);

            container.Dispose();

            Assert.IsTrue(strategy.DisposeWasCalled);
        }
                
        [Test]
        public void WithConstructorParameter_Is_Passed_To_Constructor()
        {
            ComponentContainer container = new ComponentContainer();
            
            container.Bind<Has1Param>()
                .ToSelf()
                .AsTransient()
                .WithConstructorParameter("name", "Steve");

            var obj = container.Resolve<Has1Param>();
            Assert.AreEqual("Steve", obj.Name);
        }

        [Test]
        public void Multiple_WithConstructorParameter_Are_Passed_To_Constructor()
        {
            ComponentContainer container = new ComponentContainer();
            
            container.Bind<Has2Params>().ToSelf()
                .AsTransient()
                .WithConstructorParameter("name", "Steve")
                .WithConstructorParameter("age", 12);

            var obj = container.Resolve<Has2Params>();
            Assert.AreEqual("Steve", obj.Name);
            Assert.AreEqual(12, obj.Age);
        }

        [Test]
        public void WithConstructorParameter_Overrides_Global_Bindings()
        {
            ComponentContainer container = new ComponentContainer();

            Bar1 bar = new Bar1();

            container.Bind<IFoo>().To<Foo1>()
                .AsTransient()
                .WithConstructorParameter("bar", bar);

            container.Bind<IBar>().To<Bar2>();

            var obj = container.Resolve<IFoo>();
            Assert.AreSame(bar, ((Foo1)obj).Bar);
        }

        [Test]
        public void WithConstructorParameter_Overrides_Specific_Bindings()
        {
            ComponentContainer container = new ComponentContainer();

            Bar1 bar = new Bar1();

            container.Bind<IFoo>().To<Foo1>()
                .AsTransient()
                .WithConstructorParameter("bar", bar);

            container.For<IFoo>().Bind<IBar>().To<Bar2>();

            var obj = container.Resolve<IFoo>();
            Assert.AreSame(bar, ((Foo1)obj).Bar);
        }

        [Test]
        public void WithPropertyValue_Is_Set_After_Resolution()
        {
            ComponentContainer container = new ComponentContainer();

            container.Bind<Has1Property>().To<Has1Property>()
                .AsTransient()
                .WithPropertyValue("ID", "abc");

            var obj = container.Resolve<Has1Property>();
            Assert.AreSame("abc", obj.ID);
        }

        [Test]
        public void Multiple_WithPropertyValue_Are_Set_After_Resolution()
        {
            ComponentContainer container = new ComponentContainer();

            container.Bind<Has2Properties>().To<Has2Properties>()
                .AsTransient()
                .WithPropertyValue("ID", "abc")
                .WithPropertyValue("Name", "def");

            var obj = container.Resolve<Has2Properties>();
            Assert.AreSame("abc", obj.ID);
            Assert.AreSame("def", obj.Name);
        }

        [Test, ExpectedException(typeof(ComponentResolutionException))]
        public void WithPropertyValue_That_Does_Not_Exist_Throws_Exception_When_Resolving()
        {
            ComponentContainer container = new ComponentContainer();

            container.Bind<Has1Property>().To<Has1Property>()
                .AsTransient()
                .WithPropertyValue("Foo", "abc");

            var obj = container.Resolve<Has1Property>();
        }

        [Test, ExpectedException(typeof(ComponentResolutionException))]
        public void WithPropertyValue_That_Has_No_Setter_Throws_Exception_When_Resolving()
        {
            ComponentContainer container = new ComponentContainer();

            container.Bind<Has1PropertyWithNoSetter>().To<Has1PropertyWithNoSetter>()
                .AsTransient()
                .WithPropertyValue("ID", "abc");

            var obj = container.Resolve<Has1PropertyWithNoSetter>();
        }
	}
}