using System;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	/// <summary>
	/// Tries to cast value to a nubmer and convert it to an SIString. Precision loss may occur if value is a
	/// <see cref="long"/> or an <see cref="ulong"/>. Conversion back is allowed.
	/// </summary>
	public partial class NumberToSIStringMEC : MarkupExtension
	{
		#region Public properties

		/// <summary>
		/// Unit to display
		/// </summary>
		public string Unit { get; set; } = string.Empty;

		/// <summary>
		/// Number of digits to round to
		/// </summary>
		public int RoundToDigit { get; set; }

		/// <summary>
		/// MidpointRoundting to use
		/// </summary>
		public MidpointRounding MidpointRounding { get; set; }

		/// <summary>
		/// Determines whether to use full prefix name
		/// </summary>
		public bool UseFullName { get; set; }

		/// <summary>
		/// Determines whether to exclude small prefixes. True by defualt
		/// </summary>
		public bool ExcludeSmallPrefixes { get; set; } = true;

		/// <summary>
		/// When converting back accepts u as µ. True by default
		/// </summary>
		public bool SimplifiedMicro { get; set; } = true;

		#endregion

		#region Protected methods

		/// <summary>
		/// Returns a new instance of <see cref="NumberToSIStringConverter"/>
		/// </summary>
		/// <returns></returns>
		protected override object ProvideValue() =>
			new NumberToSIStringConverter(Unit, RoundToDigit, MidpointRounding, UseFullName, ExcludeSmallPrefixes, SimplifiedMicro);

		#endregion
	}
}