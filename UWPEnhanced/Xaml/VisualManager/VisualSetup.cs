using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPEnhanced.Xaml
{
	
    public class VisualSetup : DependencyObject
    {


		public VisualState State
		{
			get => (VisualState)GetValue(VisualStateProperty);
			set => SetValue(VisualStateProperty, value);
		}

		// Using a DependencyProperty as the backing store for VisualState.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty VisualStateProperty =
			DependencyProperty.Register(nameof(State), typeof(VisualState), typeof(VisualSetup), new PropertyMetadata(null));




		private async void t(VisualState state)
		{
			if(state.Name=="test")
			{
				await Task.Delay(2000);
			}
			await Task.Delay(1000);
			//var control = VisualTreeHelpers.FindParentOrSelf<Control>(AttachedTo);
			VisualStateGroup group = new VisualStateGroup();
			group.States.Add(state);
			//VisualStateManager.GetVisualStateGroups(AttachedTo as FrameworkElement).Add(group);
			//VisualStateManager.GoToState(control, state.Name, true);
		}
	}
}
