using CSharpEnhanced.Synchronization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UWPEnhanced.Helpers;

namespace UWPEnhanced.Xaml
{
	public class VisualSetupDynamic : VisualSetupBase
	{
		protected readonly AutoResetEvent _WaitForStoryboard = new AutoResetEvent(false);


		private int _CurrentCaller = 0;

		protected override void SignalStoryboardCompleted(object sender, object e) => _WaitForStoryboard.Set();


		public override async Task TransitionIn(bool useTransitions = true)
		{
			Stopwatch s = new Stopwatch();
			s.Start();
			int id = _CurrentCaller++;
			_Cancellation.Cancel();
			//_Cancellation.Dispose();
			var cancellation = new CancellationTokenSource();
			_Cancellation = cancellation;

			if (useTransitions)
			{
				bool sbDefined = false;

				// Get on the UI thread
				await DispatcherHelpers.RunAsync(() =>
				{
					// If the storyboard is defined
					if (TransitionInStoryboard != null)
					{
						sbDefined = true;

						// Run it						
						TransitionOutStoryboard.Pause();
						TransitionInStoryboard?.Begin();
						s.Stop();
						Debug.WriteLine($"Sb started: {s.ElapsedMilliseconds}");
					}
				});

				// If the UI thread task determined that storyboard was defined, start a task that will wait for the storyboard to finish
				if (sbDefined)
				{
					WaitHandle.WaitAny(new[] { cancellation.Token.WaitHandle, _WaitForStoryboard });

					if(cancellation.IsCancellationRequested)
					{
						DispatcherHelpers.RunAsync(() =>
						{
							TemporarySetters?.ForEach((x) => x.Set());
							Setters?.ForEach((x) => x.Set());
						});
					}
					//_StoryboardSemaphore.Wait();
					//await Task.Run(() => _StoryboardSemaphore.Wait());
				}
			}

			if (_CurrentCaller == id)
			{
				//// After the storyboard finished apply the setters
				//DispatcherHelpers.RunAsync(() =>
				//{
				//	TemporarySetters?.ForEach((x) => x.Set());
				//	Setters?.ForEach((x) => x.Set());
				//});
				
			}
			cancellation.Dispose();
		}
		private CancellationTokenSource _Cancellation = new CancellationTokenSource();

		public override async Task TransitionOut(bool useTransitions = true)
		{
			Stopwatch s = new Stopwatch();
			s.Start();
			++_CurrentCaller;
			var cancellation = new CancellationTokenSource();
			_Cancellation.Cancel();
			//_Cancellation.Dispose();
			_Cancellation = cancellation;

			// Reset the temporary setters
			DispatcherHelpers.RunAsync(() => TemporarySetters.ForEach((x) => x.Reset()));

			if (useTransitions)
			{
				bool sbDefined = false;

				// Get on the UI thread
				await DispatcherHelpers.RunAsync(() =>
				{
					// If the storyboard is defined
					if (TransitionOutStoryboard != null)
					{
						sbDefined = true;

						// Run it
						//TransitionOutStoryboard?.Begin();
						TransitionInStoryboard.Pause();
						TransitionOutStoryboard.Begin();
						s.Stop();
						Debug.WriteLine($"Sb started: {s.ElapsedMilliseconds}");
					}
				});

				// If the UI thread task determined that storyboard was defined, start a task that will wait for the storyboard to finish
				if (sbDefined)
				{
					WaitHandle.WaitAny(new[] { cancellation.Token.WaitHandle, _WaitForStoryboard });					
				}
			}

			cancellation.Dispose();
		}
	}
}
