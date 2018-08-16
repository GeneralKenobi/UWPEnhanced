using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Provides means of notifying whenever key down routed events is raised
	/// </summary>
	public class VisualKeyTrigger : BaseVisualKeyTrigger
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
		
		#endregion

		#region Protected Methods

		/// <summary>
		/// Returns true if the key 
		/// </summary>
		/// <returns></returns>
		protected override bool TriggerConditionsMet(KeyRoutedEventArgs e) => KeyMatches(e.Key);

		#endregion
	}
}