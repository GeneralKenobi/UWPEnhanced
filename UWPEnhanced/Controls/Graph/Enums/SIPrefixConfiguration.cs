namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Types of SI prefix configurations available for labels in <see cref="Graph"/>
	/// </summary>
	public enum SIPrefixConfiguration
	{
		/// <summary>
		/// No prefixes are generated
		/// </summary>
		None = 0,

		/// <summary>
		/// The minimum adequate prefix for the whole data set is found and applied to all values
		/// </summary>
		MinForAll = 1,

		/// <summary>
		/// The maximum adequate prefix for the whole data set is found and applied to all values
		/// </summary>
		MaxForAll = 2,

		/// <summary>
		/// Each value is given a prefix most adequate for it
		/// </summary>
		Adequate = 3,
	}
}