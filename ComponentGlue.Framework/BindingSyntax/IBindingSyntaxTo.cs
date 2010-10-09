using System;

namespace ComponentGlue.Framework.BindingSyntax
{
	public interface IBindingSyntaxTo
	{
		IBindingSyntaxAs To(Type componentType);

		IBindingSyntaxAs To<TComponentType>();

		IBindingSyntaxAs ToSelf();

		void ToConstant(object component);

		void ToMethod(Func<Type, Type, object> factory);
	}
}
