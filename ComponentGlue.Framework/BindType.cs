using System;

namespace ComponentGlue.Framework
{
	/// <summary>
	/// Types of bindings.
	/// </summary>
	public enum BindType
	{
		/// <summary>
		/// A new component will always be constructed.
		/// </summary>
		Transient = 0,

		/// <summary>
		/// The component may be shared with other components.
		/// </summary>
		Singleton,

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
