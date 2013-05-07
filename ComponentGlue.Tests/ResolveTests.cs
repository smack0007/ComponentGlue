using System;
using System.Reflection;
using NUnit.Framework;
using ComponentGlue;
using ComponentGlue.Tests.Classes;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class ResolveTests
	{
		[Test, ExpectedException]
		public void Resolve_Abstract_Class_Throws_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			container.Resolve<AbstractClass>();
		}
		
		[Test]
        public void Resolve_Class_With_Unmarked_Default_Constructor()
		{
			ComponentContainer container = new ComponentContainer();
			var instance = container.Resolve<DefaultConstructor>();
			Assert.IsInstanceOf<DefaultConstructor>(instance);
		}
        
		[Test]
        public void Resolve_Interface_Uses_Default_Bindings()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>();

			IBar bar = container.Resolve<IBar>();

			Assert.IsInstanceOf(typeof(Bar1), bar);
		}

		[Test, ExpectedException]
        public void Resolve_Interface_With_No_Binding_Throws_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			IBar bar = container.Resolve<IBar>();
		}

		[Test, ExpectedException]
        public void Resolve_With_Circular_Dependency_Throws_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			container.Resolve<CircularDependency1>();
		}

        [Test]
        public void After_Call_To_Resolve_Where_New_Instance_Is_Constructed_Properties_Are_Injected()
        {
            ComponentContainer container = new ComponentContainer();
            container.Bind<IFoo>().To<Foo1>();
            container.Bind<IBar>().To<Bar1>();
            container.For<PropertyInject>().Bind<IBar>().To<Bar2>();

            var instance = container.Resolve<PropertyInject>();

            Assert.NotNull(instance.Foo);
            Assert.IsInstanceOf<Foo1>(instance.Foo);

            Assert.NotNull(instance.Bar);
            Assert.IsInstanceOf<Bar2>(instance.Bar);
        }

        [Test]
        public void Circular_Dependency_Can_Be_Resolved_Using_Property_Injection_And_Singletons()
        {
            ComponentContainer container = new ComponentContainer();
            container.Bind<CircularDependencyProperty1>().ToSelf().AsSingleton();
            container.Bind<CircularDependencyProperty2>().ToSelf().AsSingleton();

            var dependency1 = container.Resolve<CircularDependencyProperty1>();
            var dependency2 = container.Resolve<CircularDependencyProperty2>();

            Assert.NotNull(dependency1.Dependency);
            Assert.IsInstanceOf<CircularDependencyProperty2>(dependency1.Dependency);

            Assert.NotNull(dependency2.Dependency);
            Assert.IsInstanceOf<CircularDependencyProperty1>(dependency2.Dependency);
        }
	}
}
