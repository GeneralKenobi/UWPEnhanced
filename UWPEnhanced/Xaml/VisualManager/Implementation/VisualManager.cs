using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Visual setups are one step above visual states. They enable improved management of <see cref="FrameworkElement"/>'s
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

		#region Visual Setups

		/// <summary>
		/// Getter for <see cref="VisualSetupsProperty"/>
		/// </summary>
		public static DependencyObjectCollectionOfT<VisualSetupGroup> GetVisualSetups(DependencyObject obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(nameof(obj));
			}

			var collection = (DependencyObjectCollectionOfT<VisualSetupGroup>)obj.GetValue(VisualSetupsProperty);

			// If the collection wasn't yet set
			if (collection == null)
			{
				// Create a new instance
				collection = new DependencyObjectCollectionOfT<VisualSetupGroup>();

				// And set it for the object
				obj.SetValue(VisualSetupsProperty, collection);				
			}

			return collection;
		}

		/// <summary>
		/// Setter for <see cref="VisualSetupsProperty"/>
		/// </summary>
		public static void SetVisualSetups(DependencyObject obj, DependencyObjectCollectionOfT<VisualSetupGroup> value)
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
			DependencyProperty.RegisterAttached("VisualSetups", typeof(DependencyObjectCollectionOfT<VisualSetupGroup>),
			 typeof(VisualManager), new PropertyMetadata(null, new PropertyChangedCallback(VisualSetupsChanged)));

		/// <summary>
		/// Handles changes in <see cref="VisualSetupsProperty"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void VisualSetupsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			// Dont' do anything if the value didn't really change
			if (e.OldValue == e.NewValue)
			{
				return;
			}

			// If old collection wasn't null, detatch it
			if (e.OldValue is DependencyObjectCollectionOfT<VisualSetupGroup> cOld)
			{
				//cOld.Detach();
			}

			// If the new collection isn't null and sender isn't null, attatch the new collection to the sender
			if (e.NewValue is DependencyObjectCollectionOfT<VisualSetupGroup> cNew && sender != null)
			{
				//cNew.Attach(sender);
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Goes to the given <see cref="IVisualSetup"/> in the given <see cref="VisualSetupGroup"/>. Returns a <see cref="Task"/>
		/// that will complete when the transition is finished. If the <see cref="IVisualSetup"/> was found, it will be chosen
		/// as the current setup immediately. The <see cref="Task"/> will have true result if the transition was successful
		/// </summary>
		/// <param name="element"></param>
		/// <param name="setup"></param>
		/// <returns></returns>
		public static Task<bool> GoToSetup(FrameworkElement element, string setup, string group = "", bool useTransitions = true)
		{
			// Get the setups
			var definedSetups = GetVisualSetups(element);

			// If the setups are defined
			if (definedSetups != null)
			{
				// Search for the specified group
				foreach (VisualSetupGroup item in definedSetups)
				{
					// If it was found
					if (item.Name == group)
					{
						// Return the group's transition task
						return item.GoToSetup(setup, useTransitions);
					}
				}
			}

			// Otherwise return an already completed task
			return new TaskCompletionSource<bool>(false).Task;
		}

		#endregion
	}
}