using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace UWPEnhanced.Xaml
{
	public class PointerOverST : StateTriggerBase
	{
		public PointerOverST()
		{
		}



		public UIElement Owner
		{
			get { return (UIElement)GetValue(OwnerProperty); }
			set { SetValue(OwnerProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Owner.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty OwnerProperty =
			DependencyProperty.Register("Owner", typeof(UIElement), typeof(PointerOverST), new PropertyMetadata(null, new PropertyChangedCallback((s,e)=>
				{
					if(e.NewValue is Grid grid)
					{
						grid.PointerPressed += (x, y) =>
						{

						};
					}

				})));



	}

    
}
