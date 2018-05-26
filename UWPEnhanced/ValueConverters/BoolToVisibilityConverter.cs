using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.ValueConverters
{
	/// <summary>
	/// Converts true to <see cref="Visibility.Visible"/> and false to <see cref="Visibility.Collapsed"/>.
	/// If any parameter is given then swaps the conversion results.
	/// If value is not a <see cref="bool"/> then always returns <see cref="Visibility.Visible"/>.
	/// </summary>
	class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is bool b)
			{
				return (b ^ parameter != null) ? Visibility.Visible : Visibility.Collapsed;
			}

			return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
