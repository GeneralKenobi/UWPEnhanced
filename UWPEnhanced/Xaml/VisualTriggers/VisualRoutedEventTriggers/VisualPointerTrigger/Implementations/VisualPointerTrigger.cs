using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Provides means of notifying whenever one or more pointer routed events is raised with the marked modifiers
	/// </summary>
	public class VisualPointerTrigger : VisualRoutedEventTriggerWithModifiers<PointerRoutedEventArgs>
	{
		#region PointerEvent Dependency Property

		/// <summary>
		/// Type of the event for the trigger
		/// </summary>
		public PointerEventType PointerEvent
		{
			get => (PointerEventType)GetValue(PointerEventProperty);
			set => SetValue(PointerEventProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="PointerEvent"/>
		/// </summary>
		public static readonly DependencyProperty PointerEventProperty =
			DependencyProperty.Register(nameof(PointerEvent), typeof(PointerEventType),
			typeof(VisualPointerTrigger), new PropertyMetadata(default(PointerEventType), 
				new PropertyChangedCallback(PointerEventChanged)));

		/// <summary>
		/// Method called when <see cref="PointerEvent"/> property changes; Unsubscribes from the old event and subscribes to a new one
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void PointerEventChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is VisualPointerTrigger trigger)
			{
				try
				{
					// If the old value could be casted to enum, unsubscribe from the old event
					var tOld = (PointerEventType)e.OldValue;
					trigger.Unsubscribe(tOld);
				}
				catch (Exception) { }

				try
				{
					// If the new value could be casted to enum, unsubscribe from the old event
					var tNew = (PointerEventType)e.NewValue;
					trigger.Subscribe(tNew);
				}
				catch(Exception) { }
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Subscribes to the <paramref name="type"/> event on attached <see cref="UIElement"/>
		/// </summary>
		/// <param name="type"></param>
		private void Subscribe(PointerEventType type)
		{
			// Get the attached, if it's not null (i.e. this event trigger is attached to something)
			if (AttachedTo is UIElement element)
			{
				// Subscribe to appropriate event
				
				if(type.HasFlag(PointerEventType.PointerCanceled))
				{
					element.PointerCanceled += TriggerEvent;
				}

				if(type.HasFlag(PointerEventType.PointerCaptureLost))
				{
					element.PointerCaptureLost += TriggerEvent;
				}

				if(type.HasFlag(PointerEventType.PointerEntered))
				{
					element.PointerEntered += TriggerEvent;
				}

				if(type.HasFlag(PointerEventType.PointerExited))
				{
					element.PointerExited += TriggerEvent;
				}

				if(type.HasFlag(PointerEventType.PointerMoved))
				{
					element.PointerMoved += TriggerEvent;
				}

				if(type.HasFlag(PointerEventType.PointerPressed))
				{
					element.PointerPressed += TriggerEvent;
				}

				if(type.HasFlag(PointerEventType.PointerReleased))
				{
					element.PointerReleased += TriggerEvent;
				}

				if(type.HasFlag(PointerEventType.PointerWheelChanged))
				{
					element.PointerWheelChanged += TriggerEvent;
				}
			}
		}

		/// <summary>
		/// Unsubscribes from the <paramref name="type"/> event on attached <see cref="UIElement"/>
		/// </summary>
		/// <param name="type"></param>
		private void Unsubscribe(PointerEventType type)
		{
			// Get the attached, if it's not null (i.e. this event trigger is attached to something)
			if (AttachedTo is UIElement element)
			{
				// Unsubscribe from appropriate event

				if (type.HasFlag(PointerEventType.PointerCanceled))
				{
					element.PointerCanceled -= TriggerEvent;
				}

				if (type.HasFlag(PointerEventType.PointerCaptureLost))
				{
					element.PointerCaptureLost -= TriggerEvent;
				}

				if (type.HasFlag(PointerEventType.PointerEntered))
				{
					element.PointerEntered -= TriggerEvent;
				}

				if (type.HasFlag(PointerEventType.PointerExited))
				{
					element.PointerExited -= TriggerEvent;
				}

				if (type.HasFlag(PointerEventType.PointerMoved))
				{
					element.PointerMoved -= TriggerEvent;
				}

				if (type.HasFlag(PointerEventType.PointerPressed))
				{
					element.PointerPressed -= TriggerEvent;
				}

				if (type.HasFlag(PointerEventType.PointerReleased))
				{
					element.PointerReleased -= TriggerEvent;
				}

				if (type.HasFlag(PointerEventType.PointerWheelChanged))
				{
					element.PointerWheelChanged -= TriggerEvent;
				}
			}
		}

		#endregion

		#region Protected methods

		/// <summary>
		/// Assigns SetHandled value to <see cref="PointerRoutedEventArgs.Handled"/> property
		/// </summary>
		/// <param name="e"></param>
		protected override void AssignHandled(PointerRoutedEventArgs e) => e.Handled = SetHandled;

		/// <summary>
		/// Subscribes to the <see cref="PointerEvent"/> event on attached <see cref="UIElement"/>
		/// </summary>
		protected override void Subscribe() => Subscribe(PointerEvent);

		/// <summary>
		/// Unsubscribes from the <see cref="PointerEvent"/> event on attached <see cref="UIElement"/>
		/// </summary>
		protected override void Unsubscribe() => Unsubscribe(PointerEvent);

		/// <summary>
		/// Returns modifiers present during the event
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		protected override VirtualKeyModifiers GetModifiers(PointerRoutedEventArgs e) => e.KeyModifiers;

		#endregion
	}
}