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
	[Flags]
	public enum PointerEventType
	{
		PointerCanceled = 1,

		PointerCaptureLost = 2,

		PointerEntered = 4,

		PointerExited = 8,

		PointerMoved = 16,

		PointerPressed = 32,

		PointerReleased = 64,

		PointerWheelChanged = 128,
	}
}
