using System;

namespace ComponentGlue.Framework
{
	/// <summary>
	/// Interface for IoC Kernels.
	/// </summary>
	public interface IKernel
	{
		/// <summary>
		/// Constructs a new object of type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		object Construct(Type type);

		T Construct<T>();

		/// <summary>
		/// Gets an instance of type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		object Get(Type type);

		T Get<T>();

		void Inject(object component);
	}
}
