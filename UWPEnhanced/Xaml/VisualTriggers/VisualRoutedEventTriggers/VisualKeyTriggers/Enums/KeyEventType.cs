using System;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Types of key events
	/// </summary>
	[Flags]
	public enum KeyEventType
	{
		/// <summary>
		/// When key is pressed down
		/// </summary>
		KeyDown = 1,

		/// <summary>
		/// When key is released
		/// </summary>
		KeyUp = 2,
	}
}