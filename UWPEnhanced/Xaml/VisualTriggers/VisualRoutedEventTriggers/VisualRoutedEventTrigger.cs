using System;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Base class for visual triggers that operate based on some routed events and that may set the handled property of those
	/// events to prevent them from bubbling up the visual tree. <typeparamref name="T"/> is the type of the RoutedEventArgs for
	/// specific implementations.
	/// </summary>
	public class VisualRoutedEventTrigger<T> : VisualAttachment, IVisualTrigger<T>
		where T : RoutedEventArgs
	{
		#region Events

		/// <summary>
		///  Event fired when the criteria for trigger are met (appropriate pointer event with matching modifiers)
		/// </summary>
		public EventHandler<T> Triggered { get; set; }

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
			typeof(VisualRoutedEventTrigger<T>), new PropertyMetadata(default(bool)));

		#endregion

		#region Target Dependency Property

		/// <summary>
		/// Target to listen to (attach to) instead of the default one determined by means of <see cref="VisualAttachment"/>'s
		/// inherited methods. To use the default one this property should be left unset
		/// </summary>
		public FrameworkElement Target
		{
			get => (FrameworkElement)GetValue(TargetProperty);
			set => SetValue(TargetProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Target"/>
		/// </summary>
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register(nameof(Target), typeof(FrameworkElement),
			typeof(VisualRoutedEventTrigger<T>),
			new PropertyMetadata(default(FrameworkElement), new PropertyChangedCallback(TargetChangedCallback)));

		#endregion

		#region Private Static Methods

		/// <summary>
		/// Detatches the old value and attaches then new one (if it's not null)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private static void TargetChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			// If sender is an instance of VisualRoutedEventArgs<T>
			if (sender is VisualRoutedEventTrigger<T> trigger && args.NewValue is FrameworkElement newElement)
			{
				// Detatch from the current FrameworkElement
				trigger.Detach();

				// And attach to the new FrameworkElement
				trigger.Attach(newElement);
			}
		}

		#endregion
	}
}