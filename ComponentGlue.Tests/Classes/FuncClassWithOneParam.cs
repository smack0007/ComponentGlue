using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Tests.Classes
{
	public class FuncClassWithOneParam<T, TResult>
	{
		Func<T, TResult> func;

		public FuncClassWithOneParam(Func<T, TResult> func)
		{
			if (func == null)
				throw new ArgumentNullException("func");

			this.func = func;
		}

		public TResult InvokeFunc(T t1)
		{
			return this.func(t1);
		}
	}
}
