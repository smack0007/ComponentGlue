using ComponentGlue.Tests.Classes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Tests
{
    [TestFixture]
    public class MultiBindTests
    {
        [Test]
        public void Bind_To_Constant_Array_Of_Interface_Type()
        {
            IStubObject[] stubs = new IStubObject[]
            {
                new StubObject() { ID = "1" },
                new StubObject() { ID = "2" },
            };

            ComponentContainer container = new ComponentContainer();
            container.Bind<IStubObject[]>().ToConstant(stubs);

            var obj = container.Resolve<NeedsArrayOfStubObjects>();
            Assert.AreEqual(stubs.Length, obj.Stubs.Length);
            
            for (int i = 0; i < stubs.Length; i++)
                Assert.AreSame(stubs[i], obj.Stubs[i]);
        }

        [Test]
        public void Bind_To_Constant_Array_Of_Concrete_Type()
        {
            StubObject[] stubs = new StubObject[]
            {
                new StubObject() { ID = "1" },
                new StubObject() { ID = "2" },
            };

            ComponentContainer container = new ComponentContainer();
            container.Bind<IStubObject[]>().ToConstant(stubs);

            var obj = container.Resolve<NeedsArrayOfStubObjects>();
            Assert.AreEqual(stubs.Length, obj.Stubs.Length);

            for (int i = 0; i < stubs.Length; i++)
                Assert.AreSame(stubs[i], obj.Stubs[i]);
        }

        [Test, ExpectedException]
        public void Bind_ToMultiple_With_Non_Collection_Type_Throws_Exception()
        {
            ComponentContainer container = new ComponentContainer();
            container.Bind<IStubObject>().ToMultiple();
        }

        [Test]
        public void Bind_ToMultiple_With_Array()
        {
            ComponentContainer container = new ComponentContainer();
            container.Bind<IStubObject[]>().ToMultiple()
                .Add<StubObjectFoo>().WithConstructorParameter("id", "Foo")
                .Add<StubObjectBar>().WithConstructorParameter("id", "Bar");

            var obj = container.Resolve<NeedsArrayOfStubObjects>();
            Assert.AreEqual(2, obj.Stubs.Length);

            Assert.IsInstanceOf<StubObjectFoo>(obj.Stubs[0]);
            Assert.IsInstanceOf<StubObjectBar>(obj.Stubs[1]);
        }

        [Test]
        public void Transient_And_Singleton_Are_Not_Ignored_By_ToMultiple()
        {
            ComponentContainer container = new ComponentContainer();
            container.Bind<IStubObject[]>().ToMultiple()
                .Add<StubObjectFoo>().AsSingleton().WithConstructorParameter("id", "Foo")
                .Add<StubObjectBar>().AsTransient().WithConstructorParameter("id", "Bar");

            var obj1 = container.Resolve<NeedsArrayOfStubObjects>();
            Assert.AreEqual(2, obj1.Stubs.Length);

            Assert.IsInstanceOf<StubObjectFoo>(obj1.Stubs[0]);
            Assert.IsInstanceOf<StubObjectBar>(obj1.Stubs[1]);

            var obj2 = container.Resolve<NeedsArrayOfStubObjects>();
            Assert.AreEqual(2, obj2.Stubs.Length);

            Assert.IsInstanceOf<StubObjectFoo>(obj2.Stubs[0]);
            Assert.IsInstanceOf<StubObjectBar>(obj2.Stubs[1]);

            Assert.AreSame(obj1.Stubs[0], obj2.Stubs[0]);
            Assert.AreNotSame(obj1.Stubs[1], obj2.Stubs[1]);
        }
    }
}
