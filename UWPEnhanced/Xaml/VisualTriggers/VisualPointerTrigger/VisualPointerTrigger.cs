using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace UWPEnhanced.Xaml
{
	public class VisualPointerTrigger : VisualAttachment, IVisualTrigger
	{
		#region Events

		/// <summary>
		///  Event fired when the criteria for trigger are met (appropriate pointer event with matching modifiers)
		/// </summary>
		public EventHandler Triggered { get; }

		#endregion

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
				if (e.OldValue is PointerEventType tOld)
				{
					trigger.Unsubcribe(tOld);
				}

				if (e.NewValue is PointerEventType tNew)
				{
					trigger.Subcribe(tNew);
				}
			}
		}

		#endregion

		#region Modifiers Dependency Property

		/// <summary>
		/// Modifiers that will warrant the trigger. Setting <see cref="uint.MaxValue"/> (which is the default value)
		/// will ignore modifiers
		/// </summary>
		public VirtualKeyModifiers Modifiers
		{
			get => (VirtualKeyModifiers)GetValue(ModifiersProperty);
			set => SetValue(ModifiersProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Modifiers"/>
		/// </summary>
		public static readonly DependencyProperty ModifiersProperty =
			DependencyProperty.Register(nameof(Modifiers), typeof(VirtualKeyModifiers),
			typeof(VisualPointerTrigger), new PropertyMetadata((VirtualKeyModifiers)uint.MaxValue));

		#endregion

		#region StrictModifierCheck Dependency Property

		/// <summary>
		/// If true, the event will be fired if and only if all modifiers match the current value.
		/// If false, the event will be fired if the modifiers contain the specified modifiers.
		/// Ex: If set modifier are Ctrl only, if true the trigger will fire if and only if the pointer event has Ctrl modifier only,
		/// if false the trigger will trigger for, among others, Ctrl; Ctrl+Shift; Ctrl+Menu+Shift.
		/// Default value is false
		/// </summary>
		public bool StrictModifierCheck
		{
			get => (bool)GetValue(StrictModifierCheckProperty);
			set => SetValue(StrictModifierCheckProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="StrictModifierCheck"/>
		/// </summary>
		public static readonly DependencyProperty StrictModifierCheckProperty =
			DependencyProperty.Register(nameof(StrictModifierCheck), typeof(bool),
			typeof(VisualPointerTrigger), new PropertyMetadata(false));

		#endregion

		#region Private Methods

		/// <summary>
		/// Subscribes to the <paramref name="type"/> event on attached <see cref="UIElement"/>
		/// </summary>
		/// <param name="type"></param>
		private void Subcribe(PointerEventType type)
		{
			// Get the attached
			var element = AttachedTo as UIElement;
			
			// If it's not null (i.e. this event trigger is attached to something)
			if (element != null)
			{
				// Subscribe to an appropriate event
				switch (type)
				{
					case PointerEventType.PointerCanceled:
						{
							element.PointerCanceled += TriggerEvent;
						} break;

					case PointerEventType.PointerCaptureLost:
						{
							element.PointerCaptureLost += TriggerEvent;
						} break;

					case PointerEventType.PointerEntered:
						{
							element.PointerEntered += TriggerEvent;
						} break;

					case PointerEventType.PointerExited:
						{
							element.PointerExited += TriggerEvent;
						} break;

					case PointerEventType.PointerMoved:
						{
							element.PointerMoved += TriggerEvent;
						} break;

					case PointerEventType.PointerPressed:
						{
							element.PointerPressed += TriggerEvent;
						} break;

					case PointerEventType.PointerReleased:
						{
							element.PointerReleased += TriggerEvent;
						} break;

					case PointerEventType.PointerWheelChanged:
						{
							element.PointerWheelChanged += TriggerEvent;
						} break;
				}
			}
		}

		/// <summary>
		/// Unsubscribes from the <paramref name="type"/> event on attached <see cref="UIElement"/>
		/// </summary>
		/// <param name="type"></param>
		private void Unsubcribe(PointerEventType type)
		{
			// Get the attached
			var element = AttachedTo as UIElement;

			// If it's not null (i.e. this event trigger is attached to something)
			if (element != null)
			{
				// Unsubscribe from an appropriate event
				switch (type)
				{
					case PointerEventType.PointerCanceled:
						{
							element.PointerCanceled -= TriggerEvent;
						} break;

					case PointerEventType.PointerCaptureLost:
						{
							element.PointerCaptureLost -= TriggerEvent;
						} break;

					case PointerEventType.PointerEntered:
						{
							element.PointerEntered -= TriggerEvent;
						} break;

					case PointerEventType.PointerExited:
						{
							element.PointerExited -= TriggerEvent;
						} break;

					case PointerEventType.PointerMoved:
						{
							element.PointerMoved -= TriggerEvent;
						} break;

					case PointerEventType.PointerPressed:
						{
							element.PointerPressed -= TriggerEvent;
						} break;

					case PointerEventType.PointerReleased:
						{
							element.PointerReleased -= TriggerEvent;
						} break;

					case PointerEventType.PointerWheelChanged:
						{
							element.PointerWheelChanged -= TriggerEvent;
						} break;
				}
			}
		}

		/// <summary>
		/// Returns true if the modifiers specified by <see cref="Modifiers"/> are found with appropriate test determined by
		/// <see cref="StrictModifierCheck"/> in the <paramref name="keyModifiers"/>
		/// </summary>
		/// <param name="keyModifiers">Modifiers to check in (the handled event modifiers)</param>
		/// <returns></returns>
		private bool ModifiersMatch(VirtualKeyModifiers keyModifiers)
		{
			// uint.Max means modifiers are not taken into account
			if((uint)Modifiers==uint.MaxValue)
			{
				return true;
			}

			if (StrictModifierCheck)
			{
				if (Modifiers != keyModifiers)
				{
					return false;
				}
			}
			else
			{
				if (!keyModifiers.HasFlag(Modifiers))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Method to subscribe with to Pointer Events on the attatched UIElement. It will check if modifiers match and, if so,
		/// raise the <see cref="Triggered"/> event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TriggerEvent(object sender, PointerRoutedEventArgs e)
		{
			if (ModifiersMatch(e.KeyModifiers))
			{
				Triggered?.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// On top of the standard functionality, it unsubscribes from the old <see cref="UIElement"/> and subscribes to
		/// the new one
		/// </summary>
		/// <param name="obj"></param>
		public override void Attach(DependencyObject obj)
		{
			if (AttachedTo != null)
			{
				Unsubcribe(PointerEvent);
			}

			// Only UIElements and above have pointer events
			if (obj is UIElement)
			{
				base.Attach(obj);

				if (AttachedTo != null)
				{
					Subcribe(PointerEvent);
				}
			}
			else
			{
				throw new ArgumentException("Pointer Events are introduced in " + nameof(UIElement) + "; " +
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
				Unsubcribe(PointerEvent);
			}

			base.Detach();
		}

		#endregion
	}	
}
