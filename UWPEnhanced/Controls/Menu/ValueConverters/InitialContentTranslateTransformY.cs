using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Converter for the intial value of <see cref="TranslateTransform.X"/> of ContentPresenter in the
	/// <see cref="Menu"/>. Value should be the <see cref="Menu.Position"/>. In all other cases 0 is returned.
	/// </summary>
    public class InitialContentTranslateTransformY : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is Menu menu)
			{
				switch (menu.Position)
				{
					case MenuPosition.Top:
						{
							return -menu.ContentLength;
						}

					case MenuPosition.Bottom:
						{
							return menu.ContentLength;
						}
				}
			}			

			return 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
