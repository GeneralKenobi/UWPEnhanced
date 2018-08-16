using Windows.System;
using Windows.UI.Core;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Class containing helper methods related to <see cref="VirtualKeyModifiers"/>
	/// </summary>
	public static class ModifiersHelpers
	{
		#region Public static methods

		/// <summary>
		/// Returns <see cref="VirtualKeyModifiers"/> active at the moment of the call (including the Windows key)
		/// </summary>
		/// <returns></returns>
		public static VirtualKeyModifiers GetCurrentModifiers()
		{
			var window = CoreWindow.GetForCurrentThread();

			var standard = GetCurrentModifiersWithoutWindows();

			// Because windows key is not given as a unified key check both left and right
			var windows = (window.GetKeyState(VirtualKey.LeftWindows).HasFlag(CoreVirtualKeyStates.Down) ||
				window.GetKeyState(VirtualKey.LeftWindows).HasFlag(CoreVirtualKeyStates.Down)) ?
				VirtualKeyModifiers.Menu : VirtualKeyModifiers.None;

			return standard | windows;
		}

		/// <summary>
		/// Returns <see cref="VirtualKeyModifiers"/> active at the moment of the call (excluding the Windows key)
		/// </summary>
		/// <returns></returns>
		public static VirtualKeyModifiers GetCurrentModifiersWithoutWindows()
		{
			var window = CoreWindow.GetForCurrentThread();

			var shift = window.GetKeyState(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down) ?
				VirtualKeyModifiers.Shift : VirtualKeyModifiers.None;

			var ctrl = window.GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down) ?
				VirtualKeyModifiers.Control : VirtualKeyModifiers.None;

			var menu = window.GetKeyState(VirtualKey.Menu).HasFlag(CoreVirtualKeyStates.Down) ?
				VirtualKeyModifiers.Menu : VirtualKeyModifiers.None;

			return shift | ctrl | menu;
		}

		#endregion
	}
}