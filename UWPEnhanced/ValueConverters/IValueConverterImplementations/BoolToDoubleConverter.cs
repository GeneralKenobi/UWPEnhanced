using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.ValueConverters
{
	/// <summary>
	/// Converts true to <see cref="ValueWhenTrue"/> and false to <see cref="ValueWhenTrue"/>
	/// </summary>
	public class BoolToDoubleConverter : DependencyObject, IValueConverter
	{
		#region ValueWhenFalse Dependency Property

		/// <summary>
		/// Returend when value is false
		/// </summary>
		public double ValueWhenFalse
		{
			get => (double)GetValue(ValueWhenFalseProperty);
			set => SetValue(ValueWhenFalseProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="ValueWhenFalse"/>
		/// </summary>
		public static readonly DependencyProperty ValueWhenFalseProperty =
			DependencyProperty.Register(nameof(ValueWhenFalse), typeof(double),
			typeof(BoolToDoubleConverter), new PropertyMetadata(default(double)));

		#endregion

		#region ValueWhenTrue Dependency Property

		/// <summary>
		/// Returned when value is true
		/// </summary>
		public double ValueWhenTrue
		{
			get => (double)GetValue(ValueWhenTrueProperty);
			set => SetValue(ValueWhenTrueProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="ValueWhenTrue"/>
		/// </summary>
		public static readonly DependencyProperty ValueWhenTrueProperty =
			DependencyProperty.Register(nameof(ValueWhenTrue), typeof(double),
			typeof(BoolToDoubleConverter), new PropertyMetadata(default(double)));

		#endregion

		#region IValueConverter

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is bool b && b)
			{
				return ValueWhenTrue;
			}
			else
			{
				return ValueWhenFalse;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		#endregion

	}
}
