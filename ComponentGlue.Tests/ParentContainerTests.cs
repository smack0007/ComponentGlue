using System;
using System.Reflection;
using NUnit.Framework;
using ComponentGlue;
using ComponentGlue.Tests.Classes;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class ParentContainerTests
	{
		[Test]
		public void ChildContainerProxiesToParentContainerWhenChildContainerHasNoBinding()
		{
			ComponentContainer parent = new ComponentContainer();
			parent.Bind<IBaz>().To<Baz1>().AsSingleton();

			ComponentContainer child = new ComponentContainer(parent);
			child.Bind<IBar>().To<Bar3>().AsSingleton();

			Foo foo = child.Get<Foo>();

			Assert.IsInstanceOf(typeof(Foo), foo);
			Assert.IsInstanceOf(typeof(Bar3), foo.Bar);
			Assert.IsInstanceOf(typeof(Baz1), ((Bar3)foo.Bar).Baz);
		}

		[Test]
		public void ChildContainerDoesNotProxyToParentContainerWhenChildContainerHasBinding()
		{
			ComponentContainer parent = new ComponentContainer();
			parent.Bind<IBaz>().To<Baz1>().AsSingleton();

			ComponentContainer child = new ComponentContainer(parent);
			child.Bind<IBar>().To<Bar3>().AsSingleton();
			child.Bind<IBaz>().To<Baz2>().AsSingleton();

			Foo foo = child.Get<Foo>();

			Assert.IsInstanceOf(typeof(Foo), foo);
			Assert.IsInstanceOf(typeof(Bar3), foo.Bar);
			Assert.IsInstanceOf(typeof(Baz2), ((Bar3)foo.Bar).Baz);
		}
	}
}
