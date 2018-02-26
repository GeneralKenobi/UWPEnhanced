using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace UWPEnhanced.Controls
{
    public sealed class Menu : Control
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public Menu()
        {
            this.DefaultStyleKey = typeof(Menu);
			t();
        }

		private async Task t()
		{
			await Task.Delay(2000);
			VisualStateManager.GoToState(this, "Test1", false);
		}

		#endregion

		#region MenuPositionProperty Dependency Property

		/// <summary>
		/// Position of the menu
		/// </summary>
		public MenuPosition MenuPositionProperty
		{
			get => (MenuPosition)GetValue(MenuPositionPropertyProperty);
			set => SetValue(MenuPositionPropertyProperty, value);
		}

		/// <summary>
		/// Position of the menu
		/// </summary>
		public static readonly DependencyProperty MenuPositionPropertyProperty =
			DependencyProperty.Register(nameof(MenuPositionProperty), typeof(MenuPosition),
			typeof(Menu), new PropertyMetadata(MenuPosition.Left));

		#endregion


	}
}
