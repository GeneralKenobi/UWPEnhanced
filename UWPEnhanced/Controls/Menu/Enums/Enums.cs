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
}
