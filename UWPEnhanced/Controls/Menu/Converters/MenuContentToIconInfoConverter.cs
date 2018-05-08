using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.Controls
{
	internal class MenuContentToIconInfoConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is IEnumerable<UIElement> c)
			{
				ObservableCollection<MenuContentIconInfo> result = new ObservableCollection<MenuContentIconInfo>();

				foreach (var item in c)
				{
					result.Add(new MenuContentIconInfo(Menu.GetGlyph(item), Menu.GetImage(item)));
				}

				return result;
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
