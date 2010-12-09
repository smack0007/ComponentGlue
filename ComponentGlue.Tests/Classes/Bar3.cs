﻿using System;
using ComponentGlue.Framework;

namespace ComponentGlue.Tests.Classes
{
	[DefaultComponent(typeof(IBar))]
	public class Bar3 : IBar
	{
		public IBaz Baz
		{
			get;
			private set;
		}

		[Inject]
		public Bar3(IBaz baz)
		{
			this.Baz = baz;
		}
	}
}
