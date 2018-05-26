using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.Controls
{
	internal class RestrictedMenuPositionToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is RestrictedMenuPositions restrictedPosition && parameter is string castedParameter &&
				Enum.TryParse<RestrictedMenuPositions>(castedParameter, out RestrictedMenuPositions representedPosition) &&
				restrictedPosition.HasFlag(representedPosition))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
