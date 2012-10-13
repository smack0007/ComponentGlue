using System;
using ComponentGlue;

namespace ComponentGlue.Tests.Classes
{
	public class CircularDependencyProperty1
	{
		[Inject]
		public CircularDependencyProperty2 Dependency
		{
			get;
			private set;
		}
	}
}
