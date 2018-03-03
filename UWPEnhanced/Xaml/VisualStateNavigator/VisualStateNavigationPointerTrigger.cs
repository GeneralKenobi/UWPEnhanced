using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	public class VisualStateNavigationPointerTrigger : VisualStateNavigationTrigger
	{
		#region TriggerFor Dependency Property

		/// <summary>
		/// Pointer events are captured for this <see cref="UIElement"/>
		/// </summary>
		public UIElement TriggerFor
		{
			get => (UIElement)GetValue(TriggerForProperty);
			set => SetValue(TriggerForProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="TriggerFor"/>
		/// </summary>
		public static readonly DependencyProperty TriggerForProperty =
			DependencyProperty.Register(nameof(TriggerFor), typeof(UIElement),
			typeof(VisualStateNavigationPointerTrigger), new PropertyMetadata(default(UIElement)));

		#endregion

		#region TriggerTypes Dependency Property

		/// <summary>
		/// PointerEvents for which to trigger
		/// </summary>
		public VisualStateNavigationPointerTriggerEventType TriggerTypes
		{
			get => (VisualStateNavigationPointerTriggerEventType)GetValue(TriggerTypesProperty);
			set => SetValue(TriggerTypesProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="TriggerTypes"/>
		/// </summary>
		public static readonly DependencyProperty TriggerTypesProperty =
			DependencyProperty.Register(nameof(TriggerTypes), typeof(VisualStateNavigationPointerTriggerEventType),
			typeof(VisualStateNavigationPointerTrigger),
			new PropertyMetadata(new VisualStateNavigationPointerTriggerEventType(),
				new PropertyChangedCallback(TriggerTypesChanged)));

		private static void TriggerTypesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			
		}

		#endregion
	}
}
