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
	/// Standard implementation of <see cref="IVisualAttachment"/>
	/// </summary>
	public class VisualAttachment : DependencyObject, IVisualAttachment
	{
		#region IVisualAttachment implementation

		/// <summary>
		/// Getter to the <see cref="DependencyObject"/> the <see cref="IVisualAttachment"/> is attached to.
		/// Set is exposed privately
		/// </summary>
		public DependencyObject AttachedTo { get; private set; }

		/// <summary>
		/// Attaches this <see cref="IVisualAttachment"/> to the specified <see cref="DependencyObject"/>
		/// </summary>
		/// <param name="obj"></param>
		public void Attach(DependencyObject obj)
		{
			// Skip if the obj is the same as the stored object
			if(AttachedTo == obj)
			{
				return;
			}

			// Debug information for null obj
			Debug.Assert(obj == null, "Can't attach self to a null object");

			// Assign the obj to AttachedTo; If obj == null, throw ArgumentException instead
			AttachedTo = obj ?? throw new ArgumentNullException(nameof(obj));
		}

		/// <summary>
		/// Detaches this <see cref="IVisualAttachment"/> from the <see cref="DependencyObject"/> it is attached to
		/// (if it's attached to begin with)
		/// </summary>
		public void Detach() => AttachedTo = null;
		

		#endregion
	}
}
