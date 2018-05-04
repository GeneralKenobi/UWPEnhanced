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
    public class ContentTranslateTransformY : IValueConverter
    {
		#region IValueConverter

		/// <summary>
		/// Method for <see cref="IValueConverter"/>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is Menu menu)
			{
				return QuickConvert(menu);
			}			

			return 0;
		}

		/// <summary>
		/// Not supported
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

		#region Public Static Methods

		/// <summary>
		/// Performs the conversion based on the given <see cref="Menu"/>
		/// </summary>
		/// <param name="menu"></param>
		/// <returns></returns>
		public static double QuickConvert(Menu menu)
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

			return 0;
		}

		#endregion
	}
}
