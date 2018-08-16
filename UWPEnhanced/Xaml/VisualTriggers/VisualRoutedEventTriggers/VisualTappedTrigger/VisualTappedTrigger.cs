using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Provides means of notifying whenever tapped routed events is raised
	/// </summary>
	public class VisualTappedTrigger : VisualRoutedEventTriggerWithModifiers<TappedRoutedEventArgs>
	{
		#region Protected methods
		
		/// <summary>
		/// Subscribes to the <paramref name="type"/> event on attached <see cref="UIElement"/>
		/// </summary>
		/// <param name="type"></param>
		protected override void Subscribe()
		{
			// Get the attached, if it's not null (i.e. this event trigger is attached to something)
			if (AttachedTo is UIElement element)
			{
				element.Tapped += TriggerEvent;
			}
		}

		/// <summary>
		/// Unsubscribes from the <paramref name="type"/> event on attached <see cref="UIElement"/>
		/// </summary>
		/// <param name="type"></param>
		protected override void Unsubscribe()
		{
			// Get the attached, if it's not null (i.e. this event trigger is attached to something)
			if (AttachedTo is UIElement element)
			{
				element.Tapped -= TriggerEvent;
			}
		}

		/// <summary>
		/// Sets the Handled property on <see cref="TappedRoutedEventArgs"/>
		/// </summary>
		/// <param name="e"></param>

		protected override void AssignHandled(TappedRoutedEventArgs e) => e.Handled = SetHandled;

		#endregion
	}
}