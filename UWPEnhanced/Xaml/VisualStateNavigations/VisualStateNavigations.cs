using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Container for <see cref="VisualStateNavigation"/>s which can be used to set them in xaml
	/// </summary>
	public class VisualStateNavigations : VisualAttachmentCollection<VisualStateNavigation>
	{
		protected override void OnVectorChanged(IObservableVector<DependencyObject> s, IVectorChangedEventArgs e)
		{
			switch(e.CollectionChange)
			{
				case CollectionChange.ItemInserted:
					{
						//_ControlArchive[(int)e.Index].;

					}
					break;
			}

			base.OnVectorChanged(s, e);
		}

	}
}
