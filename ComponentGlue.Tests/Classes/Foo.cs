﻿using System;
using ComponentGlue.Framework;

namespace ComponentGlue.Tests.Classes
{
	public class Foo : IFoo
	{
		public IBar Bar
		{
			get;
			private set;
		}
		
		[Inject]
		public Foo(IBar bar)
		{
			this.Bar = bar;
		}
	}
}
