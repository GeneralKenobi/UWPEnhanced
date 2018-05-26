using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Position for the menu control
	/// </summary>
	public enum MenuPosition
	{
		/// <summary>
		/// Menu is on the left, opens to the right, content is stored vertically
		/// </summary>
		Left = 0,

		/// <summary>
		/// Menu is on the top, opens to the bottom, content is stored horizontally
		/// </summary>
		Top = 1,

		/// <summary>
		/// Menu is on the right, opens to the left, content is stored vertically
		/// </summary>
		Right = 2,

		/// <summary>
		/// Menu is on the bottom, opens to the top, content is stored horizontally
		/// </summary>
		Bottom = 3,
	}

	/// <summary>
	/// Menu positions that should be restricted in the MenuRepositioningTool
	/// </summary>
	[Flags]
	public enum RestrictedMenuPositions
	{
		/// <summary>
		/// No position is restricted
		/// </summary>
		None = 0,

		/// <summary>
		/// The left position is restricted
		/// </summary>
		Left = 1,

		/// <summary>
		/// The top position is restricted
		/// </summary>
		Top = 2,

		/// <summary>
		/// The right position is restricted
		/// </summary>
		Right = 4,

		/// <summary>
		/// The bottom position is restricted
		/// </summary>
		Bottom = 8,

		/// <summary>
		/// The vertical positions (left and right) are restricted
		/// </summary>
		Vertical = 5,

		/// <summary>
		/// The horizontal positions (top and bottom) are restricted
		/// </summary>
		Horizontal = 10,
	}

}