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
		#region Find parent of type T

		/// <summary>
		/// Finds and returns a parent of type <typeparamref name="T"/> or null if the parent wasn't found.
		/// If <paramref name="obj"/> is of type <typeparamref name="T"/> it will be the result.
		/// </summary>
		/// <typeparam name="T">Type of the parent to look for</typeparam>
		/// <param name="obj"><see cref="DependencyObject"/> whose parent to look for</param>
		/// <returns></returns>
		public static T FindParentOrSelf<T>(this DependencyObject obj)
			where T : class
		{
			// Keep looking until either the object becomes null or parent of type T is found
			while (obj != null && !(obj is T))
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
		public static bool TryFindParentOrSelf<T>(this DependencyObject obj, out T parent)
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
		public static T FindParent<T>(this DependencyObject obj)
			where T : class => obj == null ? null : FindParentOrSelf<T>(VisualTreeHelper.GetParent(obj));

		/// <summary>
		/// Tries to find a parent of type <typeparamref name="T"/> and assign it to <paramref name="parent"/>. If the parent is
		/// found, returns true, if it is not found assigns null and returns false
		/// </summary>
		/// <typeparam name="T">Type of the parent to look for</typeparam>
		/// <param name="obj"><see cref="DependencyObject"/> whose parent to look for</param>
		/// <param name="parent">Variable to assign the parent to</param>
		/// <returns></returns>
		public static bool TryFindParent<T>(this DependencyObject obj, out T parent)
			where T : class
		{
			if (obj == null)
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

		#region Find child

		/// <summary>
		/// Finds and returns the first occurance of a child of type <typeparamref name="T"/>. Search order is branch by branch
		/// </summary>
		/// <typeparam name="T">Type to look for</typeparam>
		/// <param name="obj">Object whose children to search through</param>
		/// <returns>First found child of type T or null if none was found</returns>
		private static T FindChild<T>(this DependencyObject obj)
			where T : DependencyObject
		{
			// For each child
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); ++i)
			{
				// Get it
				var nextChild = VisualTreeHelper.GetChild(obj, i);
				
				// If it's of type T, return it
				if (nextChild is T casted)
				{
					return casted;
				}

				// Otherwise search through its children
				var subSearchResult = nextChild.FindChild<T>();

				// And if there was a child of type T
				if(subSearchResult != null)
				{
					// Return it
					return subSearchResult;
				}
			}

			return null;
		}

		/// <summary>
		/// Finds the first occurance of a child of type <typeparamref name="T"/>. Search order is branch by branch.
		/// If the child was found it is stored in <paramref name="child"/> and true is returned.
		/// </summary>
		/// <typeparam name="T">Type to look for</typeparam>
		/// <param name="obj">Object whose children to search through</param>
		/// <param name="child">Reference to store the found child in</param>
		/// <returns></returns>
		public static bool TryFindChild<T>(this DependencyObject obj, out T child)
			where T : DependencyObject
		{
			child = FindChild<T>(obj);
			return child != null;
		}

		/// <summary>
		/// Returns all children that are of type T
		/// </summary>
		/// <typeparam name="T">Type to look for</typeparam>
		/// <param name="obj">Object whose children to search through</param>
		/// <returns></returns>
		public static IEnumerable<T> FindChildren<T>(this DependencyObject obj)
			where T : DependencyObject
		{
			// For each child of the object
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); ++i)
			{
				// Get the next child
				var nextChild = VisualTreeHelper.GetChild(obj, i);

				// If it'ss of type T, return it aswell
				if (nextChild is T casted)
				{
					yield return casted;
				}

				// Search through its children and return all found occurances
				foreach(var item in FindChildren<T>(nextChild))
				{
					yield return item;
				}
			}
		}

		#endregion

		#region Find child or self

		/// <summary>
		/// Finds and returns the first occurance of a child (or self) of type <typeparamref name="T"/>.
		/// Search order is branch by branch
		/// </summary>
		/// <typeparam name="T">Type to look for</typeparam>
		/// <param name="obj">Object whose children to search through</param>
		/// <returns>First found child of type T or null if none was found</returns>
		private static T FindChildOrSelf<T>(this DependencyObject obj)
			where T : DependencyObject
		{
			if(obj is T castedObj)
			{
				return castedObj;
			}
			else
			{
				return FindChild<T>(obj);
			}			
		}

		/// <summary>
		/// Finds the first occurance of a child (or self) of type <typeparamref name="T"/>. Search order is branch by branch.
		/// If the child was found it is stored in <paramref name="child"/> and true is returned.
		/// </summary>
		/// <typeparam name="T">Type to look for</typeparam>
		/// <param name="obj">Object whose children to search through</param>
		/// <param name="child">Reference to store the found child in</param>
		/// <returns></returns>
		public static bool TryFindChildOrSelf<T>(this DependencyObject obj, out T child)
			where T : DependencyObject
		{
			if(obj is T casted)
			{
				child = casted;
			}
			else
			{
				child = FindChild<T>(obj);
			}

			return child != null;
		}

		/// <summary>
		/// Returns all children (self as well) that are of type T
		/// </summary>
		/// <typeparam name="T">Type to look for</typeparam>
		/// <param name="obj">Object whose children to search through</param>
		/// <returns></returns>
		public static IEnumerable<T> FindChildrenOrSelf<T>(this DependencyObject obj)
			where T : DependencyObject
		{
			if (obj is T casted)
			{
				yield return casted;
			}

			foreach (var item in FindChildren<T>(obj))
			{
				yield return item;
			}			
		}

		#endregion
	}
}
