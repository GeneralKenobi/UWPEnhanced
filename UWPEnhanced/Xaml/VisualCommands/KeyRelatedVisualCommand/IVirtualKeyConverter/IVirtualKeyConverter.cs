using Windows.System;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Interface for converting <see cref="VirtualKey"/>s to <see cref="string"/>s
	/// </summary>
	public interface IVirtualKeyConverter
	{
		#region Methods

		/// <summary>
		/// Converts a <see cref="VirtualKey"/> to string.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		string Convert(VirtualKey key);

		#endregion
	}
}