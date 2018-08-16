using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Provides means of notifying whenever key down routed events is raised
	/// </summary>
	public class VisualKeyTrigger : VisualRoutedEventTriggerWithModifiers<KeyRoutedEventArgs>
	{
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public VisualKeyTrigger()
		{
			Whitelist = new HashSet<VirtualKey>();
			Blacklist = new HashSet<VirtualKey>();
		}

		#endregion

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
			typeof(VisualKeyTrigger), new PropertyMetadata(KeyEventType.KeyDown, new PropertyChangedCallback(EventTypeChanged)));

		#endregion

		#region Whitelist Dependency Property

		/// <summary>
		/// Only fires for <see cref="VirtualKey"/>s in this collection. If it's empty it will fire for every <see cref="VirtualKey"/>.
		/// <see cref="Blacklist"/> takes priority over this collection so any <see cref="VirtualKey"/> that is in both collections
		/// won't fire the trigger.
		/// </summary>
		public HashSet<VirtualKey> Whitelist
		{
			get => (HashSet<VirtualKey>)GetValue(WhitelistProperty);
			set => SetValue(WhitelistProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Whitelist"/>
		/// </summary>
		public static readonly DependencyProperty WhitelistProperty =
			DependencyProperty.Register(nameof(Whitelist), typeof(HashSet<VirtualKey>),
			typeof(VisualKeyTrigger), new PropertyMetadata(default(HashSet<VirtualKey>)));

		#endregion

		#region Blacklist Dependency Property

		/// <summary>
		/// If a <see cref="VirtualKey"/> is in this collection, the trigger won't fire for it. This collection takes priority over
		/// <see cref="Whitelist"/> so any <see cref="VirtualKey"/> that is in both collections won't fire the trigger.
		/// </summary>
		public HashSet<VirtualKey> Blacklist
		{
			get => (HashSet<VirtualKey>)GetValue(BlacklistProperty);
			set => SetValue(BlacklistProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Blacklist"/>
		/// </summary>
		public static readonly DependencyProperty BlacklistProperty =
			DependencyProperty.Register(nameof(Blacklist), typeof(HashSet<VirtualKey>),
			typeof(VisualKeyTrigger), new PropertyMetadata(default(HashSet<VirtualKey>)));

		#endregion

		#region BlacklistModifierKeys Dependency Property

		/// <summary>
		/// If true all presses of modifier keys (Shift, Control, Menu and Windows) are ignored
		/// </summary>
		public bool BlacklistModifierKeys
		{
			get => (bool)GetValue(BlacklistModifierKeysProperty);
			set => SetValue(BlacklistModifierKeysProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="BlacklistModifierKeys"/>
		/// </summary>
		public static readonly DependencyProperty BlacklistModifierKeysProperty =
			DependencyProperty.Register(nameof(BlacklistModifierKeys), typeof(bool),
			typeof(VisualKeyTrigger), new PropertyMetadata(false));

		#endregion

		#region Private methods

		/// <summary>
		/// Returns true if <see cref="KeyRoutedEventArgs.Key"/> is contained in the <see cref="Whitelist"/> (or the whitelist is empty)
		/// and the key is not in the <see cref="Blacklist"/>
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private bool KeyMatches(VirtualKey key) => !Blacklist.Contains(key) && (Whitelist.Count == 0 || Whitelist.Contains(key)) &&
			// If blacklisting modifier keys is on, the bracket will be evaluated. If any of the virtual keys is pressed, the bracket
			// will evaluate to true, which will be negated and the part below this comment will be false thus returning false from
			// the whole method
			(!BlacklistModifierKeys || !(key == VirtualKey.Shift || key == VirtualKey.Control || key == VirtualKey.Menu ||
			key == VirtualKey.LeftWindows || key == VirtualKey.RightWindows));

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

		/// <summary>
		/// Returns true if the key 
		/// </summary>
		/// <returns></returns>
		protected override bool TriggerConditionsMet(KeyRoutedEventArgs e) => KeyMatches(e.Key);

		#endregion

		#region Private static methods

		/// <summary>
		/// Method called when <see cref="EventType"/> property changes; Unsubscribes from the old event and subscribes to a new one
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void EventTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is VisualKeyTrigger trigger)
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