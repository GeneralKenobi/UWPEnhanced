using System;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.ValueConverters
{
	/// <summary>
	/// If the value is a double returns its absolute value, otherwise returns 0
	/// </summary>
	public class AbsoluteValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is double dValue)
			{
				return Math.Abs(dValue);
			}

			return 0d;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}