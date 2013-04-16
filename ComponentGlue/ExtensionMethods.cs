using System;
using ComponentGlue.BindingSyntax;

namespace ComponentGlue
{
	public static class ExtensionMethods
	{
		/// <summary>
		/// Gets an instance of type T.
		/// </summary>
		/// <typeparam name="TComponentType"></typeparam>
		/// <returns></returns>
		public static TComponentType Resolve<TComponentType>(this IComponentContainer container)
		{
			return (TComponentType)container.Resolve(typeof(TComponentType));
		}

		/// <summary>
		/// Begins a specific binding.
		/// </summary>
		/// <param name="constructedType"></param>
		/// <returns></returns>
		public static IBindingSyntaxBind For<TConstructedType>(this IBindingSyntaxFor container)
		{
			return container.For(typeof(TConstructedType));
		}

		/// <summary>
		/// Sets the component type.
		/// </summary>
		/// <typeparam name="TConcreteType"></typeparam>
		/// <returns></returns>
		public static IBindingSyntaxAs To<TConcreteType>(this IBindingSyntaxTo container)
		{
			return container.To(typeof(TConcreteType));
		}

		/// <summary>
		/// Adds a binding.
		/// </summary>
		/// <typeparam name="TComponentType"></typeparam>
		/// <param name="container"></param>
		/// <returns></returns>
		public static IBindingSyntaxTo Bind<TComponentType>(this IBindingSyntaxBind container)
		{
			return container.Bind(typeof(TComponentType));
		}

		/// <summary>
		/// Returns whether or not a binding has already been set.
		/// </summary>
		/// <typeparam name="TComponentType"></typeparam>
		/// <returns></returns>
		public static bool HasBinding<TComponentType>(this IBindingSyntaxBind container)
		{
			return container.HasBinding(typeof(TComponentType));
		}

		/// <summary>
		/// Adds a binding, overriding any existing binding.
		/// </summary>
		/// <typeparam name="TComponentType"></typeparam>
		/// <returns></returns>
		public static IBindingSyntaxTo Rebind<TComponentType>(this IBindingSyntaxBind container)
		{
			return container.Rebind(typeof(TComponentType));
		}
	}
}
