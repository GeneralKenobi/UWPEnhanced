namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Interface for custom setters
	/// </summary>
	interface IVisualSetter
	{
		/// <summary>
		/// Activates the setter (applies the value to the target)
		/// </summary>
		void Set();
	}
}