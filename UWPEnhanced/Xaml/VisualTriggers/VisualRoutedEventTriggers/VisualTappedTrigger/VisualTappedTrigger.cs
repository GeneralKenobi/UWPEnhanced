using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Provides means of notifying whenever tapped routed events is raised
	/// </summary>
	public class VisualTappedTrigger : VisualRoutedEventTrigger<TappedRoutedEventArgs>
	{
		#region Private Methods
		
		/// <summary>
		/// Subscribes to the <paramref name="type"/> event on attached <see cref="UIElement"/>
		/// </summary>
		/// <param name="type"></param>
		private void Subcribe()
		{
			// Get the attached
			var element = AttachedTo as UIElement;
			
			// If it's not null (i.e. this event trigger is attached to something)
			if (element != null)
			{
				element.Tapped += TriggerEvent;
			}
		}

		/// <summary>
		/// Unsubscribes from the <paramref name="type"/> event on attached <see cref="UIElement"/>
		/// </summary>
		/// <param name="type"></param>
		private void Unsubcribe()
		{
			// Get the attached
			var element = AttachedTo as UIElement;

			// If it's not null (i.e. this event trigger is attached to something)
			if (element != null)
			{
				element.Tapped -= TriggerEvent;
			}
		}

		/// <summary>
		/// Method to subscribe with to Pointer Events on the attatched UIElement. It will check if modifiers match and, if so,
		/// raise the <see cref="IVisualTrigger.Triggered"/> event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TriggerEvent(object sender, TappedRoutedEventArgs e)
		{
			Triggered?.Invoke(this, e);

			OnTriggerEvent(sender, e);

			e.Handled = SetHandled;
		}

		#endregion

		#region Protected methods

		/// <summary>
		/// Method called when<see cref="IVisualTrigger.Triggered"/> fires. May be overriden to make use of the even args
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void OnTriggerEvent(object sender, TappedRoutedEventArgs e) { }

		#endregion

		#region Public Methods

		/// <summary>
		/// On top of the standard functionality, it unsubscribes from the old <see cref="UIElement"/> and subscribes to
		/// the new one
		/// </summary>
		/// <param name="obj"></param>
		public override void Attach(DependencyObject obj)
		{
			if (AttachedTo != null)
			{
				Unsubcribe();
			}

			// Only UIElements and above have pointer events
			if (obj is UIElement)
			{
				base.Attach(obj);

				if (AttachedTo != null)
				{
					Subcribe();
				}
			}
			else
			{
				throw new ArgumentException("Pointer Events are introduced in " + nameof(UIElement) + "; " +
					nameof(obj) + " has to derive from " + nameof(UIElement));
			}
		}

		/// <summary>
		/// On top of the standard functionality also unsubscribes from the detatched object
		/// </summary>
		public override void Detach()
		{
			if (AttachedTo != null)
			{
				Unsubcribe();
			}

			base.Detach();
		}

		#endregion
	}	
}