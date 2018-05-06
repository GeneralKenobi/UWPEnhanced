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

		#region Public Properties

		/// <summary>
		/// <see cref="VisualSetupGroup"/> which owns this <see cref="IVisualSetup"/>
		/// </summary>
		public VisualSetupGroup Parent { get; private set; }

		#endregion

		#region Name Dependency Property

		/// <summary>
		/// Name of this setup
		/// </summary>		
		public string ID
		{
			get => (string)GetValue(IDProperty);
			set => SetValue(IDProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="ID"/>
		/// </summary>
		public static readonly DependencyProperty IDProperty =
			DependencyProperty.Register(nameof(ID), typeof(string),
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

		#region EnterWhenCreated Dependency Property

		/// <summary>
		/// If true, the visual setup will be navigated to when its created
		/// </summary>
		public bool EnterWhenCreated
		{
			get => (bool)GetValue(EnterWhenCreatedProperty);
			set => SetValue(EnterWhenCreatedProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="EnterWhenCreated"/>
		/// </summary>
		public static readonly DependencyProperty EnterWhenCreatedProperty =
			DependencyProperty.Register(nameof(EnterWhenCreated), typeof(bool),
			typeof(VisualSetupBase), new PropertyMetadata(default(bool)));

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
		
		/// <summary>
		/// Assigns the given <see cref="VisualSetupGroup"/> as a parent of this <see cref="IVisualSetup"/>
		/// </summary>
		/// <param name="parent"></param>
		public void AssignParent(VisualSetupGroup parent)
		{
			// Check if the argument is correct
			if (parent == null)
			{
				throw new Exception("New parent cannot be null");
			}
			// Check if a parent was already attached
			else if (Parent != null)
			{
				throw new Exception("This VisualSetup already has a parent");
			}
			// If everything is fine attach the parent
			else
			{
				Parent = parent;
			}
		}

		/// <summary>
		/// Removes the current parent from the <see cref="VisualSetupGroup"/>
		/// </summary>
		public void RemoveParent() => Parent = null;

		/// <summary>
		/// Transitions into the setup and returns a <see cref="Task"/> that will complete when the transition is done
		/// </summary>
		/// <param name="type">Type of the transition used to perform specific actions based on the implementation
		/// specific handling of repeated transitions</param>
		/// <param name="useTransitions"></param>
		/// <returns></returns>
		public abstract Task TransitionIn(VisualTransitionType type, bool useTransitions = true);

		/// <summary>
		/// Transitions out of the setup and returns a <see cref="Task"/> that will complete when the transition is done
		/// </summary>
		/// <param name="type">Type of the transition used to perform specific actions based on the implementation
		/// specific handling of repeated transitions</param>
		/// <param name="useTransitions"></param>
		/// <returns></returns>
		public abstract Task TransitionOut(VisualTransitionType type, bool useTransitions = true);

		#endregion
	}
}