using System;

namespace ComponentGlue.Tests.Classes
{
	public class CircularDependency2
	{
		public CircularDependency2(CircularDependency1 dependency)
		{
		}
	}
}
