using System;

namespace ComponentGlue.Tests.Classes
{
	public class CircularDependency1
	{
		public CircularDependency1(CircularDependency2 dependency)
		{
		}
	}
}
