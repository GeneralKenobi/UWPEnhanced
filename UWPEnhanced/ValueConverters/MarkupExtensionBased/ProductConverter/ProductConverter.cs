using CSharpEnhanced.Maths;
using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	public partial class ProductMEC : MarkupExtension
	{
		/// <summary> 
		/// If value is a numeral type (except <see cref="Complex"/>), it is to double and multiplied by <see cref="_Multiplier"/>.
		/// Otherwise returns the value without changing it.
		/// </summary>
		private class ProductConverter : IValueConverter
		{
			#region Constructors

			/// <summary>
			/// Default constructor
			/// </summary>
			public ProductConverter(double multiplier)
			{
				_Multiplier = multiplier;
			}

			#endregion

			#region Private properties

			/// <summary>
			/// Number to multiply the value by
			/// </summary>
			private double _Multiplier { get; }

			#endregion

			#region Public methods

			/// <summary>			
			/// If value is a numeral type (except <see cref="Complex"/>), it is to double and multiplied by <see cref="_Multiplier"/>.
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
					return casted * _Multiplier;
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