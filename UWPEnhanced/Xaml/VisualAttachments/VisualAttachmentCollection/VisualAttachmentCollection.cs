using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Collection of items that derive from <see cref="IAttachable"/>
	/// </summary>
	/// <typeparam name="T">Type of the items in the collection. It should derive from <see cref="IAttachable"/>
	/// and, in order to ensure proper functioning, from <see cref="DependencyObject"/></typeparam>
	public class VisualAttachmentCollection<T> : AttachableDependencyCollectionOfT<T>
		where T : class, IAttachable
	{
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="LockAttachedTo">If true, attaching to a new <see cref="DependencyObject"/> will be possible
		/// only if the <see cref="AttachedTo"/> is currently null</param>
		public VisualAttachmentCollection(bool allowDuplicates = false, bool lockAttachedTo = true)
			: base(allowDuplicates, lockAttachedTo)	{ }

		#endregion
		
		#region IAttachable implementation
				
		/// <summary>
		/// Attaches this <see cref="IAttachable"/> as well as all contained <see cref="IAttachable"/> <see cref="DependencyObject"/>s
		/// to the specified <see cref="DependencyObject"/>.
		/// </summary>
		/// <param name="obj"></param>
		public override void Attach(DependencyObject obj)
		{
			// Attach self using the base class implementation
			base.Attach(obj);
			
			// For each DependencyObject
			foreach(DependencyObject containedObject in this)
			{
				// If it implements IAttachable, attach it to the obj
				(containedObject as IAttachable)?.Attach(obj);
			}
		}

		/// <summary>
		/// Detaches this <see cref="IAttachable"/> as well as all contained <see cref="IAttachable"/> <see cref="DependencyObject"/>s
		/// from the <see cref="DependencyObject"/> they are attached to (if they're attached to begin with)
		/// </summary>
		public override void Detach()
		{
			if (IsAttached)
			{
				// For each DependencyObject
				foreach (DependencyObject containedObject in this)
				{
					// If it implements IAttachable, attach it to the obj
					(containedObject as IAttachable)?.Detach();
				}
			}

			// Detatch self using the base implementation
			base.Detach();
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Performs checks on new <see cref="DependencyObject"/>s added to the collection and if all are passed, returns
		/// the object as an IAttachable.
		/// Throws an exception if: the new object doesn't implement <see cref="IAttachable"/>, if it's already in the collection
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override T NewElementCheckRoutine(DependencyObject item)
		{
			var attachable = base.NewElementCheckRoutine(item);

			// If this collection is attached, attach the object as well
			if(IsAttached)
			{
				attachable.Attach(AttachedTo);
			}

			return attachable;
		}

		/// <summary>
		/// Detatches removed items
		/// </summary>
		/// <param name="item"></param>
		protected override void CleanupRoutine(T item) => item.Detach();

		#endregion
	}
}
