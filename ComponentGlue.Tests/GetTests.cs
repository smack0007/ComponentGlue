using System;
using System.Reflection;
using NUnit.Framework;
using ComponentGlue;
using ComponentGlue.Tests.Classes;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class GetTests
	{
		[Test, ExpectedException]
		public void Get_Abstract_Class_Throws_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			container.Get<AbstractClass>();
		}
		
		[Test]
		public void Get_Class_With_Unmarked_Default_Constructor()
		{
			ComponentContainer container = new ComponentContainer();
			var instance = container.Get<DefaultConstructor>();
			Assert.IsInstanceOf<DefaultConstructor>(instance);
		}
        
		[Test]
		public void Get_Interface_Uses_Default_Bindings()
		{
			ComponentContainer container = new ComponentContainer();
			container.Bind<IBar>().To<Bar1>();

			IBar bar = container.Get<IBar>();

			Assert.IsInstanceOf(typeof(Bar1), bar);
		}

		[Test, ExpectedException]
		public void Get_Interface_With_No_Binding_Throws_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			IBar bar = container.Get<IBar>();
		}

		[Test, ExpectedException]
		public void Get_With_Circular_Dependency_Throws_Exception()
		{
			ComponentContainer container = new ComponentContainer();
			container.Get<CircularDependency1>();
		}
	}
}
