using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Provides means of notifying whenever key down routed events is raised for a single, specific <see cref="VirtualKey"/>
	/// </summary>
	public class VisualSpecificKeyTrigger : BaseVisualKeyTrigger
	{
		#region Key Dependency Property

		/// <summary>
		/// The key for which trigger occurs
		/// </summary>
		public VirtualKey Key
		{
			get => (VirtualKey)GetValue(KeyProperty);
			set => SetValue(KeyProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Key"/>
		/// </summary>
		public static readonly DependencyProperty KeyProperty =
			DependencyProperty.Register(nameof(Key), typeof(VirtualKey),
			typeof(VisualSpecificKeyTrigger), new PropertyMetadata(default(VirtualKey)));

		#endregion

		#region Protected Methods

		/// <summary>
		/// Returns true if the key 
		/// </summary>
		/// <returns></returns>
		protected override bool TriggerConditionsMet(KeyRoutedEventArgs e) => e.Key == Key;

		#endregion
	}
}