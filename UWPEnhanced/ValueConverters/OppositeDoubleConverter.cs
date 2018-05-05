using CSharpEnhanced.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.ValueConverters
{
	/// <summary>
	/// Converts value to -value. If value is not <see cref="double"/> return 0.
	/// </summary>
	public class OppositeDoubleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is double d)
			{
				return -d;
			}
			else
			{
				return 0;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			if (value is double d)
			{
				return -d;
			}
			else
			{
				return 0;
			}
		}
	}
}
