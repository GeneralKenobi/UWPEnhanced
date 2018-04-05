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
		/// <param name="setup">Name of the setup to go to. Passing null, <see cref="string.Empty"/> or whitespace will trigger
		/// transition out of the current state and no transition into a new state</param>
		/// <param name="useTransitions">If true, the defined storyboard animations will be used in the transitions</param>
		/// <returns><see cref="Task"/> that will complete when the transition is finished.
		/// The result will be true if the transition was successful (either: to the current state, out of the current state
		/// or from current state to any other state in this <see cref="VisualSetupGroup"/>, false otherwise</returns>
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
					VisualSetup designatedSetup = null;

					// If the new setup is null or whitespace then it's simply a transition out of current state so designatedSetup
					// remains null, if not try to find the new setup, if successfull make the transition
					if (string.IsNullOrWhiteSpace(setup) || TryFindSetup(setup, out designatedSetup))
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

		/// <summary>
		/// Leaves the current setup. Equivalent to calling <see cref="GoToSetup(string, bool)"/> with setup name null,
		/// <see cref="string.Empty"/> or whitespace.
		/// </summary>
		/// <param name="useTransitions">If true, the defined storyboard animations will be used in the transitions</param>
		/// <returns><see cref="Task"/> that will complete when the transition is finished.
		/// The result will be true if the transition was successful, false otherwise</returns>
		public Task<bool> LeaveSetup(bool useTransitions = true) => GoToSetup(string.Empty, useTransitions);

		#endregion

		#region Private Methods

		/// <summary>
		/// Transitions to the given <see cref="VisualSetup"/>. Caller should ensure that the <see cref="VisualSetup"/> is in
		/// this <see cref="VisualSetupGroup"/>. Blocks execution until the transitions have completed.
		/// </summary>
		/// <param name="setup">Setup to go to. If null, the old state will be transitioned out and the group won't be in any state</param>
		/// <param name="useTransitions">If true, use the defined storyboard animations in the transitions</param>
		private void Transition(VisualSetup setup, bool useTransitions = true)
		{
			// If this group was in a state
			if (_CurrentSetup != null)
			{
				// Transition out of it
				_CurrentSetup.TransitionOut(useTransitions).Wait();
			}

			// Assign the new setup
			_CurrentSetup = setup;

			// If the transition ends in a new state
			if (_CurrentSetup != null)
			{
				// Perform the transitions
				_CurrentSetup.TransitionIn(useTransitions).Wait();
			}
		}
		
		/// <summary>
		/// Searches for a <see cref="VisualSetup"/> with the specified name.
		/// Returns true if the <see cref="VisualSetup"/> was found
		/// </summary>
		/// <param name="name">Name of the <see cref="VisualSetup"/> to look for</param>
		/// <param name="setup">Parameter to assign the setup to</param>
		/// <returns>True if the <see cref="VisualSetup"/> was found</returns>
		private bool TryFindSetup(string name, out VisualSetup setup)
		{
			// Variable to keep the result in, null by default
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

			// Assign the result to out parameter
			setup = result;

			// If the result was found (it won't equal to null) return true
			return setup != null;
		}

		#endregion
	}
}