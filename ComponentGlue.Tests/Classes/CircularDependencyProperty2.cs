using System;
using ComponentGlue;

namespace ComponentGlue.Tests.Classes
{
	public class CircularDependencyProperty2
	{
		[Inject]
		public CircularDependencyProperty1 Dependency
		{
			get;
			private set;
		}
	}
}
