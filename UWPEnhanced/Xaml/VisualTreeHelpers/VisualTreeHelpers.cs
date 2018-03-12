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
		#region Find Parent of type T

		/// <summary>
		/// Finds and returns a parent of type <typeparamref name="T"/> or null if the parent wasn't found.
		/// If <paramref name="obj"/> is of type <typeparamref name="T"/> it will be the result.
		/// </summary>
		/// <typeparam name="T">Type of the parent to look for</typeparam>
		/// <param name="obj"><see cref="DependencyObject"/> whose parent to look for</param>
		/// <returns></returns>
		public static T FindParentOrSelf<T>(DependencyObject obj)
			where T : class
		{
			// Keep looking until either the object becomes null or parent of type T is found
			while(obj != null && !(obj is T))
			{
				obj = VisualTreeHelper.GetParent(obj);
			}			
			
			return obj as T;
		}

		/// <summary>
		/// Tries to find a parent of type <typeparamref name="T"/> and assign it to <paramref name="parent"/>. If the parent is
		/// found, returns true, if it is not found assigns null and returns false
		/// If <paramref name="obj"/> is of type <typeparamref name="T"/> it will be the result.
		/// </summary>
		/// <typeparam name="T">Type of the parent to look for</typeparam>
		/// <param name="obj"><see cref="DependencyObject"/> whose parent to look for</param>
		/// <param name="parent">Variable to assign the parent to</param>
		/// <returns></returns>
		public static bool TryFindParentOrSelf<T>(DependencyObject obj, out T parent)
			where T : class
		{
			parent = FindParentOrSelf<T>(obj);

			return parent != null;
		}

		/// <summary>
		/// Finds and returns a parent of type <typeparamref name="T"/> or null if the parent wasn't found
		/// </summary>
		/// <typeparam name="T">Type of the parent to look for</typeparam>
		/// <param name="obj"><see cref="DependencyObject"/> whose parent to look for</param>
		/// <returns></returns>
		public static T FindParent<T>(DependencyObject obj)
			where T : class => obj == null ? null : FindParentOrSelf<T>(VisualTreeHelper.GetParent(obj));

		/// <summary>
		/// Tries to find a parent of type <typeparamref name="T"/> and assign it to <paramref name="parent"/>. If the parent is
		/// found, returns true, if it is not found assigns null and returns false
		/// </summary>
		/// <typeparam name="T">Type of the parent to look for</typeparam>
		/// <param name="obj"><see cref="DependencyObject"/> whose parent to look for</param>
		/// <param name="parent">Variable to assign the parent to</param>
		/// <returns></returns>
		public static bool TryFindParent<T>(DependencyObject obj, out T parent)
			where T : class
		{
			if(obj == null)
			{
				parent = null;
				return false;
			}
			else
			{
				return TryFindParentOrSelf(VisualTreeHelper.GetParent(obj), out parent);
			}
		}

		#endregion
	}
}
