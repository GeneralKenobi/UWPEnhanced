using System;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.ValueConverters
{
	/// <summary>
	/// If the value is a double and the parameter is a double returns value*parameter.
	/// If the value is a double and the parameter is not a double returns value.
	/// Otherwise returns 0.
	/// </summary>
	public class ProductConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is double dValue)
			{
				if(parameter is double dParameter || (parameter is string sParameter && 
					double.TryParse(sParameter, out dParameter)))
				{
					return dValue * dParameter;
				}
				else
				{
					return dValue;
				}
			}
			else
			{
				return 0d;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
