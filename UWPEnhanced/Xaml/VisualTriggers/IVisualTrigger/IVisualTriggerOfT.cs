using System;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Interface for triggers used in xaml
	/// </summary>
	public interface IVisualTrigger<T> : IAttachable
	{
		#region Events

		/// <summary>
		/// Event fired when the trigger is triggered
		/// </summary>
		EventHandler<T> Triggered { get; set; }

		#endregion
	}
}