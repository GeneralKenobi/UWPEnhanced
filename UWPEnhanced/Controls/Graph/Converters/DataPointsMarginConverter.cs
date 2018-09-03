using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Converter for margin around data points container, the value should be the diameter of a single point (double), it is then
	/// negated, divided by 2 and a uniform thickness constructed from the quotient is returned.
	/// </summary>
	internal class DataPointsMarginConverter : IValueConverter
	{
		#region Public methods

		/// <summary>
		/// If value is a double, returns a new uniform thickness equal to the double negated and divided by 2
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is double d)
			{
				return new Thickness(-d / 2);
			}

			return new Thickness();
		}

		/// <summary>
		/// Not implemented
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}