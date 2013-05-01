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
    }
}
