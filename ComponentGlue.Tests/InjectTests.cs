using System;
using System.Reflection;
using NUnit.Framework;
using ComponentGlue;
using ComponentGlue.Tests.Classes;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class InjectTests
	{
		[Test]
		public void Inject_With_Default_Bindings()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IFoo>().To<Foo>();
			container.Bind<IBar>().To<Bar2>();

			var instance = new PropertyInject();
			container.Inject(instance);

			Assert.NotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.NotNull(instance.Bar);
			Assert.IsInstanceOf<Bar2>(instance.Bar);
		}

		[Test]
		public void Inject_With_Specific_Bindings()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>();
			container.For<PropertyInject>().Bind<IFoo>().To<Foo>();
			container.For<PropertyInject>().Bind<IBar>().To<Bar2>();

			var instance = new PropertyInject();
			container.Inject(instance);

			Assert.NotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.NotNull(instance.Bar);
			Assert.IsInstanceOf<Bar2>(instance.Bar);
		}

		[Test]
		public void Inject_With_Specific_Bindings_Override_Default_Bindings()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IFoo>().To<Foo>();
			container.Bind<IBar>().To<Bar1>();
			container.For<PropertyInject>().Bind<IBar>().To<Bar2>();

			var instance = new PropertyInject();
			container.Inject(instance);

			Assert.NotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

			Assert.NotNull(instance.Bar);
			Assert.IsInstanceOf<Bar2>(instance.Bar);
		}

		[Test]
		public void After_Call_To_Get_Where_New_Instance_Is_Constructed_Properties_Are_Injected()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IFoo>().To<Foo>();
			container.Bind<IBar>().To<Bar1>();
			container.For<PropertyInject>().Bind<IBar>().To<Bar2>();

			var instance = container.Resolve<PropertyInject>();

			Assert.NotNull(instance.Foo);
			Assert.IsInstanceOf<Foo>(instance.Foo);

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