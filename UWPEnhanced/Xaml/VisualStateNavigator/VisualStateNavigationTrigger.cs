using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	public class VisualStateNavigationTrigger : DependencyObject
	{
		/// <summary>
		/// Event fired when the trigger occurs
		/// </summary>
		public event NavigateEventHandler Navigate;

		/// <summary>
		/// Fires the <see cref="Navigate"/> event
		/// </summary>
		protected void FireNavigate() => Navigate?.Invoke(this);
	}
}
