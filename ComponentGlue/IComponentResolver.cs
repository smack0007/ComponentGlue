using System;

namespace ComponentGlue
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
		object Resolve(Type type);

		/// <summary>
        /// Injects components into the instance, setting properties of the object marked with an Inject attribute.
		/// </summary>
		/// <param name="instance"></param>
		void ResolveProperties(object component);
	}
}
