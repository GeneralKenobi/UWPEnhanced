using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWPEnhanced.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace UWPEnhanced.Controls
{
    public sealed class ItemsContainer : ItemsControl
    {
        public ItemsContainer()
        {
            this.DefaultStyleKey = typeof(ItemsContainer);			
        }

		protected override Size ArrangeOverride(Size finalSize)
		{
			var a = VisualTreeHelpers.FindChild<ItemsContainerPanel>(this);
			a.ItemSpacing = 30;
			a.Arrange(new Rect(new Point(0, 0), finalSize));

			return finalSize;
		}
	}
}