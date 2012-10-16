using ComponentGlue.Tests.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Tests.Classes
{
	[CustomDefaultComponent(InterfaceType = typeof(IBar))]
	public class BarWithCustomDefaultComponentAttribte : IBar
	{
	}
}
