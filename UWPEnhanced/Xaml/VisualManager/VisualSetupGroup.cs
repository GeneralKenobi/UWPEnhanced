using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Class which handles <see cref="VisualSetup"/>s associated with a given <see cref="DependencyObject"/>
	/// </summary>
	public class VisualSetupGroup : DependencyObjectCollectionOfT<VisualSetup>
	{
		#region Private Members

		/// <summary>
		/// State the group is currently in, null if it's not in any state
		/// </summary>
		private VisualSetup _CurrentSetup = null;

		#endregion

		#region Public Properties

		/// <summary>
		/// The current state in the <see cref="VisualStateGroup"/>. <see cref="string.Empty"/> if no state is set
		/// </summary>
		public string CurrentSetup => _CurrentSetup == null ? string.Empty : _CurrentSetup.Name;

		#endregion

		#region Protected Methods

		/// <summary>
		/// Checks the new item and adds it to the visual group
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override VisualSetup NewElementCheckRoutine(DependencyObject item)
		{
			var checkedItem = base.NewElementCheckRoutine(item);

			return checkedItem;
		}

		/// <summary>
		/// Removes the item from the <see cref="_AssociatedVisualStateGroup"/> and detatches it
		/// </summary>
		/// <param name="item"></param>
		protected override void CleanupRoutine(VisualSetup item)
		{
			base.CleanupRoutine(item);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Goes to the <see cref="VisualSetup"/> with the given name. Returns a <see cref="Task"/> that will complete
		/// when the transition is finished. The result will be true if the transition was successful, false otherwise.
		/// If the <see cref="VisualSetupGroup"/> is already in that state the result will be false and no changes will be done.
		/// </summary>
		/// <param name="setup">Name of the setup to go to</param>
		/// <param name="useTransitions">If true, the defined storyboard animations will be used in the transitions</param>
		/// <returns><see cref="Task"/> that will complete when the transition is finished.
		/// The result will be true if the transition was successful, false otherwise</returns>
		public Task<bool> GoToSetup(string setup, bool useTransitions = true)
		{
			TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();

			if (setup == CurrentSetup)
			{
				task.SetResult(true);
			}
			else
			{
				// Run a background task
				Task.Run(() =>
				{
				// Try to find the setup
				VisualSetup designatedSetup = FindSetup(setup);

				// If it was found
				if (designatedSetup != null)
					{
					// Perform the transition using helper method
					Transition(designatedSetup, useTransitions);

						task.SetResult(true);
					}
					else
					{
						task.SetResult(false);
					}
				});
			}
			
			return task.Task;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Transitions to the given <see cref="VisualSetup"/>. Caller should ensure that the <see cref="VisualSetup"/> is in
		/// this <see cref="VisualSetupGroup"/>. Blocks execution until the transitions have completed
		/// </summary>
		/// <param name="setup">Setup to go to</param>
		/// <param name="useTransitions">If true, use the defined storyboard animations in the transitions</param>
		/// <exception cref="ArgumentNullException"/>
		private void Transition(VisualSetup setup, bool useTransitions = true)
		{

#pragma warning disable IDE0016 // Use 'throw' expression. Note: Don't simplify because before assigning to _CurrentSetup,
			// where the check would occur, there are action taken which shouldn't be taken in case setup is null
			if(setup == null)
			{
				throw new ArgumentNullException(nameof(setup));
			}
#pragma warning restore IDE0016 // Use 'throw' expression

			// If this group was in a state
			if (_CurrentSetup != null)
			{
				// Transition out of it
				_CurrentSetup.TransitionOut(useTransitions).Wait();
			}

			// Assign the new setup
			_CurrentSetup = setup;
			
			// Perform the transitions
			_CurrentSetup.TransitionIn(useTransitions).Wait();			
		}
		
		/// <summary>
		/// Searches for a <see cref="VisualSetup"/> with the specified name.
		/// Returns null if the <see cref="VisualSetup"/> wasn't found
		/// </summary>
		/// <param name="name">Name of the <see cref="VisualSetup"/> to look for</param>
		/// <returns>The <see cref="VisualSetup"/>, null if it wasn't found</returns>
		private VisualSetup FindSetup(string name)
		{
			VisualSetup result = null;

			Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				foreach (var item in _ControlArchive)
				{
					if (item.Name == name)
					{
						result = item;
						break;
					}
				}
			}).AsTask().Wait();

			return result;
		}

		#endregion
	}
}
