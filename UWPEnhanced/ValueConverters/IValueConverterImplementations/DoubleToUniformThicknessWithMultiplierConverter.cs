using System;
using Windows.UI.Xaml;

namespace UWPEnhanced.ValueConverters
{
	/// <summary> 
	/// If value is a numeral type, it is cast to double and a new thickness with all entries marked in
	/// <see cref="SelectedSides"/> equal to the value times <see cref="Multiplier"/> (and all entries not marked in
	/// <see cref="SelectedSides"/> equal to 0) is returned. If value is not a numeral type, returns thickness with all
	/// entries equal to 0.
	/// </summary>
	public class DoubleToUniformThicknessWithMultiplierConverter : DoubleToUniformThicknessConverter
	{
		#region Multiplier Dependency Property

		/// <summary>
		/// Value is multiplied by this property during conversion
		/// </summary>
		public double Multiplier
		{
			get => (double)GetValue(MultiplierProperty);
			set => SetValue(MultiplierProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Multiplier"/>
		/// </summary>
		public static readonly DependencyProperty MultiplierProperty =
			DependencyProperty.Register(nameof(Multiplier), typeof(double),
			typeof(DoubleToUniformThicknessWithMultiplierConverter), new PropertyMetadata(default(double)));

		#endregion

		#region Protected methods

		/// <summary>
		/// Returns the thickness calculated by the base class and multiplies it by multipler
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		protected override Thickness ConvertValue(double value, Type targetType, object parameter, string language)
		{
			var baseResult = base.ConvertValue(value, targetType, parameter, language);

			return new Thickness(baseResult.Left * Multiplier, baseResult.Top * Multiplier, baseResult.Right * Multiplier,
				baseResult.Bottom * Multiplier);
		}

		#endregion
	}
}