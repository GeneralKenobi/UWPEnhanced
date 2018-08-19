using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	/// <summary>
	/// Converts <see cref="true"/> to <see cref="ValueWhenTrue"/> and <see cref="false"/> to <see cref="ValueWhenFalse"/>.
	/// If value is not a <see cref="bool"/> returns <see cref="DefaultValue"/>
	/// </summary>
	public partial class BoolToParameterMEC : MarkupExtension
	{
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

		#region Protected methods

		/// <summary>
		/// Returns a new instance of converter
		/// </summary>
		/// <returns></returns>
		protected override object ProvideValue() => new BoolToParameterConverter(ValueWhenTrue, ValueWhenFalse, DefaultValue);

		#endregion]
	}
}