﻿using CSharpEnhanced.Helpers;
using CSharpEnhanced.Maths;
using System;
using System.Numerics;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	public partial class NumberToSIStringMEC : MarkupExtension
	{
		/// <summary>
		/// <see cref="IValueConverter"/> to return that will handle conversion. Precision loss may occur if value is a
		/// <see cref="long"/> or an <see cref="ulong"/>
		/// </summary>
		private class NumberToSIStringConverter : IValueConverter
		{
			#region Constructors

			/// <summary>
			/// Default constructor
			/// </summary>
			/// <param name="unit"></param>
			/// <param name="roundToDigit"></param>
			/// <param name="midpointRounding"></param>
			/// <param name="useFullName"></param>
			/// <param name="excludeSmallPrefixes"></param>
			/// <param name="simplifiedMicro"></param>
			public NumberToSIStringConverter(string unit, int roundToDigit, MidpointRounding midpointRounding, bool useFullName,
				bool excludeSmallPrefixes, bool simplifiedMicro)
			{
				_Unit = unit;
				_RoundToDigit = roundToDigit;
				_MidpointRounding = midpointRounding;
				_UseFullName = useFullName;
				_ExcludeSmallPrefixes = excludeSmallPrefixes;
				_SimplifiedMicro = simplifiedMicro;
			}

			#endregion

			#region Private properties

			/// <summary>
			/// Unit to display
			/// </summary>
			private string _Unit { get; }

			/// <summary>
			/// Number of digits to round to
			/// </summary>
			private int _RoundToDigit { get; }

			/// <summary>
			/// MidpointRoundting to use
			/// </summary>
			private MidpointRounding _MidpointRounding { get; }

			/// <summary>
			/// Determines whether to use full prefix name
			/// </summary>
			private bool _UseFullName { get; }

			/// <summary>
			/// Determines whether to exclude small prefixes
			/// </summary>
			private bool _ExcludeSmallPrefixes { get; }

			/// <summary>
			/// When converting back accepts u as µ
			/// </summary>
			private bool _SimplifiedMicro { get; }

			#endregion

			#region Public methods

			/// <summary>
			/// Converts a number to SIString using <see cref="CSharpEnhanced.Helpers.SIHelpers.ToS"/>
			/// </summary>
			/// <param name="value"></param>
			/// <param name="targetType"></param>
			/// <param name="parameter"></param>
			/// <param name="language"></param>
			/// <returns></returns>
			public object Convert(object value, Type targetType, object parameter, string language)
			{
				if (MathsHelpers.TryCastToDouble(value, out var result))
				{
					return _ExcludeSmallPrefixes ?
						SIHelpers.ToSIStringExcludingSmallPrefixes(result, _Unit, _RoundToDigit, _MidpointRounding, _UseFullName) :
						SIHelpers.ToSIString(result, _Unit, _RoundToDigit, _MidpointRounding, _UseFullName);
				}

				if(value is Complex c)
				{
					return _ExcludeSmallPrefixes ?
						SIHelpers.ToAltSIStringExcludingSmallPrefixes(c, _Unit, _RoundToDigit, _MidpointRounding, _UseFullName) :
						SIHelpers.ToAltSIString(c, _Unit, _RoundToDigit, _MidpointRounding, _UseFullName);
				}

				return 0;
			}

			/// <summary>
			/// If value is a string, tries to parse it and return the resulting value. If not successful returns 0.
			/// </summary>
			/// <param name="value"></param>
			/// <param name="targetType"></param>
			/// <param name="parameter"></param>
			/// <param name="language"></param>
			/// <returns></returns>
			public object ConvertBack(object value, Type targetType, object parameter, string language)
			{
				if(value is string s && SIHelpers.TryParseSIString(s, out var result, _SimplifiedMicro))
				{
					return result;
				}

				return 0;
			}

			#endregion
		}
	}
}