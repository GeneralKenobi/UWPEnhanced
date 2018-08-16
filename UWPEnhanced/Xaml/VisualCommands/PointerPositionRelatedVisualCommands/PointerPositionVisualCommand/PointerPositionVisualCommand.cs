using System;
using System.Numerics;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// A class for in-xaml binding of commands that are triggered by pointer routed events and whose parameter is the pointer position
	/// relative to some entity. The parameter is given as a <see cref="Complex"/>
	/// </summary>
	public class PointerPositionVisualCommand : BasePointerPositionVisualCommand<VisualPointerTrigger, PointerRoutedEventArgs>
	{
		#region Protected methods

		/// <summary>
		/// Executes the command with the position of the cursor relative to <see cref="RelativeToElement"/>. If <see cref="RelativeToElement"/>
		/// is null, gets the position relative to <see cref="IAttachable.AttachedTo"/> (if it's an <see cref="UIElement"/>, if not
		/// then an exception is thrown). The position is passed as a <see cref="Complex"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void TriggerFiredCallback(object sender, PointerRoutedEventArgs e)
		{
			PointerPoint pointerInfo = null;

			// Determine the position relative to the chosen element
			switch (RelativeToOption)
			{
				case RelativeTo.AttachedTo:
					{
						pointerInfo = e.GetCurrentPoint(AttachedTo as UIElement);
					}
					break;

				case RelativeTo.BoundObject:
					{
						pointerInfo = e.GetCurrentPoint(RelativeToElement);
					}
					break;

				case RelativeTo.Window:
					{
						pointerInfo = e.GetCurrentPoint(null);
					}
					break;

				default:
					{
						throw new Exception("Unsupported case");
					}
			}

			// Create a new Complex off of it
			var param = new Complex(pointerInfo.Position.X, pointerInfo.Position.Y);

			// Execute the command (and use the converter if it was provided)
			Command?.Execute(Converter == null ? param : Converter.Convert(param));
		}

		#endregion
	}
}