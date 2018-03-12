using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Class containing helper methods related to VisualTree
	/// </summary>
	public static class VisualTreeHelpers
	{
		/// <summary>
		/// Finds and returns a parent of type <typeparamref name="T"/> or null if the parent wasn't found
		/// </summary>
		/// <typeparam name="T">Type of the parent to look for</typeparam>
		/// <param name="obj"><see cref="DependencyObject"/> whose parent to look for</param>
		/// <returns></returns>
		public static T FindParent<T>(DependencyObject obj)
			where T : class
		{
			// Keep looking until either the object becomes null or parent of type T is found
			while(obj != null && !(obj is T))
			{
				obj = VisualTreeHelper.GetParent(obj);
			}			
			
			return obj as T;
		}
	}
}
