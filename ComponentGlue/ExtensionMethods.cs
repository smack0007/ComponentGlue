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
		public static TComponentType Get<TComponentType>(this IComponentContainer container)
		{
			return (TComponentType)container.Get(typeof(TComponentType));
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
		/// <typeparam name="TComponentType"></typeparam>
		/// <returns></returns>
		public static IBindingSyntaxAs To<TComponentType>(this IBindingSyntaxTo container)
		{
			return container.To(typeof(TComponentType));
		}

		/// <summary>
		/// Adds a binding.
		/// </summary>
		/// <typeparam name="TInterfaceType"></typeparam>
		/// <param name="container"></param>
		/// <returns></returns>
		public static IBindingSyntaxTo Bind<TInterfaceType>(this IBindingSyntaxBind container)
		{
			return container.Bind(typeof(TInterfaceType));
		}

		/// <summary>
		/// Returns whether or not a binding has already been set.
		/// </summary>
		/// <typeparam name="TInterfaceType"></typeparam>
		/// <returns></returns>
		public static bool HasBinding<TInterfaceType>(this IBindingSyntaxBind container)
		{
			return container.HasBinding(typeof(TInterfaceType));
		}

		/// <summary>
		/// Adds a binding, overriding any existing binding.
		/// </summary>
		/// <typeparam name="TInterfaceType"></typeparam>
		/// <returns></returns>
		public static IBindingSyntaxTo Rebind<TInterfaceType>(this IBindingSyntaxBind container)
		{
			return container.Rebind(typeof(TInterfaceType));
		}
	}
}
