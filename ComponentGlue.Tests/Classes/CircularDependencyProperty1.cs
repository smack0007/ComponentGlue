using System;
using ComponentGlue;

namespace ComponentGlue.Tests.Classes
{
	public class CircularDependencyProperty1
	{
		[Resolve]
		public CircularDependencyProperty2 Dependency
		{
			get;
			private set;
		}
	}
}
