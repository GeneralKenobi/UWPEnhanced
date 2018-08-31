using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	/// <summary> 
	/// If value is a string and <see cref="string.IsNullOrWhiteSpace(string)"/> returns true, converter returns
	/// <see cref="Visibility.Collapsed"/>, if <see cref="string.IsNullOrWhiteSpace(string)"/> returns false, converter returns
	/// <see cref="Visibility.Visible"/>
	/// If <see cref="InvertConvesion"/> is true then swaps the conversion results.
	/// If value is not a <see cref="string"/> then always returns <see cref="Visibility.Visible"/>.
	/// </summary>
	[MarkupExtensionReturnType(ReturnType = typeof(IValueConverter))]
	public partial class StringToVisibilityMEC : MarkupExtension
	{
		#region Public properties

		/// <summary>
		/// If true, conversion is inverted
		/// </summary>
		public bool InvertConvesion { get; set; }

		#endregion

		#region Protected methods

		/// <summary>
		/// Returns a new <see cref="StringToVisibilityConverter"/>
		/// </summary>
		/// <returns></returns>
		protected override object ProvideValue() => new StringToVisibilityConverter(InvertConvesion);

		#endregion
	}
}