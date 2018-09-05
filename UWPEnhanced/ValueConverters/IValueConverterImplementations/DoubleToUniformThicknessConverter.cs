using CSharpEnhanced.Maths;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.ValueConverters
{
	/// <summary> 
	/// If value is a numeral type, it is cast to double and a new thickness with all entries marked in
	/// <see cref="SelectedSides"/> equal to the value (and all entries not marked in
	/// <see cref="SelectedSides"/> equal to 0) is returned. If value is not a numeral type, returns thickness with all
	/// entries equal to 0.
	/// </summary>
	public class DoubleToUniformThicknessConverter : DependencyObject, IValueConverter
	{
		#region SelectedSides Dependency Property

		/// <summary>
		/// Sides that whose values are returned (all others are set to 0)
		/// </summary>
		public Side SelectedSides
		{
			get => (Side)GetValue(SelectedSidesProperty);
			set => SetValue(SelectedSidesProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="SelectedSides"/>
		/// </summary>
		public static readonly DependencyProperty SelectedSidesProperty =
			DependencyProperty.Register(nameof(SelectedSides), typeof(Side),
			typeof(DoubleToUniformThicknessConverter), new PropertyMetadata(default(Side)));

		#endregion

		#region Protected methods

		/// <summary>
		/// Calculates and returns the final value from conversion
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		protected virtual Thickness ConvertValue(double value, Type targetType, object parameter, string language)
		{
			return new Thickness(
					SelectedSides.HasFlag(Side.Left) ? value : 0,
					SelectedSides.HasFlag(Side.Top) ? value : 0,
					SelectedSides.HasFlag(Side.Right) ? value : 0,
					SelectedSides.HasFlag(Side.Bottom) ? value : 0);
		}

		#endregion

		#region Public methods

		/// <summary> 
		/// If value is a numeral type, it is cast to double and a new thickness with all entries marked in
		/// <see cref="SelectedSides"/> equal to the valuet (and all entries not marked in
		/// <see cref="SelectedSides"/> equal to 0) is returned. If value is not a numeral type, returns thickness with all
		/// entries equal to 0.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (MathsHelpers.TryCastToDouble(value, out var d))
			{
				return ConvertValue(d, targetType, parameter, language);
			}

			return new Thickness();
		}

		/// <summary>
		/// Not implemented
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		#endregion
	}	
}