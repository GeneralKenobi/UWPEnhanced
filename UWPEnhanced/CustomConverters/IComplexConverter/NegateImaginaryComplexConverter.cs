using System.Numerics;
using UWPEnhanced.ValueConverters;

namespace UWPEnhanced.CustomConverters
{
	/// <summary>
	/// Negates only imaginary part of a complex number 
	/// </summary>
	public class NegateImaginaryComplexConverter : IComplexConverter
	{
		#region Public methods

		/// <summary>
		/// Converts one <see cref="Complex"/> into another
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public Complex Convert(Complex value) => new Complex(value.Real, -value.Imaginary);

		#endregion
	}
}