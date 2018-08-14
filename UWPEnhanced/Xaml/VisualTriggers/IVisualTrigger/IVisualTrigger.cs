using System;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// General interface not meant for any specific EventArgs type
	/// </summary>
	public interface IVisualTrigger : IVisualTrigger<EventArgs> { }
}