using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	public partial class StringToVisibilityMEC : MarkupExtension
	{
		/// If value is a string and <see cref="string.IsNullOrWhiteSpace(string)"/> returns true, converter returns
		/// <see cref="Visibility.Collapsed"/>, if <see cref="string.IsNullOrWhiteSpace(string)"/> returns false, converter returns
		/// <see cref="Visibility.Visible"/>
		/// If <see cref="InvertConvesion"/> is true then swaps the conversion results.
		/// If value is not a <see cref="string"/> then always returns <see cref="Visibility.Visible"/>.
		private class StringToVisibilityConverter : IValueConverter
		{
			#region Constructors

			/// <summary>
			/// Default constructor
			/// </summary>
			public StringToVisibilityConverter(bool invertConversion)
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

			/// If value is a string and <see cref="string.IsNullOrWhiteSpace(string)"/> returns true, converter returns
			/// <see cref="Visibility.Collapsed"/>, if <see cref="string.IsNullOrWhiteSpace(string)"/> returns false, converter returns
			/// <see cref="Visibility.Visible"/>
			/// If <see cref="InvertConvesion"/> is true then swaps the conversion results.
			/// If value is not a <see cref="string"/> then always returns <see cref="Visibility.Visible"/>.
			/// <param name="value"></param>
			/// <param name="targetType"></param>
			/// <param name="parameter"></param>
			/// <param name="language"></param>
			/// <returns></returns>
			public object Convert(object value, Type targetType, object parameter, string language)
			{
				if (value is string s)
				{
					return (string.IsNullOrWhiteSpace(s) ^ _InvertConversion) ? Visibility.Collapsed : Visibility.Visible;
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