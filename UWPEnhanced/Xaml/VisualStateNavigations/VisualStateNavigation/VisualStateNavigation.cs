using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// A single navigation which can be fired by many triggers
	/// </summary>
	public class VisualStateNavigation : VisualAttachmentCollection<IVisualTrigger>
	{
		#region Private Members

		/// <summary>
		/// The control that is responsible visual states in this <see cref="DependencyObject"/>
		/// </summary>
		private Control _VisualStateHolder = null;

		#endregion

		#region State Dependency Property

		/// <summary>
		/// State to navigate to
		/// </summary>
		public string State
		{
			get => (string)GetValue(StateProperty);
			set => SetValue(StateProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="State"/>
		/// </summary>
		public static readonly DependencyProperty StateProperty =
			DependencyProperty.Register(nameof(State), typeof(string),
			typeof(VisualStateNavigation), new PropertyMetadata(default(string)));

		#endregion

		#region UseTransitions Dependency Property

		/// <summary>
		/// Determines whether to use visual transitions in the process of the navigation
		/// </summary>
		public bool UseTransitions
		{
			get => (bool)GetValue(UseTransitionsProperty);
			set => SetValue(UseTransitionsProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="UseTransitions"/>
		/// </summary>
		public static readonly DependencyProperty UseTransitionsProperty =
			DependencyProperty.Register(nameof(UseTransitions), typeof(bool),
			typeof(VisualStateNavigation), new PropertyMetadata(true));

		#endregion
		
		#region Protected Methods

		/// <summary>
		/// Besides calling the base version it also subscribes to the <see cref="IVisualTrigger.Triggered"/> event
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override IVisualTrigger NewElementCheckRoutine(DependencyObject item)
		{
			var newElement = base.NewElementCheckRoutine(item);

			// Subscribe to navigation triggered
			newElement.Triggered += NavigationTriggered;

			return newElement;
		}

		/// <summary>
		/// Besides calling the base version it also unsubscribes from the <see cref="IVisualTrigger.Triggered"/> event
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override void CleanupRoutine(IVisualTrigger item)
		{
			item.Triggered -= NavigationTriggered;
			base.CleanupRoutine(item);
		}

		/// <summary>
		/// Besides calling the base version also tries to find the parent of type control and store it in <see cref="_VisualStateHolder"/>
		/// </summary>
		/// <param name="obj"></param>
		public override void Attach(DependencyObject obj)
		{
			base.Attach(obj);
			
			// Find the control parent
			_VisualStateHolder = VisualTreeHelpers.FindParentOrSelf<Control>(AttachedTo);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Method called whenever one of the containing triggers is triggered
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NavigationTriggered(object sender, object e)
		{
			if (_VisualStateHolder != null ||
				// Often the parent cannot be found when during Attach so in case AttachedTo is not null try to find it again
				(AttachedTo != null && VisualTreeHelpers.TryFindParentOrSelf(AttachedTo, out _VisualStateHolder)))
			{
				Navigate();
			}
		}

		/// <summary>
		/// Navigates to the specified <see cref="State"/> on <see cref="_VisualStateHolder"/>. 
		/// Requires that <see cref="_VisualStateHolder"/> is not null
		/// </summary>
		private void Navigate() => VisualStateManager.GoToState(_VisualStateHolder, State, UseTransitions);

		#endregion
	}
}
