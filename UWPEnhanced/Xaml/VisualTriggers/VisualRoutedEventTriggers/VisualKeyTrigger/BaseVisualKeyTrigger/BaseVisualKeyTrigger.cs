using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Base class for key related VisualTriggers that utilise RoutedEvents with <see cref="KeyRoutedEventArgs"/> argument
	/// </summary>
	public abstract class BaseVisualKeyTrigger : VisualRoutedEventTriggerWithModifiers<KeyRoutedEventArgs>
	{
		#region EventType Dependency Property

		/// <summary>
		/// The type of key event to subscribe to. By default it's <see cref="KeyEventType.KeyDown"/>
		/// </summary>
		public KeyEventType EventType
		{
			get => (KeyEventType)GetValue(EventTypeProperty);
			set => SetValue(EventTypeProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="EventType"/>
		/// </summary>
		public static readonly DependencyProperty EventTypeProperty =
			DependencyProperty.Register(nameof(EventType), typeof(KeyEventType),
			typeof(BaseVisualKeyTrigger), new PropertyMetadata(KeyEventType.KeyDown, new PropertyChangedCallback(EventTypeChanged)));

		#endregion
		
		#region Private methods

		/// <summary>
		/// Subscribes to the <paramref name="eventType"/> event on attached <see cref="UIElement"/>
		/// </summary>
		/// <param name="eventType"></param>
		private void Subscribe(KeyEventType eventType)
		{
			// Get the attached as UIElement, if it's not null (i.e. this event trigger is attached to an UIElement) subscribe
			// accordingly to eventType
			if (AttachedTo is UIElement element)
			{
				if (eventType.HasFlag(KeyEventType.KeyDown))
				{
					element.KeyDown += TriggerEvent;
				}

				if (eventType.HasFlag(KeyEventType.KeyUp))
				{
					element.KeyUp += TriggerEvent;
				}
			}
		}

		/// <summary>
		/// Unsubscribes from the <paramref name="type"/> event on attached <see cref="UIElement"/>
		/// </summary>
		/// <param name="type"></param>
		private void Unsubscribe(KeyEventType eventType)
		{
			// Get the attached as UIElement, if it's not null (i.e. this event trigger is attached to an UIElement) unsubscribe
			// accordingly to eventType
			if (AttachedTo is UIElement element)
			{
				if (eventType.HasFlag(KeyEventType.KeyDown))
				{
					element.KeyDown -= TriggerEvent;
				}

				if (eventType.HasFlag(KeyEventType.KeyUp))
				{
					element.KeyUp -= TriggerEvent;
				}
			}
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Subscribes to the <see cref="EventType"/> event on attached <see cref="UIElement"/>
		/// </summary>
		protected override void Subscribe() => Subscribe(EventType);

		/// <summary>
		/// Unsubscribes from the <see cref="EventType"/> event on attached <see cref="UIElement"/>
		/// </summary>
		protected override void Unsubscribe() => Unsubscribe(EventType);

		/// <summary>
		/// Assigns handled accoring to SetHandled on <see cref="KeyRoutedEventArgs"/>
		/// </summary>
		/// <param name="e"></param>
		protected override void AssignHandled(KeyRoutedEventArgs e) => e.Handled = SetHandled;

		#endregion

		#region Private static methods

		/// <summary>
		/// Method called when <see cref="EventType"/> property changes; Unsubscribes from the old event and subscribes to a new one
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void EventTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is BaseVisualKeyTrigger trigger)
			{
				try
				{
					// If the old value could be casted to enum, unsubscribe from the old event
					var tOld = (KeyEventType)e.OldValue;
					trigger.Unsubscribe(tOld);
				}
				catch (Exception) { }

				try
				{
					// If the new value could be casted to enum, unsubscribe from the old event
					var tNew = (KeyEventType)e.NewValue;
					trigger.Subscribe(tNew);
				}
				catch (Exception) { }
			}
		}

		#endregion
	}
}