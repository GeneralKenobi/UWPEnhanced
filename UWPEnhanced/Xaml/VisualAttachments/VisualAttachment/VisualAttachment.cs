using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Standard implementation of <see cref="IAttachable"/>
	/// </summary>
	public class VisualAttachment : DependencyObject, IAttachable
	{
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="LockAttachedTo">If true, attaching to a new <see cref="DependencyObject"/> will be possible
		/// only if the <see cref="AttachedTo"/> is currently null</param>
		public VisualAttachment(bool lockAttachedTo = true)
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

		#region IVisualAttachment implementation

		/// <summary>
		/// Getter to the <see cref="DependencyObject"/> the <see cref="IAttachable"/> is attached to.
		/// Set is exposed privately
		/// </summary>
		public DependencyObject AttachedTo { get; private set; }

		/// <summary>
		/// True if this <see cref="IAttachable"/> is attached
		/// </summary>
		public bool IsAttached => AttachedTo != null;

		/// <summary>
		/// Attaches this <see cref="IAttachable"/> to the specified <see cref="DependencyObject"/>
		/// </summary>
		/// <param name="obj"></param>
		public virtual void Attach(DependencyObject obj)
		{
			// Skip if the obj is the same as the stored object
			if(AttachedTo == obj)
			{
				return;
			}

			// Check if this VisualAttachment cna be reattached and if it's already attached
			if(LockAttachedTo && IsAttached)
			{
				throw new InvalidOperationException("This " + nameof(VisualAttachment) + " can be attached only if it's not attached; " +
					"Current settings prevent it from being reattached");
			}

			// Debug information for null obj
			Debug.Assert(obj != null, "Can't attach self to a null object");

			// Assign the obj to AttachedTo; If obj == null, throw ArgumentException instead
			AttachedTo = obj ?? throw new ArgumentNullException(nameof(obj));
		}

		/// <summary>
		/// Detaches this <see cref="IAttachable"/> from the <see cref="DependencyObject"/> it is attached to
		/// (if it's attached to begin with)
		/// </summary>
		public virtual void Detach() => AttachedTo = null;

		#endregion
	}
}
