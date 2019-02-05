using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	public partial class DoubleToVisibilityMEC : MarkupExtension
	{
		/// <summary> 
		/// Returns <see cref="Visibility.Visible"/> if value equals <see cref="TargetNumber"/>.
		/// If <see cref="InvertConvesion"/> is true then swaps the conversion results.
		/// If value is not a <see cref="double"/> then always returns <see cref="Visibility.Visible"/>.
		/// </summary>
		private class DoubleToVisibilityConverter : IValueConverter
		{
			#region Constructors

			/// <summary>
			/// Default constructor
			/// </summary>
			public DoubleToVisibilityConverter(double targetNumber, bool invertConversion)
			{
				_InvertConversion = invertConversion;
			}

			#endregion

			#region Private properties

			/// <summary>
			/// True if the conversion is to be inverted (<see cref="Visibility.Visible"/> returned for false)
			/// </summary>
			private bool _InvertConversion { get; }

			/// <summary>
			/// Number that results in positive conversion
			/// </summary>
			private double _TargetNumber { get; }

			#endregion

			#region Public methods

			/// <summary> 
			/// Returns <see cref="Visibility.Visible"/> if value equals <see cref="TargetNumber"/>.			
			/// If <see cref="InvertConvesion"/> is true then swaps the conversion results.
			/// If value is not a <see cref="double"/> then always returns <see cref="Visibility.Visible"/>.
			/// </summary>
			/// <param name="value"></param>
			/// <param name="targetType"></param>
			/// <param name="parameter"></param>
			/// <param name="language"></param>
			/// <returns></returns>
			public object Convert(object value, Type targetType, object parameter, string language)
			{
				if (value is double d)
				{
					return ((d == _TargetNumber) ^ _InvertConversion) ? Visibility.Visible : Visibility.Collapsed;
				}

				return Visibility.Visible;
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