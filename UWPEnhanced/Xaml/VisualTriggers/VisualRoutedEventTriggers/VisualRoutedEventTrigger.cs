using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Base class for visual triggers that operate based on some routed events and that may set the handled property of those
	/// events to prevent them from bubbling up the visual tree
	/// </summary>
	public class VisualRoutedEventTrigger : VisualAttachment, IVisualTrigger
	{
		#region Events

		/// <summary>
		///  Event fired when the criteria for trigger are met (appropriate pointer event with matching modifiers)
		/// </summary>
		public EventHandler Triggered { get; set; }

		#endregion

		#region SetHandled Dependency Property

		/// <summary>
		/// Sets the handled property on the intercepted <see cref="RoutedEventArgs"/> to this value
		/// </summary>
		public bool SetHandled
		{
			get => (bool)GetValue(SetHandledProperty);
			set => SetValue(SetHandledProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="SetHandled"/>
		/// </summary>
		public static readonly DependencyProperty SetHandledProperty =
			DependencyProperty.Register(nameof(SetHandled), typeof(bool),
			typeof(VisualRoutedEventTrigger), new PropertyMetadata(default(bool)));

		#endregion
	}
}