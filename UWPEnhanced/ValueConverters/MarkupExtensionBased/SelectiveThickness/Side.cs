using System;

namespace UWPEnhanced.ValueConverters
{
	/// <summary>
	/// Enumeration of possible side configurations
	/// </summary>
	[Flags]
	public enum Side
	{
		None = 0,
		Left = 1,		
		Top = 2,
		Right = 4,
		Bottom = 8,
		LeftTop = 3,
		LeftRight = 5,
		LeftBottom = 9,
		TopRight = 6,
		TopBottom = 10,
		RightBottom = 12,
		LeftTopRight = 7,
		LeftTopBottom = 11,
		LeftRightBottom = 13,
		TopRightBottom = 14,
		All = 15,
	}
}