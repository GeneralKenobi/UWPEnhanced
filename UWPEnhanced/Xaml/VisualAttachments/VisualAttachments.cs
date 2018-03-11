using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Container for visual attachments in xaml (like VisualStateManager)
	/// </summary>
	public sealed class VisualAttachments
	{
		#region Constructor

		/// <summary>
		/// Default constructor, hidden
		/// </summary>
		private VisualAttachments() { }

		#endregion

		#region AttachedVisuals

		/// <summary>
		/// Getter for <see cref="AttachedVisualsProperty"/>
		/// </summary>
		public static VisualAttachmentCollection<IAttachable> GetAttachedVisuals(DependencyObject obj)
		{
			if(obj == null)
			{
				throw new ArgumentNullException(nameof(obj));
			}

			var collection = (VisualAttachmentCollection<IAttachable>)obj.GetValue(AttachedVisualsProperty);

			// If the collection wasn't yet set
			if(collection == null)
			{
				// Create a new instance
				collection = new VisualAttachmentCollection<IAttachable>();
				
				// And set it for the object
				obj.SetValue(AttachedVisualsProperty, collection);

				if(obj is FrameworkElement element)
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
		/// Setter for <see cref="AttachedVisualsProperty"/>
		/// </summary>
		public static void SetAttachedVisuals(DependencyObject obj, VisualAttachmentCollection<IAttachable> value)
		{
			if(obj == null)
			{
				throw new ArgumentNullException(nameof(obj));
			}

			obj.SetValue(AttachedVisualsProperty, value);
		}

		/// <summary>
		/// Attached property for attached visuals
		/// </summary>
		public static readonly DependencyProperty AttachedVisualsProperty =
			DependencyProperty.RegisterAttached("AttachedVisuals", typeof(VisualAttachmentCollection<IAttachable>),
			 typeof(VisualAttachments), new PropertyMetadata(null, new PropertyChangedCallback(AttachedVisualsChanged)));
		
		/// <summary>
		/// Handles changes in <see cref="AttachedVisualsProperty"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void AttachedVisualsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			// Dont' do anything if the value didn't really change
			if(e.OldValue==e.NewValue)
			{
				return;
			}

			// If old collection wasn't null, detatch it
			if(e.OldValue is VisualAttachmentCollection<IAttachable> cOld)
			{
				cOld.Detach();
			}

			// If the new collection isn't null and sender isn't null, attatch the new collection to the sender
			if (e.NewValue is VisualAttachmentCollection<IAttachable> cNew && sender != null)
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
			if(sender is DependencyObject obj)
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
