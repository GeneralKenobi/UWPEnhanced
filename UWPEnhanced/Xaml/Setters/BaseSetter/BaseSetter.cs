using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Standard base class implementing <see cref="IVisualSetter"/>, provides casting value to expected type functionality
	/// </summary>
	[ContentProperty(Name = nameof(Value))]
	public abstract class BaseSetter : DependencyObject, IVisualSetter
	{
		#region Private Members

		/// <summary>
		/// Backing store for <see cref="ExpectedType"/>
		/// </summary>
		private Type _ExpectedType = null;

		#endregion

		#region Protected Properties

		/// <summary>
		/// Type of which <see cref="Value"/> is expected to be (should be assigned based on the target property by inheriting classes)
		/// </summary>
		protected Type ExpectedType
		{
			get => _ExpectedType;
			set
			{
				if (_ExpectedType != value)
				{
					_ExpectedType = value;
					// Call the method to try to cast the value to the new type
					ValueOrTargetTypeChanged();
				}
			}
		}
		
		#endregion

		#region Value Dependency Property

		/// <summary>
		/// Value to apply with the setter
		/// </summary>
		public object Value
		{
			get => GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Value"/>
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(nameof(Value), typeof(object),
			typeof(BaseSetter), new PropertyMetadata(default(object), ValueOrTargetTypePropertyChangedCallback));

		#endregion

		#region Private Static Methods	

		/// <summary>
		/// Callback for <see cref="Value"/> dependency property
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private static void ValueOrTargetTypePropertyChangedCallback(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			// If the sender is a BaseSetter and the new value is different
			if(s is BaseSetter setter && e.NewValue != e.OldValue)
			{
				setter.ValueOrTargetTypeChanged();
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// It will try to cast a generic string value (which is usually obtained from properties set in xaml) to the expected type.
		/// For example when the target property to set is Width (of type double) it is necessary to convert the string value to
		/// a number (double). Similiarly with enums.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="expectedType"></param>
		/// <returns></returns>
		private bool TryCastValueToExpectedType(string value, Type expectedType) =>
			// Start with enum because there's only one type check. The value will be assigned in the helpers
			TryToCastToEnum(value, expectedType) || TryToCastToNumeral(value, expectedType);

		/// <summary>
		/// Method which will call <see cref="TryCastValueToExpectedType(string, Type)"/>. Should be called 
		/// when <see cref="Value"/> or the expected type for value changes
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private void ValueOrTargetTypeChanged()
		{
			if(ExpectedType != null && Value != null && Value is string str)
			{
				TryCastValueToExpectedType(str, ExpectedType);
			}
		}

		/// <summary>
		/// Tries to cast the generic string value to Enum and assign it to <see cref="Value"/> on success.
		/// Helper of <see cref="TryCastValueToExpectedType(string, Type)"/>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="expectedType"></param>
		/// <returns></returns>
		private bool TryToCastToEnum(string value, Type expectedType)
		{
			// If the value actually is an enum
			if (expectedType.IsEnum)
			{
				// Try to parse it
				if (Enum.TryParse(expectedType, value, out var result))
				{
					Value = result;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Tries to cast the generic string value to a numeral (double) and assign it to <see cref="Value"/> on success.
		/// Helper of <see cref="TryCastValueToExpectedType(string, Type)"/>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="expectedType"></param>
		/// <returns></returns>
		private bool TryToCastToNumeral(string value, Type expectedType)
		{
			// Check if the type is actually some sort of a number
			if((expectedType == typeof(double) || expectedType==typeof(int) || expectedType == typeof(long) ||
				expectedType == typeof(float) || expectedType == typeof(short) || expectedType == typeof(byte)) &&
				// If so, try to parse it
				double.TryParse(value, out double result))
			{
				// If both conditions were met assign the result
				Value = result;
				return true;
			}
			else
			{
				return false;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Activates the setter (applies the value to the target)
		/// </summary>
		public abstract void Set();

		#endregion
	}
}