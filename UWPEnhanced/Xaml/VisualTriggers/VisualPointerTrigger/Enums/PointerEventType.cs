using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Enum denoting the type of the pointer event type, these correspond to pointer events on <see cref="UIElement"/>
	/// </summary>
	public enum PointerEventType
	{
		// Default value - none
		None = 0,

		PointerCanceled = 1,

		PointerCaptureLost = 2,

		PointerEntered = 3,

		PointerExited = 4,

		PointerMoved = 5,

		PointerPressed = 6,

		PointerReleased = 7,

		PointerWheelChanged = 8,
	}
}
