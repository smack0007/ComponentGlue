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
		public void Child_Container_Proxies_To_Parent_Container_When_Child_Container_Has_No_Binding()
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
		public void Child_Container_Does_Not_Proxy_To_Parent_Container_When_Child_Container_Has_Binding()
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
