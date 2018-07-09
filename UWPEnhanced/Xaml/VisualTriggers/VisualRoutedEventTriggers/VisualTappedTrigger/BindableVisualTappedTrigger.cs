using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// <see cref="VisualTappedTrigger"/> which can be bound to an element
	/// (the element is attached as the <see cref="IAttachable.AttachedTo"/>)
	/// </summary>
	public class BindableVisualTappedTrigger : VisualTappedTrigger
	{
		#region Target Dependency Property

		/// <summary>
		/// Element whose events to listen to
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
			typeof(BindableVisualPointerTrigger), new PropertyMetadata(null, new PropertyChangedCallback(TargetChangedCallback)));

		#endregion

		#region Private Static Methods

		/// <summary>
		/// Detatches the old value and attaches then new one (if it's not null)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private static void TargetChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if(sender is BindableVisualPointerTrigger trigger)
			{
				trigger.Detach();

				if(args.NewValue is FrameworkElement newElement)
				{
					trigger.Attach(newElement);
				}
			}
		}

		#endregion
	}
}