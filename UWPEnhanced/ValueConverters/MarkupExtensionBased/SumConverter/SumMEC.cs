using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	/// <summary> 
	/// If value is a numeral type (except <see cref="Complex"/>), it is cast to double and <see cref="Summand"/> is added to it.
	/// Otherwise returns the value without changing it. By default <see cref="Summand"/> is 0.
	/// </summary>
	[MarkupExtensionReturnType(ReturnType = typeof(IValueConverter))]
	public partial class SumMEC : MarkupExtension
	{
		#region Public properties

		/// <summary>
		/// Number to add to the value. By default it's 0.
		/// </summary>
		public double Summand { get; set; }

		#endregion

		#region Protected methods

		/// <summary>
		/// Returns a new <see cref="SumConverter"/>
		/// </summary>
		/// <returns></returns>
		protected override object ProvideValue() => new SumConverter(Summand);

		#endregion
	}
}