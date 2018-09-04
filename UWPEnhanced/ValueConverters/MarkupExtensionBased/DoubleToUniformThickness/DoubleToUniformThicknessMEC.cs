using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	/// <summary> 
	/// If value is a numeral type, it is cast to double and a new thickness with all entries marked in
	/// <see cref="SelectedSides"/> equal to the value times <see cref="Multiplier"/> (and all entries not marked in
	/// <see cref="SelectedSides"/> equal to 0) is returned. If value is not a numeral type, returns thickness with all
	/// entries equal to 0.
	/// By default <see cref="Multiplier"/> is equal to 1.
	/// By default <see cref="SelectedSides"/> is equal to <see cref="Side.All"/>.
	/// </summary>
	[MarkupExtensionReturnType(ReturnType = typeof(IValueConverter))]
	public partial class DoubleToUniformThicknessMEC : MarkupExtension
	{
		#region Public properties

		/// <summary>
		/// Sides that whose values are returned (all others are set to 0). By default it's <see cref="Side.All"/>
		/// </summary>
		public Side SelectedSides { get; set; } = Side.All;

		/// <summary>
		/// Mutliples the final thickness by this value. By default it's 1.
		/// </summary>
		public double Multiplier { get; set; } = 1;

		#endregion

		#region Protected methods

		/// <summary>
		/// If value is a <see cref="Thickness"/>, returns a new thickness with all entries marked in <see cref="SelectedSides"/> equal to
		/// their respective entries in value and all entries not marked in <see cref="SelectedSides"/> equal to 0.
		/// If value is not a Thickenss, returns thickness with all entries equal to 0.
		/// </summary>
		/// <returns></returns>
		protected override object ProvideValue() => new DoubleToUniformThicknessConverter(SelectedSides, Multiplier);

		#endregion
	}
}