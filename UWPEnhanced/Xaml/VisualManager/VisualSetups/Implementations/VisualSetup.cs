using CSharpEnhanced.Synchronization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UWPEnhanced.Helpers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// A single setup for <see cref="VisualManager"/>. Defines transition in/out animations, setters, temporary setters.
	/// Acts as an expanded <see cref="VisualState"/>
	/// </summary>
    public class VisualSetup : VisualSetupBase
    {
		#region Protected Members

		/// <summary>
		/// Semaphore used to ensure only one Transition call is operating on the setters and storyboards
		/// </summary>
		protected readonly SemaphoreSlimFIFO _TransitionSemaphore = new SemaphoreSlimFIFO(1,1);

		/// <summary>
		/// AutoResetEvent that can be used to wait for a storyboard to finish (since storyboards won't be played simultaneously
		/// by definition, only one is enough to serve both storyboards)
		/// </summary>
		protected readonly AutoResetEvent _WaitForStoryboardToFinish = new AutoResetEvent(false);

		#endregion

		#region Protected Methods

		protected override void SignalStoryboardCompleted(object sender, object e) => _WaitForStoryboardToFinish.Set();

		#endregion

		#region Public Methods	

		/// <summary>
		/// Transitions into the state. Calls to <see cref="TransitionIn"/> and <see cref="TransitionOut"/> are synchronized
		/// and only one will be executed at a time, the rest will be blocked and executed in FIFO order.
		/// </summary>
		/// <paramref name="useTransitions">If true, use defined storyboard animations in the transition</paramref>
		public override async Task TransitionIn(VisualTransitionType type, bool useTransitions = true)
		{
			// If it's a transition to self and the transition in should be omitted return
			if(type == VisualTransitionType.ToTheSameSetup &&
				!(await GetRepeatedTransition()).HasFlag(RepeatedTransitionBehavior.TransitionIn))
			{
				return;
			}

			// Get into the semaphore
			await _TransitionSemaphore.WaitAsync();		
		
			if (useTransitions)
			{
				bool sbDefined = false;

				// Get on the UI thread
				await DispatcherHelpers.RunAsync(() =>
				{
					// If the storyboard is defined
					if (TransitionInStoryboard.WillComplete())
					{
						sbDefined = true;
						
						// Run it
						TransitionInStoryboard?.Begin();
					}
				});

				// If the UI thread task determined that storyboard was defined, start a task that will wait for the storyboard to finish
				if (sbDefined)
				{
					await Task.Run(() => _WaitForStoryboardToFinish.WaitOne());
				}
			}

			// After the storyboard finished apply the setters
			DispatcherHelpers.RunAsync(() =>
			{
				TemporarySetters?.ForEach((x) => x.Set());
				Setters?.ForEach((x) => x.Set());
			});

			// Transition finished; Release the semaphore
			_TransitionSemaphore.Release();
		}
		
		/// <summary>
		/// Transitions out of the state. Calls to <see cref="TransitionIn"/> and <see cref="TransitionOut"/> are synchronized
		/// and only one will be executed at a time, the rest will be blocked and executed in FIFO order.
		/// </summary>
		/// <paramref name="useTransitions">If true, use defined storyboard animations in the transition</paramref>
		public override async Task TransitionOut(VisualTransitionType type, bool useTransitions = true)
		{
			// If it's a transition to self and the transition out should be omitted return
			if (type == VisualTransitionType.ToTheSameSetup &&
				!(await GetRepeatedTransition()).HasFlag(RepeatedTransitionBehavior.TransitionOut))
			{
				return;
			}

			// Get into the semaphore
			await _TransitionSemaphore.WaitAsync();

			// Reset the temporary setters
			DispatcherHelpers.RunAsync(() => TemporarySetters.ForEach((x) => x.Reset()));

			if (useTransitions)
			{
				bool sbDefined = false;

				// Get on the UI thread
				await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,() =>
				{
					// If the storyboard is defined
					if (TransitionOutStoryboard.WillComplete())
					{
						sbDefined = true;
												
						// Run it
						TransitionOutStoryboard?.Begin();
					}
				});

				// If the UI thread task determined that storyboard was defined, start a task that will wait for the storyboard to finish
				if (sbDefined)
				{
					await Task.Run(() => _WaitForStoryboardToFinish.WaitOne());					
				}
			}
			
			// Transition finished; Release the semaphore
			_TransitionSemaphore.Release();			
		}

		#endregion
	}
}