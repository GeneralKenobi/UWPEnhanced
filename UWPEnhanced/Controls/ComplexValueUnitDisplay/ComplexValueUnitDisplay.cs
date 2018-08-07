using CSharpEnhanced.Helpers;
using CSharpEnhanced.Maths;
using System;
using System.ComponentModel;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Control displaying values with appropriate SI prefixes
	/// </summary>
	public sealed class ComplexValueUnitDisplay : Control, INotifyPropertyChanged
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public ComplexValueUnitDisplay()
		{
			this.DefaultStyleKey = typeof(ComplexValueUnitDisplay);
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
		public Complex Value
		{
			get => (Complex)GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Value"/>
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(nameof(Value), typeof(Complex),
			typeof(ComplexValueUnitDisplay), new PropertyMetadata(default(Complex), new PropertyChangedCallback(NotifyDisplayTextChanged)));

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
			typeof(ComplexValueUnitDisplay), new PropertyMetadata(default(int), new PropertyChangedCallback(NotifyDisplayTextChanged)));

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
			typeof(ComplexValueUnitDisplay), new PropertyMetadata(string.Empty, new PropertyChangedCallback(NotifyDisplayTextChanged)));

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
			typeof(ComplexValueUnitDisplay), new PropertyMetadata(default(bool), new PropertyChangedCallback(NotifyDisplayTextChanged)));

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
			typeof(ComplexValueUnitDisplay), new PropertyMetadata(default(bool), new PropertyChangedCallback(NotifyDisplayTextChanged)));

		#endregion

		#region UseJAsImaginaryUnit Dependency Property

		/// <summary>
		/// If true, "j" is used as the imaginary unit
		/// </summary>
		public bool UseJAsImaginaryUnit
		{
			get => (bool)GetValue(UseJAsImaginaryUnitProperty);
			set => SetValue(UseJAsImaginaryUnitProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="UseJAsImaginaryUnit"/>
		/// </summary>
		public static readonly DependencyProperty UseJAsImaginaryUnitProperty =
			DependencyProperty.Register(nameof(UseJAsImaginaryUnit), typeof(bool),
			typeof(ComplexValueUnitDisplay), new PropertyMetadata(default(bool)));

		#endregion

		#region Private static methods

		/// <summary>
		/// Property changed callback for dependency properties that determine the value of <see cref="DisplayText"/>
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void NotifyDisplayTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
			(d as ComplexValueUnitDisplay)?.PropertyChanged?.Invoke(d, new PropertyChangedEventArgs(nameof(DisplayText)));

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
				
				// Get the SI string
				var result =  SIHelpers.ToAltSIStringExcludingSmallPrefixes(roundedValue, Unit, imaginaryAsJ: UseJAsImaginaryUnit);

				// If there is supposed to be a space before the unit
				if(PutSpaceBeforeUnit)
				{
					// If the value is equal to 0 the result will be in the form of 0*unit* so insert the space at index 1
					if(roundedValue == 0)
					{
						result = result.Insert(1, " ");
					}
					// Otherwise the result will be in the form (*some number*)*unit* so insert the space 1 index after the ")"
					else
					{
						result = result.Insert(result.IndexOf(")") + 1, " ");
					}
				}

				return result;
			}
		}

		#endregion
	}
}