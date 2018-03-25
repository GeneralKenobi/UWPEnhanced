using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Visual setups are one step above visual states. They allow for improved management of <see cref="FrameworkElement"/>'s
	/// visuals - with use of <see cref="VisualState"/>s, transition into and out of state animations,
	/// intermediate transition states, chained temporary states and more.
	/// </summary>
	public class VisualManager
	{
		#region Constructor

		/// <summary>
		/// Default constructor, hidden
		/// </summary>
		private VisualManager() { }

		#endregion

		#region AttachedVisuals

		/// <summary>
		/// Getter for <see cref="VisualSetupsProperty"/>
		/// </summary>
		public static VisualAttachmentCollection<VisualSetup> GetAttachedVisuals(DependencyObject obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(nameof(obj));
			}

			var collection = (VisualAttachmentCollection<VisualSetup>)obj.GetValue(VisualSetupsProperty);

			// If the collection wasn't yet set
			if (collection == null)
			{
				// Create a new instance
				collection = new VisualAttachmentCollection<VisualSetup>();

				// And set it for the object
				obj.SetValue(VisualSetupsProperty, collection);

				if (obj is FrameworkElement element)
				{
					// Subscribe to loaded and unloaded events
					element.Loaded -= FrameworkElementLoaded;
					element.Loaded += FrameworkElementLoaded;
					element.Unloaded -= FrameworkElementUnloaded;
					element.Unloaded += FrameworkElementUnloaded;
				}
			}

			return collection;
		}

		/// <summary>
		/// Setter for <see cref="VisualSetupsProperty"/>
		/// </summary>
		public static void SetAttachedVisuals(DependencyObject obj, VisualAttachmentCollection<VisualSetup> value)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(nameof(obj));
			}

			obj.SetValue(VisualSetupsProperty, value);
		}

		/// <summary>
		/// Attached property for attached visuals
		/// </summary>
		public static readonly DependencyProperty VisualSetupsProperty =
			DependencyProperty.RegisterAttached("VisualSetups", typeof(VisualAttachmentCollection<VisualSetup>),
			 typeof(VisualAttachments), new PropertyMetadata(null, new PropertyChangedCallback(AttachedVisualsChanged)));

		/// <summary>
		/// Handles changes in <see cref="VisualSetupsProperty"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void AttachedVisualsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			// Dont' do anything if the value didn't really change
			if (e.OldValue == e.NewValue)
			{
				return;
			}

			// If old collection wasn't null, detatch it
			if (e.OldValue is VisualAttachmentCollection<VisualSetup> cOld)
			{
				cOld.Detach();
			}

			// If the new collection isn't null and sender isn't null, attatch the new collection to the sender
			if (e.NewValue is VisualAttachmentCollection<VisualSetup> cNew && sender != null)
			{
				cNew.Attach(sender);
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Method to call when FrameworkElement is loaded
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void FrameworkElementLoaded(object sender, RoutedEventArgs e)
		{
			// If sender is a DependencyObject
			if (sender is DependencyObject obj)
			{
				// Attach its visuals
				GetAttachedVisuals(obj).Attach(obj);
			}
		}

		/// <summary>
		/// Method to call when FrameworkElement is unloaded
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void FrameworkElementUnloaded(object sender, RoutedEventArgs e)
		{
			// If sender is a DependencyObject
			if (sender is DependencyObject obj)
			{
				// Attach its visuals
				GetAttachedVisuals(obj).Detach();
			}
		}

		#endregion
	}
}
