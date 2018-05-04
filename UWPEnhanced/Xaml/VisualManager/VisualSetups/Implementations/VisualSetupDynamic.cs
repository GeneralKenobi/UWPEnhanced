using CSharpEnhanced.Synchronization;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UWPEnhanced.Helpers;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Visual setup that will cancel transition in/out upon transition out/in is requested
	/// </summary>
	public class VisualSetupDynamic : VisualSetupBase
	{
		#region Private Members

		/// <summary>
		/// Marks the direction of the transition. True for transition in, false for transition out.
		/// </summary>
		private bool _TransitionDirection = false;

		/// <summary>
		/// Event fired when <see cref="TransitionIn(bool)"/> or <see cref="TransitionOut(bool)"/> storyboard finishes
		/// </summary>
		protected readonly AutoResetEvent _WaitForStoryboard = new AutoResetEvent(false);

		/// <summary>
		/// Transition methods upon being called assign their <see cref="CancellationTokenSource"/>s which the next caller
		/// will use to signal cancellation.
		/// </summary>
		private CancellationTokenSource _Cancellation = new CancellationTokenSource();

		#endregion		

		#region Protected Methods

		/// <summary>
		/// Method used as callback for for Storyboard.Completed event. Sets the <see cref="_WaitForStoryboard"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void SignalStoryboardCompleted(object sender, object e) => _WaitForStoryboard.Set();

		#endregion

		#region Public Methods

		/// <summary>
		/// Transitions into the state. Cancels transition out operation (if it was happening).
		/// </summary>
		/// <param name="useTransitions"></param>
		/// <returns></returns>
		public override async Task TransitionIn(VisualTransitionType type, bool useTransitions = true)
		{
			// If there's already a transition in going on and we're not supposed to restart it, just return
			if (type == VisualTransitionType.ToTheSameSetup &&
				!(await GetRepeatedTransition()).HasFlag(RepeatedTransitionBehavior.TransitionIn))
			{
				return;
			}

			// If the CancellationTokenSource from the previous transition call is still alive cancel it
			_Cancellation?.Cancel();

			// Create a new source for ourselves
			var cancellation = new CancellationTokenSource();

			// And assign it to the class-wide variable
			_Cancellation = cancellation;

			if (useTransitions)
			{
				bool sbDefined = false;

				// Get on the UI thread
				await DispatcherHelpers.RunAsync(() =>
				{
					// If the storyboard is defined and has at least one Timeline object
					if (TransitionInStoryboard != null && TransitionInStoryboard.Children.Count > 0)
					{
						// Mark it for the rest of the Task
						sbDefined = true;

						// Pause the transition out storyboard
						TransitionOutStoryboard.Pause();
						// Run the transition in storyboard
						TransitionInStoryboard?.Begin();
					}
				});

				// If the UI thread task determined that storyboard was defined,
				// start a task that will wait for the storyboard to finish
				if (sbDefined)
				{
					// Wait for either for the storyboard to finish or for the CancellationTokenSource to be cancelled
					// and if the storyboard finished first apply the setters on UI thread
					//if (WaitHandle.WaitAny(new[] { cancellation.Token.WaitHandle, _WaitForStoryboard }) == 1)
					if(await Task.Run(() => WaitHandle.WaitAny(new[] { cancellation.Token.WaitHandle, _WaitForStoryboard }) == 1))
					{
						// If we weren't cancelled remove the reference to the _Cancellation, otherwise the one cancelling
						// will provide their own CancellationTokenSource therefore removing the reference to ours
						_Cancellation = null;

						await DispatcherHelpers.RunAsync(() =>
						{
							TemporarySetters?.ForEach((x) => x.Set());
							Setters?.ForEach((x) => x.Set());
						});
					}
				}
			}

			cancellation.Dispose();
		}

		/// <summary>
		/// Transitions out of the state. Cancels transition in operation (if it was happening).
		/// </summary>
		/// <param name="useTransitions"></param>
		/// <returns></returns>
		public override async Task TransitionOut(VisualTransitionType type, bool useTransitions = true)
		{
			// If there's already a transition out going on and we're not supposed to restart it, just return
			if (type == VisualTransitionType.ToTheSameSetup &&
				!(await GetRepeatedTransition()).HasFlag(RepeatedTransitionBehavior.TransitionOut))
			{
				return;
			}
			
			// If the CancellationTokenSource from the previous transition call is still alive cancel it
			_Cancellation?.Cancel();

			// Create a new source for ourselves
			var cancellation = new CancellationTokenSource();

			// And assign it to the class-wide variable
			_Cancellation = cancellation;

			// Reset the temporary setters
			await DispatcherHelpers.RunAsync(() => TemporarySetters.ForEach((x) => x.Reset()));

			if (useTransitions)
			{
				bool sbDefined = false;

				// Get on the UI thread
				await DispatcherHelpers.RunAsync(() =>
				{
					// If the storyboard is defined and has at least one Timeline object
					if (TransitionOutStoryboard != null && TransitionOutStoryboard.Children.Count > 0)
					{
						// signal it
						sbDefined = true;

						// Pause the transition in storyboard
						TransitionInStoryboard.Pause();
						// Run the transition out storyboard
						TransitionOutStoryboard.Begin();
					}
				});

				// If the UI thread task determined that storyboard was defined, start a task that will wait for the storyboard to finish
				if (sbDefined)
				{
					// Wait either for cancellation or storyboard to finish so that this Task doesn't end before the operations
					// are completed
					if(await Task.Run(() => WaitHandle.WaitAny(new[] { cancellation.Token.WaitHandle, _WaitForStoryboard }) == 1))
					{
						// If we weren't cancelled remove the reference to the _Cancellation, otherwise the one cancelling
						// will provide their own CancellationTokenSource therefore removing the reference to ours
						_Cancellation = null;
					}
				}
			}

			cancellation.Dispose();
		}

		#endregion
	}
}
