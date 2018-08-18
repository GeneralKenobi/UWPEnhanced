using CSharpEnhanced.Helpers;
using System;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.ValueConverters
{
	/// <summary>
	/// Converts a <see cref="double"/> to an SIString, if parameter is a <see cref="string"/> uses it as the unit,
	/// allows for converting back
	/// </summary>
	public class DoubleToSIStringConverter : IValueConverter
	{
		#region Public methods

		/// <summary>
		/// Converts a <see cref="double"/> to an SIString, if parameter is a <see cref="string"/> uses it as the unit
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is double d)
			{
				return SIHelpers.ToSIStringExcludingSmallPrefixes(d, (parameter as string) ?? string.Empty);
			}

			return string.Empty;
		}

		/// <summary>
		/// If value is a <see cref="string"/> tries to parse it to a double and returns it if successful.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			if (value is string s && SIHelpers.TryParseSIString(s, out double result))
			{
				return result;
			}

			return 0;
		}

		#endregion
	}
}