using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// <see cref="DataTemplateSelector"/> that chooses a template from a collection of provided templates based on the type of
	/// the Data that will be presented.
	/// </summary>
	public class TypeBasedDataTemplateSelector : DataTemplateSelector, IAttachable
	{
		#region Public properties

		/// <summary>
		/// Getter to the <see cref="DependencyObject"/> the <see cref="IAttachable"/> is attached to.
		/// </summary>
		public DependencyObject AttachedTo { get; private set; }

		/// <summary>
		/// True if this <see cref="IAttachable"/> is attached
		/// </summary>
		public bool IsAttached => AttachedTo != null;

		#endregion

		#region Public methods

		/// <summary>
		/// Attaches this <see cref="IAttachable"/> to the specified <see cref="DependencyObject"/>
		/// </summary>
		/// <param name="obj"></param>
		public void Attach(DependencyObject obj)
		{
			// Skip if the obj is the same as the stored object
			if (AttachedTo == obj)
			{
				return;
			}

			// Debug information for null obj
			Debug.Assert(obj != null, "Can't attach self to a null object");

			// Assign the obj to AttachedTo; If obj == null, throw ArgumentNullException instead
			AttachedTo = obj ?? throw new ArgumentNullException(nameof(obj));
		}

		/// <summary>
		/// Detaches this <see cref="IAttachable"/> from the <see cref="DependencyObject"/> it is attached to
		/// (if it's attached to begin with)
		/// </summary>
		public void Detach() => AttachedTo = null;

		#endregion
	}
}