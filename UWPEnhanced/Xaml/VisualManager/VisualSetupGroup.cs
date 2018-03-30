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
	public class VisualSetupGroup : DependencyObjectCollectionOfT<VisualSetup>
	{
		/// <summary>
		/// Checks the new item and adds it to the visual group
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override VisualSetup NewElementCheckRoutine(DependencyObject item)
		{
			var checkedItem = base.NewElementCheckRoutine(item);

			return checkedItem;
		}
		
		/// <summary>
		/// Removes the item from the <see cref="_AssociatedVisualStateGroup"/> and detatches it
		/// </summary>
		/// <param name="item"></param>
		protected override void CleanupRoutine(VisualSetup item)
		{
			base.CleanupRoutine(item);
		}
	}
}
