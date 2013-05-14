Component Glue
==============

Component Glue is a dependency injection / IoC container. Component Glue has a fluent syntax and aims to be simple to use.

Key Features
------------

* **Fluent Syntax** - Use a fluent syntax to glue together your components.
* **Auto Binding** - The cotainer can look through an assembly an automatically wire up interfaces which only have one implementing component.
* **Auto Factories** - Just request a Func<T> in a component and the container will inject a factory that will resolve the binding.
* **Multi Binding** - Components can receive arrays of dependencies using ToMultiple.


Getting Started
---------------

_The following code snippets are taken from the provided demo program._

The first thing you'll need to do is instaniate an instance of the container and set your first binding.

	ComponentContainer container = new ComponentContainer();
	container.Bind<IBar>().To<Bar>().AsTransient();
	
This code snippet above binds the component type IBar to the concrete type Bar as transient. Transient means there
will be a new instance of Bar constructed every time a component requires an instance of IBar. This is binding is also
global as we did not specify any component to which the binding is specific. This can be done with the For syntax.

	container.For<Foo>.Bind<IBar>().To<OtherBar>().AsSingleton();
	
This code snippet says for the component Foo, the interface IBar should be bound to the component OtherBar. It also
specifies that OtherBar should be used as a singleton. This means that when multiple instances of Bar are constructed,
they will all have the same instance of OtherBar. Let's get an instance of Foo.

	Foo foo = container.Resolve<Foo>();
	
Resolve constructs an instance of Foo and resolves the IBar constructor parameter. Even though we used Foo in a For clause,
we have not created a binding for Foo in any way. The container will therefore always construct a new instance of Foo.

Bindings are not just for interfaces though. Bindings can also be made on class types as well. We can make a binding for
the type Foo and bind it to itself.

	container.Bind<Foo>().ToSelf().AsSingleton();

The component type Foo is now bound to the concrete type Foo as a singleton. Resolving Foo multiple times will now return
the same instance.

Syntax
------
	[For]
		Bind | Rebind
			To | ToSelf
				As | AsTransient | AsSingleton
					WithConstructorParameter
			ToConstant
			ToFactoryMethod
			ToMultiple
				Add
					As | AsTransient | AsSingleton
						WithConstructorParameter | WithPropertyValue
						
