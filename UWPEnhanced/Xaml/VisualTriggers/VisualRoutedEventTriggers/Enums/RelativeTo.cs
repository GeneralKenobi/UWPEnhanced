namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Enumeration for visual trigger commands with command parameter equal to position of pointer relative to something
	/// </summary>
	public enum RelativeTo
	{
		/// <summary>
		/// Relative to the the command is attached to (if that object is a UIElement, otherwise null is passed which results
		/// in <see cref="Window"/>
		/// </summary>
		AttachedTo = 0,

		/// <summary>
		/// Relative to the overall window
		/// </summary>
		Window = 1,

		/// <summary>
		/// Relative to a UIElement bound to the RelativeToElement dependency property
		/// </summary>
		BoundObject	= 2,
	}
}