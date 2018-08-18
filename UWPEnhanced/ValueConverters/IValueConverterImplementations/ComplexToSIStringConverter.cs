using CSharpEnhanced.Helpers;
using System;
using System.Numerics;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.ValueConverters
{
	/// <summary>
	/// Converts a <see cref="Complex"/> to an alt SIString, if parameter is a <see cref="string"/> uses it as the unit
	/// </summary>
	public class ComplexToSIStringConverter : IValueConverter
	{
		#region Public methods

		/// <summary>
		/// Converts a <see cref="Complex"/> to an SIString, if parameter is a <see cref="string"/> uses it as the unit
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is Complex c)
			{
				return SIHelpers.ToAltSIStringExcludingSmallPrefixes(c, (parameter as string) ?? string.Empty);
			}

			return string.Empty;
		}

		/// <summary>
		/// Not implemented
		/// </summary>
		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}