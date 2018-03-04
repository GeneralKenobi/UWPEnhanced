using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{

	/// <summary>
	/// Container for visual attachments in xaml (like VisualStateManager)
	/// </summary>
	public sealed class VisualAttachments
	{

		#region AttachedVisuals

		/// <summary>
		/// Getter for <see cref="AttachedVisualsProperty"/>
		/// </summary>
		public static int GetAttachedVisuals(DependencyObject obj) => (int)obj.GetValue(AttachedVisualsProperty);

		/// <summary>
		/// Setter for <see cref="AttachedVisualsProperty"/>
		/// </summary>
		public static void SetAttachedVisuals(DependencyObject obj, int value) => obj.SetValue(AttachedVisualsProperty, value);

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty AttachedVisualsProperty =
			DependencyProperty.RegisterAttached("AttachedVisuals", typeof(int),
			 typeof(VisualAttachments), new PropertyMetadata(default(int)));

		#endregion
	}
}
