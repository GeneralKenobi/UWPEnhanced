using CSharpEnhanced.Maths;
using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	public partial class SumMEC : MarkupExtension
	{
		/// <summary> 
		/// If value is a numeral type (except <see cref="Complex"/>), it is cast to double and <see cref="_Summand"/> is added to it.
		/// Otherwise returns the value without changing it.
		/// </summary>
		private class SumConverter : IValueConverter
		{
			#region Constructors

			/// <summary>
			/// Default constructor
			/// </summary>
			public SumConverter(double summand)
			{
				_Summand = summand;
			}

			#endregion

			#region Private properties

			/// <summary>
			/// Number to add to the value
			/// </summary>
			private double _Summand { get; }

			#endregion

			#region Public methods

			/// <summary>			
			/// If value is a numeral type (except <see cref="Complex"/>), it is cast to double and <see cref="_Summand"/> is added to it.
			/// Otherwise returns the value without changing it.
			/// </summary>
			/// <param name="value"></param>
			/// <param name="targetType"></param>
			/// <param name="parameter"></param>
			/// <param name="language"></param>
			/// <returns></returns>
			public object Convert(object value, Type targetType, object parameter, string language)
			{
				if (MathsHelpers.TryCastToDouble(value, out var casted))
				{
					return casted + _Summand;
				}

				return value;
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