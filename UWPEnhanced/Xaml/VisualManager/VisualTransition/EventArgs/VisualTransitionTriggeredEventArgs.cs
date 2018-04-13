using System;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Arguments for VisualTransitionEvent
	/// </summary>
	public class VisualTransitionTriggeredEventArgs : EventArgs
	{
		/// <summary>
		/// <see cref="IVisualTrigger"/> that was resposnible for the trigger
		/// </summary>
		public IVisualTrigger TriggeredEvent { get; private set; }

		/// <summary>
		/// Name of the instance to transition to
		/// </summary>
		public string TransitionTo { get; private set; }

		/// <summary>
		/// Determines whether to use transitions in the process
		/// </summary>
		public bool UseTransitions { get; private set; }

		/// <summary>
		/// Default constructor, requires parameters
		/// </summary>
		/// <exception cref="ArgumentNullException">For <paramref name="trigger"/></exception>
		public VisualTransitionTriggeredEventArgs(IVisualTrigger trigger, string transitionTo, bool useTransitions)
		{
			TriggeredEvent = trigger ?? throw new ArgumentNullException(nameof(trigger));
			TransitionTo = transitionTo;
			UseTransitions = useTransitions;
		}
	}
}