using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	/// <summary> 
	/// Conversion depends on unit. If <see cref="Unit"/> is <see cref="GridUnitType.Auto"/> then result is always
	/// <see cref="GridLength.Auto"/>. If unit is <see cref="GridUnitType.Pixel"/> or <see cref="GridUnitType.Star"/> and
	/// <paramref name="value"/> is a <see cref="double"/> then the result is a <see cref="GridLength"/> constructed from
	/// <paramref name="value"/> and <see cref="Unit"/>. If neither of the cases occured then a 0 length <see cref="GridLength"/> is
	/// returned.
	/// </summary>
	public partial class DoubleToGridLengthMEC : MarkupExtension
	{
		#region Public properties

		/// <summary>
		/// Determines unit used when constructing <see cref="GridLength"/>
		/// </summary>
		public GridUnitType Unit { get; set; }

		#endregion

		#region Protected methods

		/// <summary>
		/// Returns a new <see cref="DoubleToGridLengthConverter"/>
		/// </summary>
		/// <returns></returns>
		protected override object ProvideValue() => new DoubleToGridLengthConverter(Unit);

		#endregion
	}
}