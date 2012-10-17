using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Tests.Classes
{
	public class AutoFactoryClass<T>
	{
		Func<T> factory;

		public AutoFactoryClass(Func<T> factory)
		{
			if (factory == null)
				throw new ArgumentNullException("factory");

			this.factory = factory;
		}

		public T InvokeFactory()
		{
			return this.factory();
		}
	}
}
