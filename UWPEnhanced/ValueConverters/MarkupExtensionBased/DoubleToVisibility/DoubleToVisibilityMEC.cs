using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	/// <summary> 
	/// Returns <see cref="Visibility.Visible"/> if value equals <see cref="TargetNumber"/>.
	/// If <see cref="InvertConvesion"/> is true then swaps the conversion results.
	/// If value is not a <see cref="double"/> then always returns <see cref="Visibility.Visible"/>.
	/// </summary>
	[MarkupExtensionReturnType(ReturnType = typeof(IValueConverter))]
	public partial class DoubleToVisibilityMEC : MarkupExtension
	{
		#region Public properties

		/// <summary>
		/// If true, conversion is inverted
		/// </summary>
		public bool InvertConvesion { get; set; }

		/// <summary>
		/// Number that results in positive conversion
		/// </summary>
		public double TargetNumber { get; set; }

		#endregion

		#region Protected methods

		/// <summary>
		/// Returns a new <see cref="DoubleToVisibilityConverter"/>
		/// </summary>
		/// <returns></returns>
		protected override object ProvideValue() => new DoubleToVisibilityConverter(TargetNumber, InvertConvesion);

		#endregion
	}
}