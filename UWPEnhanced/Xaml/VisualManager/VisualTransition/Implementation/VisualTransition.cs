using System;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Allows to condense transitions to a single state triggered by different <see cref="IVisualTrigger"/>
	/// into a single object
	/// </summary>
	public class VisualTransition : VisualAttachmentCollection<IVisualTrigger>
	{
		#region Events

		/// <summary>
		/// Event fired whenever one of the contained triggers fires
		/// </summary>
		public EventHandler<VisualTransitionTriggeredEventArgs> TransitionTriggered;

		#endregion

		#region TransitionTo Dependency Property

		/// <summary>
		/// Name of the instance to transition to
		/// </summary>
		public string TransitionTo
		{
			get => (string)GetValue(TransitionToProperty);
			set => SetValue(TransitionToProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="TransitionTo"/>
		/// </summary>
		public static readonly DependencyProperty TransitionToProperty =
			DependencyProperty.Register(nameof(TransitionTo), typeof(string),
			typeof(VisualTransition), new PropertyMetadata(string.Empty));

		#endregion

		#region UseTransitions Dependency Property

		/// <summary>
		/// Determines whether to use transitions during the instance transition. Default value is true
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
			typeof(VisualTransition), new PropertyMetadata(true));

		#endregion

		#region Protected Methods

		/// <summary>
		/// After calling base version subscribes to <see cref="IVisualTrigger.Triggered"/> event
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override IVisualTrigger NewElementCheckRoutine(DependencyObject item)
		{
			var baseCheck = base.NewElementCheckRoutine(item);

			baseCheck.Triggered += TriggerFired;

			return baseCheck;
		}

		/// <summary>
		/// Before calling base version unsubscribes from <see cref="IVisualTrigger.Triggered"/> event
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override void CleanupRoutine(IVisualTrigger item)
		{
			item.Triggered -= TriggerFired;

			base.CleanupRoutine(item);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Used to subscribe to contained events; Invokes <see cref="TransitionTriggered"/> event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void TriggerFired(object sender, object args) =>
			TransitionTriggered?.Invoke(this, new VisualTransitionTriggeredEventArgs(
				sender as IVisualTrigger, TransitionTo, UseTransitions));

		#endregion
	}
}