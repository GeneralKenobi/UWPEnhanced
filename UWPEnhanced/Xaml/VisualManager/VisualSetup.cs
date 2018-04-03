﻿using CSharpEnhanced.Synchronization;
using System;
using System.Collections.Generic;
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
    public class VisualSetup : DependencyObject
    {
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public VisualSetup()
		{
			Setters = new DependencyObjectCollectionOfT<IVisualSetter>();
			TemporarySetters = new DependencyObjectCollectionOfT<ITemporaryVisualSetter>();			
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Semaphore used to ensure only one Transition call is operating on the setters and storyboards
		/// </summary>
		private readonly SemaphoreSlimFIFO _TransitionSemaphore = new SemaphoreSlimFIFO(1,1);

		/// <summary>
		/// AutoResetEvent that can be used to wait for a storyboard to finish (since storyboards won't be played simultaneously
		/// by definition, only one is enough to serve both storyboards)
		/// </summary>
		private readonly AutoResetEvent _WaitForStoryboardToFinish = new AutoResetEvent(false);

		#endregion

		#region Name Dependency Property

		/// <summary>
		/// Name of this setup
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
			typeof(VisualSetup), new PropertyMetadata(default(string)));

		#endregion

		#region Setters Dependency Property

		/// <summary>
		/// Permanent setters in this instance
		/// </summary>
		public DependencyObjectCollectionOfT<IVisualSetter> Setters
		{
			get => (DependencyObjectCollectionOfT<IVisualSetter>)GetValue(SettersProperty);
			set => SetValue(SettersProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Setters"/>
		/// </summary>
		public static readonly DependencyProperty SettersProperty =
			DependencyProperty.Register(nameof(Setters), typeof(DependencyObjectCollectionOfT<IVisualSetter>),
			typeof(VisualSetup), new PropertyMetadata(null));

		#endregion

		#region TemporarySetters Dependency Property

		/// <summary>
		/// Temporary setters that will be reset when this visual setup is no longer the current one
		/// </summary>
		public DependencyObjectCollectionOfT<ITemporaryVisualSetter> TemporarySetters
		{
			get => (DependencyObjectCollectionOfT<ITemporaryVisualSetter>)GetValue(TemporarySettersProperty);
			set => SetValue(TemporarySettersProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="TemporarySetters"/>
		/// </summary>
		public static readonly DependencyProperty TemporarySettersProperty =
			DependencyProperty.Register(nameof(TemporarySetters), typeof(DependencyObjectCollectionOfT<ITemporaryVisualSetter>),
			typeof(VisualSetup), new PropertyMetadata(null));

		#endregion

		#region TransitionInStoryboard Dependency Property

		/// <summary>
		/// Storyboard ran when transitioning into the state
		/// </summary>
		public Storyboard TransitionInStoryboard
		{
			get => (Storyboard)GetValue(TransitionInStoryboardProperty);
			set => SetValue(TransitionInStoryboardProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="TransitionInStoryboard"/>
		/// </summary>
		public static readonly DependencyProperty TransitionInStoryboardProperty =
			DependencyProperty.Register(nameof(TransitionInStoryboard), typeof(Storyboard),
			typeof(VisualSetup), new PropertyMetadata(null, new PropertyChangedCallback(StoryboardChanged)));

		#endregion

		#region TransitionOutStoryboard Dependency Property

		/// <summary>
		/// Storyboard ran when transitioning out of the state
		/// </summary>
		public Storyboard TransitionOutStoryboard
		{
			get => (Storyboard)GetValue(TransitionOutStoryboardProperty);
			set => SetValue(TransitionOutStoryboardProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="TransitionOutStoryboard"/>
		/// </summary>
		public static readonly DependencyProperty TransitionOutStoryboardProperty =
			DependencyProperty.Register(nameof(TransitionOutStoryboard), typeof(Storyboard),
			typeof(VisualSetup), new PropertyMetadata(null, new PropertyChangedCallback(StoryboardChanged)));

		#endregion

		#region Private Static Methods

		/// <summary>
		/// Callback for <see cref="TransitionInStoryboard"/> and <see cref="TransitionOutStoryboard"/>.
		/// Subscribes to Storyboard.Completed event on on new storyboards and unsubscribes from it on old storyboards
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void StoryboardChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if(sender is VisualSetup setup && e.NewValue != e.OldValue)
			{
				if (e.OldValue is Storyboard oldSb)
				{
					oldSb.Completed -= setup.SignalStoryboardCompleted;
				}

				if (e.NewValue is Storyboard newSb)
				{
					newSb.Completed += setup.SignalStoryboardCompleted;
				}
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Method used to subscribe to storyboards. It will Set the <see cref="_WaitForStoryboardToFinish"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SignalStoryboardCompleted(object sender, object e) => _WaitForStoryboardToFinish.Set();

		#endregion

		#region Public Methods

		/// <summary>
		/// Transitions into the state. Calls to <see cref="TransitionIn"/> and <see cref="TransitionOut"/> are synchronized
		/// and only one will be executed at a time, the rest will be blocked and executed in FIFO order.
		/// </summary>
		/// <paramref name="useTransitions">If true, use defined storyboard animations in the transition</paramref>
		public async Task TransitionIn(bool useTransitions = true)
		{
			// Get into the semaphore
			await _TransitionSemaphore.WaitAsync();

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
			await DispatcherHelpers.RunAsync(() =>
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
		public async Task TransitionOut(bool useTransitions = true)
		{
			// Get into the semaphore
			await _TransitionSemaphore.WaitAsync();

			// Reset the temporary setters
			await DispatcherHelpers.RunAsync(() => TemporarySetters.ForEach((x) => x.Reset()));

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