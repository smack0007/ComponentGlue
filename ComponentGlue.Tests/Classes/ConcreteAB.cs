using System;

namespace ComponentGlue.Tests.Classes
{
	public class ConcreteAB
	{
		public ConcreteA A
		{
			get;
			private set;
		}

		public ConcreteB B
		{
			get;
			private set;
		}

		public ConcreteAB(ConcreteA a, ConcreteB b)
		{
			this.A = a;
			this.B = b;
		}
	}
}
