﻿using System;

namespace ComponentGlue.Tests.Attributes
{
	public class CustomDefaultComponentAttribute : Attribute, IDefaultComponentAttribute
	{
		public Type InterfaceType
		{
			get;
			set;
		}
	}
}
