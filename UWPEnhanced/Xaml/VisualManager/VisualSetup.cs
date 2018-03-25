using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPEnhanced.Xaml
{
	
    public class VisualSetup : DependencyObjectCollectionOfT<VisualState>, IAttachable
    {
		public DependencyObject AttachedTo { get; private set; }

		public bool IsAttached => AttachedTo != null;

		public void Attach(DependencyObject obj)
		{
			AttachedTo = obj;			
		}

		public void Detach()
		{
			//throw new NotImplementedException();
		}

		protected override VisualState NewElementCheckRoutine(DependencyObject item)
		{
			var a = base.NewElementCheckRoutine(item);
			t(a);
			return a;
		}

		private async void t(VisualState state)
		{
			if(state.Name=="test")
			{
				await Task.Delay(2000);
			}
			await Task.Delay(1000);
			var control = VisualTreeHelpers.FindParentOrSelf<Control>(AttachedTo);
			VisualStateGroup group = new VisualStateGroup();
			group.States.Add(state);
			VisualStateManager.GetVisualStateGroups(AttachedTo as FrameworkElement).Add(group);
			VisualStateManager.GoToState(control, state.Name, true);
		}
	}
}
