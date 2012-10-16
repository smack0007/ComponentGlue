using System;

namespace ComponentGlue.Tests.Attributes
{
	public class CustomDefaultComponentAttribute : Attribute, IDefaultComponentAttribute
	{
		public Type ComponentType
		{
			get;
			set;
		}
	}
}
