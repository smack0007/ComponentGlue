﻿using System;

namespace ComponentGlue.BindingSyntax
{
	public interface IBindingSyntaxFor
	{
		/// <summary>
		/// Begins a specific binding.
		/// </summary>
		/// <param name="constructedType"></param>
		/// <returns></returns>
		IBindingSyntaxBind For(Type constructedType);
	}
}
