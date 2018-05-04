using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UWPEnhanced.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace UWPEnhanced.Xaml
{
	public abstract class VisualSetupBase : DependencyObject, IVisualSetup
	{
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		protected VisualSetupBase()
		{
			Setters = new DependencyObjectCollectionOfT<IVisualSetter>();
			TemporarySetters = new DependencyObjectCollectionOfT<ITemporaryVisualSetter>();
		}

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
			typeof(VisualSetupBase), new PropertyMetadata(default(string)));

		#endregion

		#region RestartTransition Dependency Property

		/// <summary>
		/// Defines the desired repeated transition behavior for the given instance
		/// </summary>
		public RepeatedTransitionBehavior RepeatedTransition
		{
			get => (RepeatedTransitionBehavior)GetValue(RepeatedTransitionProperty);
			set => SetValue(RepeatedTransitionProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="RepeatedTransition"/>
		/// </summary>
		public static readonly DependencyProperty RepeatedTransitionProperty =
			DependencyProperty.Register(nameof(RepeatedTransition), typeof(RepeatedTransitionBehavior),
				typeof(VisualSetupBase), new PropertyMetadata(RepeatedTransitionBehavior.Skip));

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
			typeof(VisualSetupBase), new PropertyMetadata(null));

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
			typeof(VisualSetupBase), new PropertyMetadata(null));

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
			typeof(VisualSetupBase), new PropertyMetadata(null, new PropertyChangedCallback(StoryboardChanged)));

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
			typeof(VisualSetupBase), new PropertyMetadata(null, new PropertyChangedCallback(StoryboardChanged)));

		#endregion

		#region Protected Methods

		/// <summary>
		/// Returns the value of <see cref="RestartTransition"/> (enters the UI thread)
		/// </summary>
		/// <returns></returns>
		protected async Task<RepeatedTransitionBehavior> GetRepeatedTransition()
		{
			RepeatedTransitionBehavior repeatedTransition = RepeatedTransitionBehavior.Skip;

			// Get on UI thread and get the value
			await DispatcherHelpers.RunAsync(() => repeatedTransition = RepeatedTransition);

			return repeatedTransition;
		}

		/// <summary>
		/// Method used to subscribe to storyboards. It will Set the <see cref="_WaitForStoryboardToFinish"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected abstract void SignalStoryboardCompleted(object sender, object e);

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
			if (sender is VisualSetupBase setup && e.NewValue != e.OldValue)
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

		#region Public Methods

		public abstract Task TransitionIn(VisualTransitionType type, bool useTransitions = true);

		public abstract Task TransitionOut(VisualTransitionType type, bool useTransitions = true);

		#endregion
	}
}
