using System;
using ComponentGlue.Framework.BindingSyntax;

namespace ComponentGlue.Framework
{
	public static class ExtensionMethods
	{
		/// <summary>
		/// Gets an instance of type T.
		/// </summary>
		/// <typeparam name="TComponentType"></typeparam>
		/// <returns></returns>
		public static TComponentType Get<TComponentType>(this IKernel kernel)
		{
			return (TComponentType)kernel.Get(typeof(TComponentType));
		}

		/// <summary>
		/// Begins a specific binding.
		/// </summary>
		/// <param name="constructedType"></param>
		/// <returns></returns>
		public static IBindingSyntaxBind For<TConstructedType>(this IBindingSyntaxFor kernel)
		{
			return kernel.For(typeof(TConstructedType));
		}

		/// <summary>
		/// Sets the component type.
		/// </summary>
		/// <typeparam name="TComponentType"></typeparam>
		/// <returns></returns>
		public static IBindingSyntaxAs To<TComponentType>(this IBindingSyntaxTo kernel)
		{
			return kernel.To(typeof(TComponentType));
		}

		/// <summary>
		/// Adds a binding.
		/// </summary>
		/// <typeparam name="TInterfaceType"></typeparam>
		/// <param name="kernel"></param>
		/// <returns></returns>
		public static IBindingSyntaxTo Bind<TInterfaceType>(this IBindingSyntaxBind kernel)
		{
			return kernel.Bind(typeof(TInterfaceType));
		}

		/// <summary>
		/// Returns whether or not a binding has already been set.
		/// </summary>
		/// <typeparam name="TInterfaceType"></typeparam>
		/// <returns></returns>
		public static bool HasBinding<TInterfaceType>(this IBindingSyntaxBind kernel)
		{
			return kernel.HasBinding(typeof(TInterfaceType));
		}

		/// <summary>
		/// Adds a binding, overriding any existing binding.
		/// </summary>
		/// <typeparam name="TInterfaceType"></typeparam>
		/// <returns></returns>
		public static IBindingSyntaxTo Rebind<TInterfaceType>(this IBindingSyntaxBind kernel)
		{
			return kernel.Rebind(typeof(TInterfaceType));
		}


	}
}
