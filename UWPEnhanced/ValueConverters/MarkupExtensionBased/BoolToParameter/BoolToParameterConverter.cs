using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	public partial class BoolToParameterMEC : MarkupExtension
	{
		/// Converts <see cref="true"/> to <see cref="ValueWhenTrue"/> and <see cref="false"/> to <see cref="ValueWhenFalse"/>.
		/// If value is not a <see cref="bool"/> returns <see cref="DefaultValue"/>
		private class BoolToParameterConverter : IValueConverter
		{
			#region Constructors

			/// <summary>
			/// Default constructor
			/// </summary>
			/// <param name="valueWhenTrue"></param>
			/// <param name="valueWhenFalse"></param>
			/// <param name="defaultValue"></param>
			public BoolToParameterConverter(object valueWhenTrue, object valueWhenFalse, object defaultValue)
			{
				ValueWhenTrue = valueWhenTrue;
				ValueWhenFalse = valueWhenFalse;
				DefaultValue = defaultValue;
			}

			#endregion

			#region Public properties

			/// <summary>
			/// Return when converter value is <see cref="true"/>
			/// </summary>
			public object ValueWhenTrue { get; set; }

			/// <summary>
			/// Returned when value is <see cref="false"/>
			/// </summary>
			public object ValueWhenFalse { get; set; }

			/// <summary>
			/// Returned if value is not a <see cref="bool"/>
			/// </summary>
			public object DefaultValue { get; set; }

			#endregion

			#region Public methods

			/// <summary>
			/// Converts <see cref="true"/> to <see cref="ValueWhenTrue"/> and <see cref="false"/> to <see cref="ValueWhenFalse"/>.
			/// If value is not a <see cref="bool"/> returns <see cref="DefaultValue"/>
			/// </summary>
			/// <param name="value"></param>
			/// <param name="targetType"></param>
			/// <param name="parameter"></param>
			/// <param name="language"></param>
			/// <returns></returns>
			public object Convert(object value, Type targetType, object parameter, string language)
			{
				if(value is bool b)
				{
					return b ? ValueWhenTrue : ValueWhenFalse;
				}

				return DefaultValue;
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