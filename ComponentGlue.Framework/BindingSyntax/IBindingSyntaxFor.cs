using System;

namespace ComponentGlue.Framework.BindingSyntax
{
	public interface IBindingSyntaxFor
	{
		IBindingSyntaxBind For(Type constructedType);

		IBindingSyntaxBind For<TConstructedType>();
	}
}
