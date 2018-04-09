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
		protected readonly SemaphoreSlimFIFO _StoryboardSemaphore = new SemaphoreSlimFIFO(0,1);


		private int _CurrentCaller = 0;

		protected override void SignalStoryboardCompleted(object sender, object e) => _StoryboardSemaphore.Release();


		public override async Task TransitionIn(bool useTransitions = true)
		{
			Stopwatch s = new Stopwatch();
			s.Start();
			int id = _CurrentCaller++;
			cc.Cancel();
			c.Dispose();
			c = new CancellationTokenSource();
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
					await Task.Factory.StartNew(() =>
					{
						Task.Run(() => _StoryboardSemaphore.Wait(), c.Token).ContinueWith((task) =>
						{
							DispatcherHelpers.RunAsync(() =>
							{
								TemporarySetters?.ForEach((x) => x.Set());
								Setters?.ForEach((x) => x.Set());
							});
						}, TaskContinuationOptions.NotOnCanceled).Wait();
					}, c.Token);
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
		}
		private CancellationTokenSource c = new CancellationTokenSource();
		private CancellationTokenSource cc = new CancellationTokenSource();
		public override async Task TransitionOut(bool useTransitions = true)
		{
			Stopwatch s = new Stopwatch();
			s.Start();
			++_CurrentCaller;
			c.Cancel();
			cc.Dispose();
			cc = new CancellationTokenSource();
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
					await Task.Factory.StartNew(() =>
					{
						Task.Run(() => _StoryboardSemaphore.Wait(), cc.Token).Wait();
					}, cc.Token);
					//await Task.Run(() => _WaitForStoryboardToFinish.WaitOne());
					//await Task.Run(() => _StoryboardSemaphore.Wait());
					//_StoryboardSemaphore.Wait();
				}
			}
		}
	}
}
