using CSharpEnhanced.Helpers;
using CSharpEnhanced.Maths;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Control displaying values with appropriate SI prefixes
	/// </summary>
	public sealed class ValueUnitDisplay : Control, INotifyPropertyChanged
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public ValueUnitDisplay()
		{
			this.DefaultStyleKey = typeof(ValueUnitDisplay);
		}

		#endregion

		#region Events

		/// <summary>
		/// Event fired whenever a property changes its value
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Value Dependency Property

		/// <summary>
		/// The value to display
		/// </summary>
		public double Value
		{
			get => (double)GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Value"/>
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(nameof(Value), typeof(double),
			typeof(ValueUnitDisplay), new PropertyMetadata(default(double), new PropertyChangedCallback(NotifyDisplayTextChanged)));

		#endregion

		#region RoundToDigit Dependency Property

		/// <summary>
		/// Rounds the value to obtain a number of digits equal to this, negative values are changed to 0
		/// </summary>
		public int RoundToDigit
		{
			get => (int)GetValue(RoundToDigitProperty);
			set => SetValue(RoundToDigitProperty, value >= 0 ? value : 0);
		}

		/// <summary>
		/// Backing store for <see cref="RoundToDigit"/>
		/// </summary>
		public static readonly DependencyProperty RoundToDigitProperty =
			DependencyProperty.Register(nameof(RoundToDigit), typeof(int),
			typeof(ValueUnitDisplay), new PropertyMetadata(default(int), new PropertyChangedCallback(NotifyDisplayTextChanged)));

		#endregion

		#region Unit Dependency Property

		/// <summary>
		/// The unit to display with the value
		/// </summary>
		public string Unit
		{
			get => (string)GetValue(UnitProperty);
			set => SetValue(UnitProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Unit"/>
		/// </summary>
		public static readonly DependencyProperty UnitProperty =
			DependencyProperty.Register(nameof(Unit), typeof(string),
			typeof(ValueUnitDisplay), new PropertyMetadata(string.Empty, new PropertyChangedCallback(NotifyDisplayTextChanged)));

		#endregion

		#region UseFullPrefixName Dependency Property

		/// <summary>
		/// If true, a full name is used in the prefix
		/// </summary>
		public bool UseFullPrefixName
		{
			get => (bool)GetValue(UseFullPrefixNameProperty);
			set => SetValue(UseFullPrefixNameProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="UseFullPrefixName"/>
		/// </summary>
		public static readonly DependencyProperty UseFullPrefixNameProperty =
			DependencyProperty.Register(nameof(UseFullPrefixName), typeof(bool),
			typeof(ValueUnitDisplay), new PropertyMetadata(default(bool), new PropertyChangedCallback(NotifyDisplayTextChanged)));

		#endregion

		#region PutSpaceBeforeUnit Dependency Property

		/// <summary>
		/// If true, a space is put between the value and the prefix with unit
		/// </summary>
		public bool PutSpaceBeforeUnit
		{
			get => (bool)GetValue(PutSpaceBeforeUnitProperty);
			set => SetValue(PutSpaceBeforeUnitProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="PutSpaceBeforeUnit"/>
		/// </summary>
		public static readonly DependencyProperty PutSpaceBeforeUnitProperty =
			DependencyProperty.Register(nameof(PutSpaceBeforeUnit), typeof(bool),
			typeof(ValueUnitDisplay), new PropertyMetadata(default(bool), new PropertyChangedCallback(NotifyDisplayTextChanged)));

		#endregion

		#region Private static methods

		/// <summary>
		/// Property changed callback for dependency properties that determine the value of <see cref="DisplayText"/>
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void NotifyDisplayTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
			(d as ValueUnitDisplay)?.PropertyChanged?.Invoke(d, new PropertyChangedEventArgs(nameof(DisplayText)));

		#endregion

		#region Public properties

		/// <summary>
		/// Text to display (formatted accordingly to <see cref="Value"/>, <see cref="RoundToDigit"/>, <see cref="Unit"/> and
		/// <see cref="UseFullPrefixName"/>
		/// </summary>
		public string DisplayText
		{
			get
			{
				// Get the prefix
				var prefix = SIHelpers.GetClosestPrefixExcludingSmall(Value);

				// Divide the value by the multiplier of the prefix
				var roundedValue = (Value / Math.Pow(10, prefix.Base10Power)).RoundToDigit(RoundToDigit);

				// Return a string consisting of value, optional space, the name or the symbol of the prefix and the unit
				return roundedValue.ToString() + (PutSpaceBeforeUnit ? " " : string.Empty) +
					(UseFullPrefixName ? prefix.Name : prefix.Symbol) + Unit;
			}
		}

		#endregion
	}
}