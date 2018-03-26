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
		


		protected override VisualSetup NewElementCheckRoutine(DependencyObject item)
		{
			var baseChecked =  base.NewElementCheckRoutine(item);

			if(VisualTreeHelpers.TryFindParentOrSelf(baseChecked, out FrameworkElement parent))
			{
				var registeredGroups = VisualStateManager.GetVisualStateGroups(parent);

				if(registeredGroups.Count > 0)
				{
					var primaryGroup = registeredGroups[0];
					
				}
			}

			return baseChecked;
		}

		#region Public Methods

		/// <summary>
		/// Adds a <see cref="VisualSetup"/> to the container
		/// </summary>
		/// <param name="setup"></param>
		public void AddVisualSetup(VisualSetup setup)
		{
			//if(!_AssociatedVisualSetups.Contains(setup))
			//{
			//	_AssociatedVisualSetups.Add(setup);
			//}
		}

		#endregion
	}
}
