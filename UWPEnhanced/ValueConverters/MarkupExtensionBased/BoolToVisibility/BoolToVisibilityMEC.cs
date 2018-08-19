using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	/// <summary> 
	/// Converts true to <see cref="Visibility.Visible"/> and false to <see cref="Visibility.Collapsed"/>.
	/// If <see cref="InvertConvesion"/> is true then swaps the conversion results.
	/// If value is not a <see cref="bool"/> then always returns <see cref="Visibility.Visible"/>.
	/// </summary>
	[MarkupExtensionReturnType(ReturnType = typeof(IValueConverter))]
	public partial class BoolToVisibilityMEC : MarkupExtension
	{
		#region Public properties

		/// <summary>
		/// If true, conversion is inverted
		/// </summary>
		public bool InvertConvesion { get; set; }

		#endregion

		#region Protected methods

		/// <summary>
		/// Returns a new <see cref="BoolToVisibilityConverter"/>
		/// </summary>
		/// <returns></returns>
		protected override object ProvideValue() => new BoolToVisibilityConverter(InvertConvesion);

		#endregion
	}
}