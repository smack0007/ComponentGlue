using System;

namespace ComponentGlue.Framework
{
	/// <summary>
	/// Types of bindings.
	/// </summary>
	public enum BindType
	{
		/// <summary>
		/// The component may be shared with other components.
		/// </summary>
		Shared = 0,

		/// <summary>
		/// A new component will always be constructed.
		/// </summary>
		New,

		/// <summary>
		/// The component instance is specified.
		/// </summary>
		Constant,

		/// <summary>
		/// The component will be generated from a factory method.
		/// </summary>
		Method
	}
}
