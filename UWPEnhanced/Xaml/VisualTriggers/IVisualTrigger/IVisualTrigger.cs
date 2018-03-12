using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Interface for triggers used in xaml
	/// </summary>
	public interface IVisualTrigger : IAttachable
	{
		/// <summary>
		/// Event fired when the trigger is triggered
		/// </summary>
		EventHandler Triggered { get; set; }
	}
}