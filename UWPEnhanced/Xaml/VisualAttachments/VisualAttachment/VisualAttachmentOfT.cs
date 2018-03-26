using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Standard <see cref="IAttachable"/> implementation that can be attached only to objects that derive from <typeparamref name="T"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class VisualAttachment<T> : VisualAttachment
		where T : DependencyObject
	{
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="LockAttachedTo">If true, attaching to a new <see cref="DependencyObject"/> will be possible
		/// only if the <see cref="AttachedTo"/> is currently null</param>
		public VisualAttachment(bool lockAttachedTo = true) : base(lockAttachedTo) { }

		#endregion

		#region Public Properties

		/// <summary>
		/// Getter to the <see cref="T"/> the <see cref="IAttachable"/> is attached to.
		/// </summary>
		public new T AttachedTo { get; private set; }

		#endregion

		#region Public Methods

		/// <summary>
		/// Before attaching checks if <paramref name="obj"/> is of type <typeparamref name="T"/>
		/// </summary>
		/// <param name="obj"></param>
		public override void Attach(DependencyObject obj)
		{
			if (obj == null || obj is T)
			{
				base.Attach(obj);
			}
			else
			{
				throw new ArgumentException("The attached object has to derive from " + typeof(T).ToString());
			}
		}

		#endregion
	}
}
