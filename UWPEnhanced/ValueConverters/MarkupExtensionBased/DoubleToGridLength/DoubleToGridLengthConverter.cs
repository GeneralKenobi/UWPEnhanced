using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.ValueConverters
{
	partial class DoubleToGridLengthMEC
	{
		/// <summary> 
		/// Conversion depends on unit. If <see cref="_Unit"/> is <see cref="GridUnitType.Auto"/> then result is always
		/// <see cref="GridLength.Auto"/>. If unit is <see cref="GridUnitType.Pixel"/> or <see cref="GridUnitType.Star"/> and
		/// <paramref name="value"/> is a <see cref="double"/> then the result is a <see cref="GridLength"/> constructed from
		/// <paramref name="value"/> and <see cref="_Unit"/>. If neither of the cases occured then a 0 length <see cref="GridLength"/> is
		/// returned.
		/// </summary>
		private class DoubleToGridLengthConverter : IValueConverter
		{
			#region Constructors

			/// <summary>
			/// Default constructor
			/// </summary>
			public DoubleToGridLengthConverter(GridUnitType unit)
			{
				_Unit = unit;
			}

			#endregion

			#region Private properties

			/// <summary>
			/// Unit to which the value will be converter
			/// </summary>
			private GridUnitType _Unit { get; }

			#endregion

			#region Public methods

			/// <summary> 
			/// Conversion depends on unit. If <see cref="_Unit"/> is <see cref="GridUnitType.Auto"/> then result is always
			/// <see cref="GridLength.Auto"/>. If unit is <see cref="GridUnitType.Pixel"/> or <see cref="GridUnitType.Star"/> and
			/// <paramref name="value"/> is a <see cref="double"/> then the result is a <see cref="GridLength"/> constructed from
			/// <paramref name="value"/> and <see cref="_Unit"/>. If neither of the cases occured then a 0 length <see cref="GridLength"/> is
			/// returned.
			/// </summary>
			/// <param name="value"></param>
			/// <param name="targetType"></param>
			/// <param name="parameter"></param>
			/// <param name="language"></param>
			/// <returns></returns>
			public object Convert(object value, Type targetType, object parameter, string language)
			{
				switch (_Unit)
				{
					// For Auto unit always return auto
					case GridUnitType.Auto:
						{
							return GridLength.Auto;
						}

					// For Pixel and Star check if value is a double, if so construct a GridLength
					case GridUnitType.Pixel:
					case GridUnitType.Star:
						{
							if (value is double d)
							{
								return new GridLength(d, _Unit);
							}
						} break;
				}
				
				// If no case was applicable return a 0 GridLength
				return new GridLength();
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