using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.ValueConverters
{
	/// <summary>
	/// Converts <see cref="double"/> to a <see cref="GridLength"/>. If parameter is "*", then the GridUnitType will be star
	/// </summary>
	public class DoubleToGridLength : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is double d)
			{
				if(parameter is string stringParameter && stringParameter == "*")
				{
					return new GridLength(d, GridUnitType.Star);
				}
				else
				{
					return new GridLength(d);
				}
			}

			return new GridLength();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
