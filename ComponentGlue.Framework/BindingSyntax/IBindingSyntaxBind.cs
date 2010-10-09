using System;

namespace ComponentGlue.Framework.BindingSyntax
{
	public interface IBindingSyntaxBind
	{
		IBindingSyntaxTo Bind(Type interfaceType);

		IBindingSyntaxTo Bind<TInterfaceType>();
	}
}
