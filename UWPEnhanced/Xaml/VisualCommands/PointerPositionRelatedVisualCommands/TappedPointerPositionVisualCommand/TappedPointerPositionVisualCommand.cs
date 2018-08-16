using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// A class for in-xaml binding of commands that are triggered by tapped routed event and use the pointer position as the command's
	/// parameter
	/// </summary>
	public class TappedPointerPositionVisualCommand : BasePointerPositionVisualCommand<VisualTappedTrigger, TappedRoutedEventArgs>
	{
		#region Protected methods

		/// <summary>
		/// Executes the command with the position of the cursor relative to <see cref="RelativeToElement"/>. If <see cref="RelativeToElement"/>
		/// is null, gets the position relative to <see cref="IAttachable.AttachedTo"/> (if it's an <see cref="UIElement"/>, if not
		/// then an exception is thrown). The position is passed as a <see cref="Complex"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void TriggerFiredCallback(object sender, TappedRoutedEventArgs e)
		{
			Point position;

			// Determine the position relative to the chosen element
			switch (RelativeToOption)
			{
				case RelativeTo.AttachedTo:
					{
						position = e.GetPosition(AttachedTo as UIElement);
					}
					break;

				case RelativeTo.BoundObject:
					{
						position = e.GetPosition(RelativeToElement);
					}
					break;

				case RelativeTo.Window:
					{
						position = e.GetPosition(null);
					}
					break;
			}

			// Create a new Complex off of it
			var param = new Complex(position.X, position.Y);

			// Execute the command (and use the converter if it was provided)
			Command?.Execute(Converter == null ? param : Converter.Convert(param));		
		}

		#endregion
	}
}