using System.Numerics;

namespace UWPEnhanced.ValueConverters
{
	/// <summary>
	/// Interface allowing for conversion of a complex number
	/// </summary>
	public interface IComplexConverter
	{
		#region Methods

		/// <summary>
		/// Converts one <see cref="Complex"/> into another
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		Complex Convert(Complex value);

		#endregion
	}
}