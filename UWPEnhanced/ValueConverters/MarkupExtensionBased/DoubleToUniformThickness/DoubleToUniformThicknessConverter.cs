using CSharpEnhanced.Maths;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	public partial class DoubleToUniformThicknessMEC : MarkupExtension
	{
		/// <summary> 
		/// If value is a numeral type, it is cast to double and a new thickness with all entries marked in
		/// <see cref="SelectedSides"/> equal to the value times multiplier (and all entries not marked in
		/// <see cref="SelectedSides"/> equal to 0) is returned. If value is not a numeral type, returns thickness with all
		/// entries equal to 0.
		/// </summary>
		private class DoubleToUniformThicknessConverter : IValueConverter
		{
			#region Constructors

			/// <summary>
			/// Default constructor
			/// </summary>
			public DoubleToUniformThicknessConverter(Side selectedSides, double multiplier)
			{
				_SelectedSides = selectedSides;
				_Multiplier = multiplier;
			}

			#endregion

			#region Private properties

			/// <summary>
			/// Sides that whose values are returned (all others are set to 0)
			/// </summary>
			private Side _SelectedSides { get; }

			/// <summary>
			/// Mutliples the final thickness by this value
			/// </summary>
			private double _Multiplier { get; }

			#endregion

			#region Public methods

			/// <summary> 
			/// If value is a numeral type, it is cast to double and a new thickness with all entries marked in
			/// <see cref="SelectedSides"/> equal to the valuet times multiplier (and all entries not marked in
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
					return new Thickness(
						_SelectedSides.HasFlag(Side.Left) ? _Multiplier * d : 0,
						_SelectedSides.HasFlag(Side.Top) ? _Multiplier * d : 0,
						_SelectedSides.HasFlag(Side.Right) ? _Multiplier * d : 0,
						_SelectedSides.HasFlag(Side.Bottom) ? _Multiplier * d : 0);
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
}