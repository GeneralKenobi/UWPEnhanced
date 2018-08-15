using System;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Base class for visual triggers that operate based on some routed events and that may set the handled property of those
	/// events to prevent them from bubbling up the visual tree. <typeparamref name="T"/> is the specific type of the RoutedEventArgs
	/// for specific implementations.
	/// </summary>
	public abstract class VisualRoutedEventTrigger<T> : VisualTrigger
		where T : RoutedEventArgs
	{
		#region SetHandled Dependency Property

		/// <summary>
		/// Sets the handled property on the intercepted <see cref="RoutedEventArgs"/> to this value (if the given
		/// <see cref="RoutedEventArgs"/> has that property (it usually does))
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
		public UIElement Target
		{
			get => (UIElement)GetValue(TargetProperty);
			set => SetValue(TargetProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Target"/>
		/// </summary>
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register(nameof(Target), typeof(UIElement),
			typeof(VisualRoutedEventTrigger<T>),
			new PropertyMetadata(default(UIElement), new PropertyChangedCallback(TargetChangedCallback)));

		#endregion

		#region Protected methods

		/// <summary>
		/// Subscribes to the specific RoutedEvent
		/// </summary>
		protected abstract void Subscribe();

		/// <summary>
		/// Unsubscribes from the specific RoutedEvent
		/// </summary>
		protected abstract void Unsubcribe();

		/// <summary>
		/// Sets the Handled property on a specific <see cref="RoutedEventArgs"/>
		/// </summary>
		/// <param name="e"></param>
		protected abstract void AssignHandled(T e);

		/// <summary>
		/// Check method, event won't be fired if it returns false. By default returns true. May be overriden to provide specific checks
		/// </summary>
		/// <returns></returns>
		protected virtual bool TriggerConditionsMet() => true;

		/// <summary>
		/// Method calling <see cref="AssignHandled(T)"/> and then firing the Triggered event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void TriggerEvent(object sender, T e)
		{
			if (TriggerConditionsMet())
			{
				// Let the derived classes set the Handled property on their specific RoutedEventArgs
				AssignHandled(e);

				// Invoke the event
				Triggered?.Invoke(this, e);
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		/// On top of the standard functionality, it unsubscribes from the old <see cref="UIElement"/> and subscribes to
		/// the new one
		/// </summary>
		/// <param name="obj"></param>
		public override void Attach(DependencyObject obj)
		{
			if (AttachedTo != null)
			{
				Unsubcribe();
			}

			// Only UIElements and above have routed events
			if (obj is UIElement)
			{
				base.Attach(obj);

				if (AttachedTo != null)
				{
					Subscribe();
				}
			}
			else
			{
				throw new ArgumentException("Routed Events are introduced in " + nameof(UIElement) + "; " +
					nameof(obj) + " has to derive from " + nameof(UIElement));
			}
		}

		/// <summary>
		/// On top of the standard functionality also unsubscribes from the detatched object
		/// </summary>
		public override void Detach()
		{
			if (AttachedTo != null)
			{
				Unsubcribe();
			}

			base.Detach();
		}

		#endregion

		#region Private Static Methods

		/// <summary>
		/// Detatches the old value and attaches then new one (if it's not null and is an <see cref="UIElement"/>)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private static void TargetChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			// If sender is an instance of VisualRoutedEventArgs<T>
			if (sender is VisualRoutedEventTrigger<T> trigger && args.NewValue is UIElement newElement)
			{
				// Detatch from the current UIElement
				trigger.Detach();

				// And attach to the new UIElement
				trigger.Attach(newElement);
			}
		}

		#endregion
	}
}