using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Interface which defines a VisualAttachment for <see cref="VisualAttachments"/>
	/// </summary>
	public interface IVisualAttachment
	{
		/// <summary>
		/// Getter to the <see cref="DependencyObject"/> the <see cref="IVisualAttachment"/> is attached to
		/// </summary>
		DependencyObject AttachedTo { get; }

		/// <summary>
		/// Attaches this <see cref="IVisualAttachment"/> to the specified <see cref="DependencyObject"/>
		/// </summary>
		/// <param name="obj"></param>
		void Attach(DependencyObject obj);

		/// <summary>
		/// Detaches this <see cref="IVisualAttachment"/> from the <see cref="DependencyObject"/> it is attached to
		/// (if it's attached to begin with)
		/// </summary>
		void Detach();
	}
}
