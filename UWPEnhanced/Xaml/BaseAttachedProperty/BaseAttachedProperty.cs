using System;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// A base attached property to replace the vanilla UWP attached property
	/// </summary>
	/// <typeparam name="Parent">The parent class to be the attached property</typeparam>
	/// <typeparam name="Property">The type of this attached property</typeparam>
	internal abstract class BaseAttachedProperty<Parent, Property>
		where Parent : new()
	{
		#region Public Events

		/// <summary>
		/// Fired when the value changes
		/// </summary>
		public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };

		/// <summary>
		/// Fired when the value changes, even when the value is the same
		/// </summary>
		public event Action<DependencyObject, object> ValueUpdated = (sender, value) => { };

		#endregion

		#region Private Properties

		public static object DefaultValue { get; set; } = default(Property);

		#endregion

		#region Public Properties

		/// <summary>
		/// A singleton instance of our parent class
		/// </summary>
		public static Parent Instance { get; private set; } = new Parent();

		#endregion

		#region Attached Property Definitions

		/// <summary>
		/// The attached property for this class
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.RegisterAttached(
				"Value",
				typeof(Property),
				typeof(BaseAttachedProperty<Parent, Property>),
				new PropertyMetadata(DefaultValue,
				new PropertyChangedCallback(OnValuePropertyChanged)));
				
		/// <summary>
		/// The callback event when the <see cref="ValueProperty"/> is changed
		/// </summary>
		/// <param name="d">The UI element that had it's property changed</param>
		/// <param name="e">The arguments for the event</param>
		private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// Call the parent function
			(Instance as BaseAttachedProperty<Parent, Property>)?.OnValueChanged(d, e);

			// Call event listeners
			(Instance as BaseAttachedProperty<Parent, Property>)?.ValueChanged(d, e);
		}

		/// <summary>
		/// Gets the attached property
		/// </summary>
		/// <param name="d">The element to get the property from</param>
		/// <returns></returns>
		public static Property GetValue(DependencyObject d) => (Property)d.GetValue(ValueProperty);

		/// <summary>
		/// Sets the attached property
		/// </summary>
		/// <param name="d">>The element to get the property from</param>
		/// <param name="value">The value to set the property to</param>
		public static void SetValue(DependencyObject d, Property value) => d.SetValue(ValueProperty, value);

		#endregion

		#region Event Methods

		/// <summary>
		/// The method that is called when any attached property of this type is changed
		/// </summary>
		/// <param name="sender">The UI element that this property was changed for</param>
		/// <param name="e">The arguments for this event</param>
		public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }
		
		#endregion
	}
}