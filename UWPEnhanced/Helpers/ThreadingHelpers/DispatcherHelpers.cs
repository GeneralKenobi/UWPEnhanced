using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace UWPEnhanced.Helpers
{
	/// <summary>
	/// Class containing helper methods related to
	/// <see cref="Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher"/>
	/// </summary>
	public static class DispatcherHelpers
	{
		/// <summary>
		/// Runs the <see cref="DispatchedHandler"/> and waits (blocks execution) until it completes.
		/// </summary>
		/// <param name="dispatchedHandler">Action to perform</param>
		/// <param name="priority">Priority to assign to it</param>
		public static void Run(DispatchedHandler dispatchedHandler,
			CoreDispatcherPriority priority = CoreDispatcherPriority.Normal) =>
			RunAsync(dispatchedHandler, priority).Wait();
			
		/// <summary>
		/// Runs the <see cref="DispatchedHandler"/> and returns a task that will complete when the action has completed.
		/// </summary>
		/// <param name="dispatchedHandler">Action to perform</param>
		/// <param name="priority">Priority to assign to it</param>
		public static Task RunAsync(DispatchedHandler dispatchedHandler,
			CoreDispatcherPriority priority = CoreDispatcherPriority.Normal) =>		
			Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
				priority, dispatchedHandler).AsTask();
	}
}
