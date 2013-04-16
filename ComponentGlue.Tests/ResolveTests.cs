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
	}
}
