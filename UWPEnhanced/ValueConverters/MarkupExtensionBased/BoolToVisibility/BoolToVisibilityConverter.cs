using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	public partial class BoolToVisibilityMEC : MarkupExtension
	{
		/// <summary> 
		/// Converts true to <see cref="Visibility.Visible"/> and false to <see cref="Visibility.Collapsed"/>.
		/// If <see cref="InvertConvesion"/> is true then swaps the conversion results.
		/// If value is not a <see cref="bool"/> then always returns <see cref="Visibility.Visible"/>.
		/// </summary>
		private class BoolToVisibilityConverter : IValueConverter
		{
			#region Constructors

			/// <summary>
			/// Default constructor
			/// </summary>
			public BoolToVisibilityConverter(bool invertConversion)
			{
				_InvertConversion = invertConversion;
			}

			#endregion

			#region Private properties

			/// <summary>
			/// True if the conversion is to be inverted (<see cref="Visibility.Visible"/> returned for false)
			/// </summary>
			private bool _InvertConversion { get; }

			#endregion

			#region Public methods

			/// <summary> 
			/// Converts true to <see cref="Visibility.Visible"/> and false to <see cref="Visibility.Collapsed"/>.
			/// If <see cref="InvertConvesion"/> is true then swaps the conversion results.
			/// If value is not a <see cref="bool"/> then always returns <see cref="Visibility.Visible"/>.
			/// </summary>
			/// <param name="value"></param>
			/// <param name="targetType"></param>
			/// <param name="parameter"></param>
			/// <param name="language"></param>
			/// <returns></returns>
			public object Convert(object value, Type targetType, object parameter, string language)
			{
				if (value is bool b)
				{
					return (b ^ _InvertConversion) ? Visibility.Visible : Visibility.Collapsed;
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