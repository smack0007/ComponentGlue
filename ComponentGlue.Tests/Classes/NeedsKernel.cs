using System;
using ComponentGlue.Framework;

namespace ComponentGlue.Tests.Classes
{
	public class NeedsKernel
	{
		public IKernel Kernel
		{
			get;
			private set;
		}

		[Inject]
		public NeedsKernel(IKernel kernel)
		{
			this.Kernel = kernel;
		}
	}
}
