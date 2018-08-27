using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	/// <summary> 
	/// If value is a numeral type (except <see cref="Complex"/>), it is cast to double and multiplied by <see cref="Multiplier"/>.
	/// Otherwise returns the value without changing it. By default <see cref="Multiplier"/> is 1.
	/// </summary>
	[MarkupExtensionReturnType(ReturnType = typeof(IValueConverter))]
	public partial class ProductMEC : MarkupExtension
	{
		#region Public properties

		/// <summary>
		/// Number to multiply the value by. By default it's 1.
		/// </summary>
		public double Multiplier { get; set; } = 1;

		#endregion

		#region Protected methods

		/// <summary>
		/// Returns a new <see cref="ProductConverter"/>
		/// </summary>
		/// <returns></returns>
		protected override object ProvideValue() => new ProductConverter(Multiplier);

		#endregion
	}
}