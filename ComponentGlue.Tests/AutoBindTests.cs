using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using ComponentGlue;
using ComponentGlue.Tests.Classes;
using ComponentGlue.Tests.Attributes;

namespace ComponentGlue.Tests
{
	[TestFixture]
	public class AutoBindTests
	{
        [Test]
        public void AutoBind_Finds_Interfaces_With_No_Implementors()
        {
            bool check = false;

            ComponentContainer container = new ComponentContainer();
            container.AutoBind(Assembly.GetExecutingAssembly(), new TestAutoBindingStrategy((x, interfaceType, componentTypes) =>
            {
                if (interfaceType == typeof(INotImplemented))
                {
                    check = true;
                }
            }));

            Assert.IsTrue(check);
        }

        [Test]
        public void AutoBind_Finds_All_Implementors()
        {
            ComponentContainer container = new ComponentContainer();
            container.AutoBind(Assembly.GetExecutingAssembly(), new TestAutoBindingStrategy((x, interfaceType, componentTypes) =>
            {
                if (interfaceType == typeof(IFoo))
                {
                    Assert.Greater(componentTypes.Count, 1);
                }
            }));
        }

        [Test, ExpectedException]
        public void Assigning_DefaultComponentAttributeType_To_Null_Throws_Exception()
        {
            AutoBindStrategy strategy = new AutoBindStrategy();
            strategy.DefaultComponentAttributeType = null;
        }

        [Test, ExpectedException]
        public void Assigning_DefaultComponentAttributeType_To_Non_Attribute_Type_Throws_Exception()
        {
            AutoBindStrategy strategy = new AutoBindStrategy();
            strategy.DefaultComponentAttributeType = typeof(ConfigurationTests);
        }

        [Test, ExpectedException]
        public void Assigning_DefaultComponentAttributeType_Where_Type_Does_Not_Implement_IDefaultComponentAttribute_Throws_Exception()
        {
            AutoBindStrategy strategy = new AutoBindStrategy();
            strategy.DefaultComponentAttributeType = typeof(CustomResolveAttribute);
        }

        [Test]
        public void Components_Can_Be_Resolved_When_Using_Custom_DefaultComponentAttribute()
        {
            ComponentContainer container = new ComponentContainer();
            container.AutoBind(Assembly.GetExecutingAssembly(), new AutoBindStrategy()
            {
                DefaultComponentAttributeType = typeof(CustomDefaultComponentAttribute),
            });
            
            IBar bar = container.Resolve<IBar>();
            Assert.IsInstanceOf<BarWithCustomDefaultComponentAttribte>(bar);
        }

        [Test]
        public void AutoBind_Binds_Correctly_For_Interfaces_With_One_Implementor()
        {
            ComponentContainer container = new ComponentContainer();
            container.AutoBind(Assembly.GetExecutingAssembly());

            Assert.IsTrue(container.HasBinding<IHasOneImplementor>());
            Assert.IsInstanceOf(typeof(HasOneImplementor), container.Resolve<IHasOneImplementor>());
        }

        [Test]
        public void AutoBind_Binds_Correctly_For_Interfaces_With_Multiple_Implementors_When_One_Implementor_Is_Marked_As_Default()
        {
            ComponentContainer container = new ComponentContainer();
            container.AutoBind(Assembly.GetExecutingAssembly());

            Assert.IsTrue(container.HasBinding<IBaz>());
            Assert.IsInstanceOf(typeof(Baz1), container.Resolve<IBaz>());
        }

        [Test]
        public void After_AutoBind_With_BindType_Singleton_IHasOneImplementor_Resolves_As_Singleton()
        {
            ComponentContainer container = new ComponentContainer();
            container.AutoBind(Assembly.GetExecutingAssembly(), new AutoBindStrategy()
            {
                BindType = ComponentBindType.Singleton
            });

            var obj1 = container.Resolve<IHasOneImplementor>();
            var obj2 = container.Resolve<IHasOneImplementor>();

            Assert.AreSame(obj1, obj2);
        }

        [Test]
        public void AutoBind_With_InterfaceTypeFilter_Filters_Properly()
        {
            ComponentContainer container = new ComponentContainer();
            container.AutoBind(Assembly.GetExecutingAssembly(), new AutoBindStrategy()
            {
                InterfaceTypeFilter = x => x.Name == "IBar"
            });

            Assert.IsTrue(container.HasBinding<IBar>());
            Assert.IsFalse(container.HasBinding<IFoo>());
        }

        [Test]
        public void AutoBind_With_ComponentTypesFilter_Filters_Properly()
        {
            ComponentContainer container = new ComponentContainer();
            container.AutoBind(Assembly.GetExecutingAssembly(), new AutoBindStrategy()
            {
                ComponentTypesFilter = x => x.Name.EndsWith("2")
            });

            Assert.IsInstanceOf(typeof(Baz2), container.Resolve<IBaz>());
        }

        [Test, ExpectedException(typeof(AutoBindException))]
        public void AutoBind_With_ThrowOnUnresolved_Throws_Exception_When_There_Are_No_Implementors()
        {
            ComponentContainer container = new ComponentContainer();
            container.AutoBind(Assembly.GetExecutingAssembly(), new AutoBindStrategy()
            {
                ThrowOnUnresolved = true,
                InterfaceTypeFilter = x => x.Name == "INotImplemented"
            });
        }

        [Test, ExpectedException(typeof(AutoBindException))]
        public void AutoBind_With_ThrowOnUnresolved_Throws_Exception_When_There_Are_Multiple_Implementors_And_One_Is_Not_Marked_As_Default()
        {
            ComponentContainer container = new ComponentContainer();
            container.AutoBind(Assembly.GetExecutingAssembly(), new AutoBindStrategy()
            {
                ThrowOnUnresolved = true,
                InterfaceTypeFilter = x => x.Name == "IBar",
                ComponentTypesFilter = x => !x.Name.EndsWith("3")
            });
        }

        [Test]
        public void Multiple_Calls_To_AutoBind_Using_Default_AutoBindStrategy_Does_Not_Throw_Exception()
        {
            ComponentContainer container = new ComponentContainer();
            container.AutoBind(Assembly.GetExecutingAssembly());
            container.AutoBind(Assembly.GetExecutingAssembly());
        }
	}
}
