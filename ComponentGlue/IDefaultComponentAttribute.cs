using System;

namespace ComponentGlue
{
	public interface IDefaultComponentAttribute
	{
		/// <summary>
		/// The interface this component is the default for. 
		/// </summary>
		Type InterfaceType { get; }
	}
}
