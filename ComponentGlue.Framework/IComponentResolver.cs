﻿using System;

namespace ComponentGlue.Framework
{
	/// <summary>
	/// Interface for component which can resolve dependencies on other components.
	/// </summary>
	public interface IComponentResolver
	{
		/// <summary>
		/// Gets an instance of type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		object Get(Type type);

		/// <summary>
		/// Injects components into the properties of the instance marked with an Inject attribute.
		/// </summary>
		/// <param name="instance"></param>
		void Inject(object component);
	}
}
