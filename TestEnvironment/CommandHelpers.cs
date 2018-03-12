using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestEnvironment
{
    public static class CommandHelpers
    {
		#region Run Command

		/// <summary>
		/// Runs the command if the flag is not set
		/// </summary>
		/// <param name="updatingFlag">If set to true, command won't be run</param>
		/// <param name="action">Command to run</param>
		/// <returns></returns>
		public static async Task RunCommand(this Func<Task> action, Expression<Func<bool>> updatingFlag)
		{
			//// Check if the flag is set (function is already running)
			//if (updatingFlag.GetPropertyValue())
			//{
			//	return;
			//}

			//// Set flag to true to indicate that we are running
			//updatingFlag.SetPropertyValue(true);

			//try
			//{
				// Run the action
				await action();
			//}
			//finally
			//{
			//	// Set the property flag back to false after finishing
			//	updatingFlag.SetPropertyValue(false);
			//}
		}

		#endregion

	}
}
