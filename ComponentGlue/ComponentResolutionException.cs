using System;

namespace ComponentGlue
{
	/// <summary>
	/// Exception thrown when an error occurs while trying to resolve a component.
	/// </summary>
	public class ComponentResolutionException : InvalidOperationException
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message"></param>
		public ComponentResolutionException(string message)
			: base(message)
		{
		}
	}
}
