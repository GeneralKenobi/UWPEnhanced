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
	internal class MenuContentToIconInfoConverter : DependencyObject, IValueConverter
	{
		#region Owner Dependency Property

		/// <summary>
		/// Menu which owns the Icon this converter creates info for
		/// </summary>
		public Menu Owner
		{
			get => (Menu)GetValue(OwnerProperty);
			set => SetValue(OwnerProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Owner"/>
		/// </summary>
		public static readonly DependencyProperty OwnerProperty =
			DependencyProperty.Register(nameof(Owner), typeof(Menu),
			typeof(MenuContentToIconInfoConverter), new PropertyMetadata(default(Menu)));

		#endregion


		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is IEnumerable<UIElement> c)
			{
				ObservableCollection<MenuContentIconInfo> result = new ObservableCollection<MenuContentIconInfo>();

				foreach (var item in c)
				{
					result.Add(new MenuContentIconInfo(Menu.GetGlyph(item), Menu.GetImage(item),
						Owner == null ? null : Owner.IconPressedCommand, item, Owner.FontSize, Owner.Foreground));
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
