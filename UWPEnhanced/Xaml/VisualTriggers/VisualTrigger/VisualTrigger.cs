using System;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Standard implementation of <see cref="IVisualTrigger"/>
	/// </summary>
	public class VisualTrigger : VisualAttachment, IVisualTrigger
	{
		/// <summary>
		/// Notifies when the trigger is fired
		/// </summary>
		public EventHandler<object> Triggered { get; set; }
	}
}