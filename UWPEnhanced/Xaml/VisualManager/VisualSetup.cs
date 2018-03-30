using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;

namespace UWPEnhanced.Xaml
{
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

		private ManualResetEvent _WaitForStoryboard = new ManualResetEvent(false);

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
			typeof(VisualSetup), new PropertyMetadata(default(Storyboard), new PropertyChangedCallback(StoryboardChanged)));

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
			typeof(VisualSetup), new PropertyMetadata(default(Storyboard)));

		#endregion

		#region Private Methods

		private static void StoryboardChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if(sender is VisualSetup setup && e.NewValue != e.OldValue && e.NewValue is Storyboard storyboard)
			{
				storyboard.Completed += setup.StoryboardCompleted;
			}
		}

		private void StoryboardCompleted(object sender, object e)
		{
			_WaitForStoryboard.Set();
		}
		#endregion
		#region Public Methods

		/// <summary>
		/// Transitions into the state: 
		/// </summary>
		public void TransitionIn()
		{
			lock(_WaitForStoryboard)
			{
				_WaitForStoryboard.Reset();
				TransitionInStoryboard.Begin();
				_WaitForStoryboard.WaitOne();
			}
			Setters.ForEach((x) => x.Set());
			TemporarySetters.ForEach((x) => x.Set());
		}

		/// <summary>
		/// Transitions into the state: 
		/// </summary>
		public void TransitionOut()
		{
			TemporarySetters.ForEach((x) => x.Reset());
			lock (_WaitForStoryboard)
			{
				_WaitForStoryboard.Reset();
				TransitionOutStoryboard.Begin();
				_WaitForStoryboard.WaitOne();
			}			
		}


		#endregion
	}
}
