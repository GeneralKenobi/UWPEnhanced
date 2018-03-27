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
		private readonly VisualStateGroup _AssociatedVisualStateGroup = new VisualStateGroup();

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
							
				// Add the items
				foreach(var item in this)
				{
					if(item is VisualSetup setup)
					{
						var copy = new VisualState();

						foreach (var setter in setup.State.Setters)
						{
							copy.Setters.Add(setter);
						}

						_AssociatedVisualStateGroup.States.Add(copy);
					}
				}

				// Add the group to the element
				VisualStateManager.GetVisualStateGroups(element).Add(_AssociatedVisualStateGroup);
			}
			else
			{
				throw new ArgumentException("Visual states can be added only to " + nameof(FrameworkElement) + "s");
			}
		}
		TargetPropertyPath parh;
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
