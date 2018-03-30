namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Interface for temporary setters. Temporary setters remember the old value and allow the owner to restore it.
	/// For example a temporary setter for width before setting its 300d value will first remember the old value (ex. 100d).
	/// Then when <see cref="Reset"/> is called it will set the width to 100d
	/// </summary>
	public interface ITemporaryVisualSetter : IVisualSetter
	{
		/// <summary>
		/// The old value remembered by the setter
		/// </summary>
		object OldValue { get; }

		/// <summary>
		/// Restores the old value on the property
		/// </summary>
		void Reset();
	}
}