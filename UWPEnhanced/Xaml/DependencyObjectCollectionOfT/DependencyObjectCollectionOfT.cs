using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using CSharpEnhanced.Helpers;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Collection of DependencyObjects of only type <typeparamref name="T"/>. If an object of a different type
	/// is stored in this collection, an exception will be thrown
	/// </summary>
	/// <typeparam name="T">Type to store in the collection, ot should derive from <see cref="DependencyObject"/></typeparam>
	public class DependencyObjectCollectionOfT<T> : DependencyObjectCollection
		where T : class
	{
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="allowDuplicates">The collection will allow/disallow duplicate items</param>
		public DependencyObjectCollectionOfT(bool allowDuplicates = false)
		{			
			_AllowDuplicates = allowDuplicates;
			VectorChanged += OnVectorChanged;
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Collection which serves as a control copy
		/// </summary>
		protected readonly List<T> _ControlArchive = new List<T>();

		/// <summary>
		/// Determines if this collection allows duplicates
		/// </summary>
		protected readonly bool _AllowDuplicates = false;

		#endregion

		#region Private Methods

		/// <summary>
		/// Handles changes in the main collection. Can be overridden in order to provide custom handling
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		protected virtual void OnVectorChanged(IObservableVector<DependencyObject> s, IVectorChangedEventArgs e)
		{
			switch (e.CollectionChange)
			{
				case CollectionChange.ItemInserted:
					{
						// Add the new item to the control archive using the new element check routine
						_ControlArchive.Insert((int)e.Index, NewElementCheckRoutine(this[(int)e.Index]));
					}
					break;

				case CollectionChange.ItemChanged:
					{
						// Reassign the item in the control archive using the cleanup routine on the old item
						// and the new element check routine
						CleanupRoutine(_ControlArchive[(int)e.Index]);
						_ControlArchive[(int)e.Index] = NewElementCheckRoutine(this[(int)e.Index]);
					}
					break;

				case CollectionChange.ItemRemoved:
					{
						// Remove the old item from the control archive
						CleanupRoutine(_ControlArchive[(int)e.Index]);
						_ControlArchive.RemoveAt((int)e.Index);
					}
					break;

				case CollectionChange.Reset:
					{
						// Cleanup after every item
						_ControlArchive.ForEach((x) => CleanupRoutine(x));

						// Clear the control archive
						_ControlArchive.Clear();

						foreach (DependencyObject item in this)
						{
							// Add items that are left to the control archive using new item check routine
							_ControlArchive.Add(NewElementCheckRoutine(item));
						}
					}
					break;

				default:
					{
						// Unsupported action - indicate by breaking if the debugger is attatched
						if (Debugger.IsAttached)
						{
							// Unhandled change in the collection, possible source of errors
							Debugger.Break();
						}
					}
					break;
			}

			// Check integrity (conditional on DEBUG)
			CheckIntegrity();
		}

		/// <summary>
		/// Performs checks on new <see cref="DependencyObject"/>s added to the collection and if all are passed, returns
		/// the object as T.
		/// Throws an exception if: the new object doesn't derive from <see cref="T"/>, or
		/// if the collection doesn't allow duplicates and the item is already in the collection
		/// Can be overriden to provide custom functionality.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected virtual T NewElementCheckRoutine(DependencyObject item)
		{
			// Check if the new item implements IAttachable
			var casted = item as T ?? throw new InvalidOperationException(
				"This collection stores only " + typeof(T).ToString() + " items");

			// Check if it's already in the collection
			if (!_AllowDuplicates && _ControlArchive.Contains(casted))
			{
				throw new InvalidOperationException("Duplicates aren't allowed");
			}			

			return casted;
		}

		/// <summary>
		/// Cleanup routine ran on every item that is deleted from this collection. Default version doesn't do anything
		/// but it can be overriden to provide necessary functionality
		/// </summary>
		/// <param name="item"></param>
		protected virtual void CleanupRoutine(T item) { }

		/// <summary>
		/// Checks the integrity of this collection (if the underlying collection is equal to <see cref="_ControlArchive"/>)
		/// Dedicated for debug
		/// </summary>
		[Conditional("DEBUG")]
		private void CheckIntegrity()
		{
			// Check the count
			if (this.Count == _ControlArchive.Count)
			{
				// For each element
				for (int i = 0; i < this.Count; ++i)
				{
					// If one pair isn't equal signal it by breaking if a debugger is attached
					if (this[i] != _ControlArchive[i] && Debugger.IsAttached)
					{
						// Collection integrity compromised - pair of items wasn't equal
						Debugger.Break();
					}
				}
			}
			else if (Debugger.IsAttached)
			{
				// Collection integrity compromised - _ControlArchive has a different number of items than underlying collection
				Debugger.Break();
			}
		}

		#endregion
	}
}
