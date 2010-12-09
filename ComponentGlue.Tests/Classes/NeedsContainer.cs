using System;
using ComponentGlue.Framework;

namespace ComponentGlue.Tests.Classes
{
	public class NeedsContainer
	{
		public IComponentContainer Container
		{
			get;
			private set;
		}

		[Inject]
		public NeedsContainer(IComponentContainer container)
		{
			this.Container = container;
		}
	}
}
