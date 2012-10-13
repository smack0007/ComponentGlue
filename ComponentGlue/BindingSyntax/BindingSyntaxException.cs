using System;

namespace ComponentGlue.BindingSyntax
{
	/// <summary>
	/// Exception thrown when an error occurs while using binding syntax.
	/// </summary>
	public class BindingSyntaxException : InvalidOperationException
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message"></param>
		public BindingSyntaxException(string message)
			: base(message)
		{
		}
	}
}
