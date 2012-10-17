using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Tests.Classes
{
	public class FuncClass<T>
	{
		Func<T> func;

		public FuncClass(Func<T> func)
		{
			if (func == null)
				throw new ArgumentNullException("func");

			this.func = func;
		}

		public T InvokeFunc()
		{
			return this.func();
		}
	}
}
