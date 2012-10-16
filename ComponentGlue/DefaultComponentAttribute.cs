﻿using System;

namespace ComponentGlue
{
	/// <summary>
	/// Marks a class as the default component.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=false)]
	public class DefaultComponentAttribute : Attribute, IDefaultComponentAttribute
	{
		/// <summary>
		/// The interface this component is the default for. 
		/// </summary>
		public Type InterfaceType
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="interfaceType"></param>
		public DefaultComponentAttribute(Type interfaceType)
		{
			this.InterfaceType = interfaceType;
		}
	}
}
