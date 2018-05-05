using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UWPEnhanced.Helpers;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Class which handles <see cref="VisualSetup"/>s associated with a given <see cref="DependencyObject"/>
	/// </summary>
	public class VisualSetupGroup : DependencyObjectCollectionOfT<IVisualSetup>
	{
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public VisualSetupGroup()
		{
			Triggers = new VisualTransitionCollection();
		}

		#endregion

		#region Private Members

		/// <summary>
		/// State the group is currently in, null if it's not in any state
		/// </summary>
		private IVisualSetup _CurrentSetup = null;

		#endregion

		#region Name Dependency Property

		/// <summary>
		/// Name assigned to identify the visual group; Has to be unique with respect to other groups of the <see cref="FrameworkElement"/>
		/// </summary>
		public string Name
		{
			get => (string)GetValue(NameProperty);
			set => SetValue(NameProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Name"/>
		/// </summary>
		public static readonly DependencyProperty NameProperty =
			DependencyProperty.Register(nameof(Name), typeof(string),
			typeof(VisualSetupGroup), new PropertyMetadata(string.Empty));

		#endregion

		#region Triggers Dependency Property

		/// <summary>
		/// Collection of triggers for this group
		/// </summary>
		public VisualTransitionCollection Triggers
		{
			get => (VisualTransitionCollection)GetValue(TriggersProperty);
			set => SetValue(TriggersProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Triggers"/>
		/// </summary>
		public static readonly DependencyProperty TriggersProperty =
			DependencyProperty.Register(nameof(Triggers), typeof(VisualTransitionCollection),
			typeof(VisualSetupGroup), new PropertyMetadata(null, new PropertyChangedCallback(TriggersChanged)));

		#endregion

		#region Public Properties

		/// <summary>
		/// The current state in the <see cref="VisualStateGroup"/>. <see cref="string.Empty"/> if no state is set
		/// </summary>
		public string CurrentSetup => _CurrentSetup == null ? string.Empty : _CurrentSetup.ID;

		#endregion

		#region Protected Methods

		/// <summary>
		/// Checks the new item and adds it to the visual group
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override IVisualSetup NewElementCheckRoutine(DependencyObject item)
		{
			var checkedItem = base.NewElementCheckRoutine(item);

			checkedItem.AssignParent(this);

			return checkedItem;
		}

		/// <summary>
		/// Removes the item from the <see cref="_AssociatedVisualStateGroup"/> and detatches it
		/// </summary>
		/// <param name="item"></param>
		protected override void CleanupRoutine(IVisualSetup item)
		{
			item.RemoveParent();
			base.CleanupRoutine(item);
		}

		#endregion

		#region Private Static Methods

		/// <summary>
		/// Callback for changes in <see cref="TriggersProperty"/>. Detatches old collections and attaches new ones
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private static void TriggersChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is VisualSetupGroup group)
			{
				if(args.OldValue is VisualTransitionCollection oldCollection)
				{
					oldCollection.TransitionTriggered -= group.TriggerCallback;
				}

				if (args.NewValue is VisualTransitionCollection newCollection)
				{
					newCollection.TransitionTriggered += group.TriggerCallback;
				}
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Goes to the <see cref="VisualSetupBase"/> with the given name. Returns a <see cref="Task"/> that will complete
		/// when the transition is finished. The result will be true if the transition was successful, false otherwise.
		/// If the <see cref="VisualSetupGroup"/> is already in that state the result will be false and no changes will be done.
		/// </summary>
		/// <param name="setup">Name of the setup to go to. Passing null, <see cref="string.Empty"/> or whitespace will trigger
		/// transition out of the current state and no transition into a new state</param>
		/// <param name="useTransitions">If true, the defined storyboard animations will be used in the transitions</param>
		/// <returns><see cref="Task"/> that will complete when the transition is finished.
		/// The result will be true if the transition was successful (either: to the current state, out of the current state
		/// or from current state to any other state in this <see cref="VisualSetupGroup"/>, false otherwise</returns>
		public async Task<bool> GoToSetup(string setup, bool useTransitions = true)
		{
			IVisualSetup designatedSetup = null;

			// If the new setup is null or whitespace then it's simply a transition out of current state so designatedSetup
			// remains null, if not try to find the new setup, if successfull make the transition
			if (string.IsNullOrWhiteSpace(setup) || TryFindSetup(setup, out designatedSetup))
			{
				// Perform the transition using helper method
				await Transition(designatedSetup, useTransitions);
				return true;
			}
			else
			{
				return false;
			}
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

		private void TriggerCallback(object sender, VisualTransitionTriggeredEventArgs e) => GoToSetup(e.TransitionTo, e.UseTransitions);

		/// <summary>
		/// Transitions to the given <see cref="IVisualSetup"/>. Caller should ensure that the <see cref="VisualSetupBase"/> is in
		/// this <see cref="VisualSetupGroup"/>. Blocks execution until the transitions have completed.
		/// </summary>
		/// <param name="setup">Setup to go to. If null, the old state will be transitioned out and the group won't be in any state</param>
		/// <param name="useTransitions">If true, use the defined storyboard animations in the transitions</param>
		private async Task Transition(IVisualSetup setup, bool useTransitions = true)
		{
			// Remember the old setup
			var temp = _CurrentSetup;

			// Assign the new setup
			_CurrentSetup = setup;

			// If this group was in a setup, transition out of it
			if (temp != null)
			{
				// Transition out of it
				await temp.TransitionOut(temp == setup ? VisualTransitionType.ToTheSameSetup : VisualTransitionType.BetweenSetups, useTransitions);
			}
			
			// If the transition ends in a new setup
			if (_CurrentSetup != null)
			{
				// Perform the transitions
				await _CurrentSetup.TransitionIn(temp == setup ? VisualTransitionType.ToTheSameSetup : VisualTransitionType.BetweenSetups, useTransitions);
			}
		}

		/// <summary>
		/// Searches for a <see cref="IVisualSetup"/> with the specified name.
		/// Returns true if the <see cref="IVisualSetup"/> was found
		/// </summary>
		/// <param name="name">Name of the <see cref="IVisualSetup"/> to look for</param>
		/// <param name="setup">Parameter to assign the setup to</param>
		/// <returns>True if the <see cref="IVisualSetup"/> was found</returns>
		private bool TryFindSetup(string name, out IVisualSetup setup)
		{
			// Variable to keep the result in, null by default
			IVisualSetup result = null;
			
			foreach (var item in _ControlArchive)
			{
				if (item.ID == name)
				{
					result = item;
					break;
				}
			}			

			// Assign the result to out parameter
			setup = result;

			// If the result was found (it won't equal to null) return true
			return setup != null;
		}
				
		#endregion
	}
}