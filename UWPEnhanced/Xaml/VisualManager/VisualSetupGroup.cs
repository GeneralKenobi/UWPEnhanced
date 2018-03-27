using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Class which handles <see cref="VisualSetup"/>s associated with a given <see cref="DependencyObject"/>
	/// </summary>
	public class VisualSetupGroup : AttachableDependencyCollectionOfT<VisualSetup>
	{
		/// <summary>
		/// The group used by this instance to store its visual state setups
		/// </summary>
		private VisualStateGroup _AssociatedVisualStateGroup = null;

		/// <summary>
		/// Before attaching checks if the <paramref name="obj"/> is a <see cref="FrameworkElement"/>, then adds the visual
		/// group to the element's visual groups.
		/// </summary>
		/// <param name="obj"></param>
		public override void Attach(DependencyObject obj)
		{
			if (obj is FrameworkElement element)
			{
				base.Attach(obj);

				if (_AssociatedVisualStateGroup == null)
				{
					// Create a new group dedicated for the VisualStateManager
					_AssociatedVisualStateGroup = new VisualStateGroup();
				}
				// Add the items
				foreach(var item in this)
				{
					if(item is VisualSetup setup)
					{
						_AssociatedVisualStateGroup.States.Add(setup.State);
					}
				}

				// Add the group to the element
				VisualStateManager.GetVisualStateGroups(element).Insert(0, _AssociatedVisualStateGroup);
			}
			else
			{
				throw new ArgumentException("Visual states can be added only to " + nameof(FrameworkElement) + "s");
			}
		}

		/// <summary>
		/// Detatches the group and removes the group from the <see cref="_AssociatedVisualStateGroup"/>
		/// </summary>
		public override void Detach()
		{
			VisualStateManager.GetVisualStateGroups(AttachedTo as FrameworkElement).Remove(_AssociatedVisualStateGroup);

			base.Detach();
		}

		/// <summary>
		/// Checks the new item and adds it to the visual group
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override VisualSetup NewElementCheckRoutine(DependencyObject item)
		{
			var checkedItem = base.NewElementCheckRoutine(item);

			_AssociatedVisualStateGroup.States.Add(checkedItem.State);

			return checkedItem;
		}
		
		/// <summary>
		/// Removes the item from the <see cref="_AssociatedVisualStateGroup"/> and detatches it
		/// </summary>
		/// <param name="item"></param>
		protected override void CleanupRoutine(VisualSetup item)
		{
			base.CleanupRoutine(item);

			_AssociatedVisualStateGroup.States.Remove(item.State);
		}
	}
}
