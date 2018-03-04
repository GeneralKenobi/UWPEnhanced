using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Interface which allows to attach to <see cref="DependencyObject"/>s
	/// </summary>
	public interface IAttachable
	{
		/// <summary>
		/// Getter to the <see cref="DependencyObject"/> the <see cref="IAttachable"/> is attached to
		/// </summary>
		DependencyObject AttachedTo { get; }

		/// <summary>
		/// Attaches this <see cref="IAttachable"/> to the specified <see cref="DependencyObject"/>
		/// </summary>
		/// <param name="obj"></param>
		void Attach(DependencyObject obj);

		/// <summary>
		/// Detaches this <see cref="IAttachable"/> from the <see cref="DependencyObject"/> it is attached to
		/// (if it's attached to begin with)
		/// </summary>
		void Detach();

		/// <summary>
		/// True if this <see cref="IAttachable"/> is attached
		/// </summary>
		bool IsAttached { get; }
	}
}
