using System;

namespace ComponentGlue.Framework.BindingSyntax
{
	public interface IBindingSyntaxTo
	{
		/// <summary>
		/// Sets the component type.
		/// </summary>
		/// <param name="componentType"></param>
		/// <returns></returns>
		IBindingSyntaxAs To(Type componentType);
				
		/// <summary>
		/// Sets the component type to the interface type.
		/// </summary>
		/// <returns></returns>
		IBindingSyntaxAs ToSelf();

		/// <summary>
		/// Sets the component as an already existing instance.
		/// </summary>
		/// <param name="component"></param>
		void ToConstant(object component);

		/// <summary>
		/// Sets a factory method which will be used to generate the component.
		/// </summary>
		/// <param name="factory"></param>
		void ToMethod(Func<Type, Type, object> factory);
	}
}
