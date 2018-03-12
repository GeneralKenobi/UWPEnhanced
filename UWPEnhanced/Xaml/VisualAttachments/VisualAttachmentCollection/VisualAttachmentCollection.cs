﻿using System;
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
	public class VisualAttachmentCollection<T> : DependencyObjectCollectionOfT<T>, IAttachable
		where T : class, IAttachable
	{
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="LockAttachedTo">If true, attaching to a new <see cref="DependencyObject"/> will be possible
		/// only if the <see cref="AttachedTo"/> is currently null</param>
		public VisualAttachmentCollection(bool lockAttachedTo = true)
		{
			LockAttachedTo = lockAttachedTo;
		}

		#endregion
		
		#region Public Properties

		/// <summary>
		/// If true, attaching to a new <see cref="DependencyObject"/> will be possible
		/// only if the <see cref="AttachedTo"/> is currently null
		/// </summary>
		public bool LockAttachedTo { get; private set; } = true;

		#endregion
		
		#region IAttachable implementation

		/// <summary>
		/// Getter to the <see cref="DependencyObject"/> the <see cref="IAttachable"/> is attached to
		/// </summary>
		public DependencyObject AttachedTo { get; private set; }

		/// <summary>
		/// True if this <see cref="IAttachable"/> is attached
		/// </summary>
		public bool IsAttached => AttachedTo != null;

		/// <summary>
		/// Attaches this <see cref="IAttachable"/> as well as all contained <see cref="IAttachable"/> <see cref="DependencyObject"/>s
		/// to the specified <see cref="DependencyObject"/>.
		/// </summary>
		/// <param name="obj"></param>
		public virtual void Attach(DependencyObject obj)
		{
			// Skip if the obj is the same as the stored object
			if (AttachedTo == obj)
			{
				return;
			}

			// Check if this VisualAttachment cna be reattached and if it's already attached
			if (LockAttachedTo && IsAttached)
			{
				throw new InvalidOperationException("This " + nameof(VisualAttachmentCollection<T>) + " can be attached only if it's not attached; " +
					"Current settings prevent it from being reattached");
			}

			// Debug information for null obj
			Debug.Assert(obj != null, "Can't attach self to a null object");

			// Assign the obj to AttachedTo; If obj == null, throw ArgumentException instead
			AttachedTo = obj ?? throw new ArgumentNullException(nameof(obj));

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
		public virtual void Detach()
		{
			if (IsAttached)
			{
				// For each DependencyObject
				foreach (DependencyObject containedObject in this)
				{
					// If it implements IAttachable, attach it to the obj
					(containedObject as IAttachable)?.Detach();
				}

				AttachedTo = null;
			}
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
