using System;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// If value is a <see cref="MenuPosition"/>, returns a proper <see cref="ItemsDirection"/> for the IconsPanel.
	/// Otherwise returns <see cref="ItemsDirection.TopToBottom"/>
	/// </summary>
	public class MenuPositionToItemsDirectionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is MenuPosition position)
			{
				switch (position)
				{
					case MenuPosition.Left:
					case MenuPosition.Right:
						{
							return ItemsDirection.TopToBottom;
						}

					case MenuPosition.Top:
					case MenuPosition.Bottom:
						{
							return ItemsDirection.LeftToRight;
						}
				}
			}

			return ItemsDirection.TopToBottom;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
