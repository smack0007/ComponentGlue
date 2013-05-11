using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComponentGlue.Tests.Classes;

namespace ComponentGlue.Tests
{
    [TestFixture]
    public class LifecycleManagementTests
    {
        [Test]
        public void Disposing_Of_Container_Calls_Dispose_On_Singleton_Obects()
        {
            ComponentContainer container = new ComponentContainer();
            container.Bind<DisposableObject>().ToSelf().AsSingleton();

            DisposableObject obj = container.Resolve<DisposableObject>();

            container.Dispose();

            Assert.IsTrue(obj.WasDisposed);
        }
    }
}
