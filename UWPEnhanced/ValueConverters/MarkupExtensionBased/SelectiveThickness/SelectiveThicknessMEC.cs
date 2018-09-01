using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.ValueConverters
{
	/// <summary> 
	/// If value is a <see cref="Thickness"/>, returns a new thickness with all entries marked in <see cref="SelectedSides"/> equal to
	/// their respective entries in value and all entries not marked in <see cref="SelectedSides"/> equal to 0.
	/// If value is not a Thickenss, returns thickness with all entries equal to 0.
	/// </summary>
	[MarkupExtensionReturnType(ReturnType = typeof(IValueConverter))]
	public partial class SelectiveThicknessMEC : MarkupExtension
	{
		#region Public properties

		/// <summary>
		/// Sides that whose values are returned (all others are set to 0)
		/// </summary>
		Side SelectedSides { get; set; }

		#endregion

		#region Protected methods

		/// <summary>
		/// If value is a <see cref="Thickness"/>, returns a new thickness with all entries marked in <see cref="SelectedSides"/> equal to
		/// their respective entries in value and all entries not marked in <see cref="SelectedSides"/> equal to 0.
		/// If value is not a Thickenss, returns thickness with all entries equal to 0.
		/// </summary>
		/// <returns></returns>
		protected override object ProvideValue() => new SelectiveThicknessConverter(SelectedSides);

		#endregion
	}
}