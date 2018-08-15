using System;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// General interface with <see cref="object"/> event args
	/// </summary>
	public interface IVisualTrigger : IAttachable
	{
		#region Events

		/// <summary>
		/// Notifies when the trigger is fired
		/// </summary>
		EventHandler<object> Triggered { get; set; }

		#endregion
	}
}