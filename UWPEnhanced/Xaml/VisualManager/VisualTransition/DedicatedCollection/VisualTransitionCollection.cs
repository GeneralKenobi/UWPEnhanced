using System;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Collection of <see cref="VisualTransition"/>s which propagates events fired by the contained <see cref="VisualTransition"/>
	/// </summary>
	public class VisualTransitionCollection : VisualAttachmentCollection<VisualTransition>
	{
		#region Events

		/// <summary>
		/// Propagates all events from contained <see cref="VisualTransitionCollection"/>
		/// </summary>
		public EventHandler<VisualTransitionTriggeredEventArgs> TransitionTriggered;

		#endregion

		#region Protected Methods

		/// <summary>
		/// After calling the base version subscribes to its <see cref="VisualTransition.TransitionTriggered"/> event
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override VisualTransition NewElementCheckRoutine(DependencyObject item)
		{
			var baseChecked = base.NewElementCheckRoutine(item);

			baseChecked.TransitionTriggered += PropagateEvent;

			return baseChecked;
		}

		/// <summary>
		/// Before calling the base version unsubscribes from its <see cref="VisualTransition.TransitionTriggered"/> event
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override void CleanupRoutine(VisualTransition item)
		{
			item.TransitionTriggered -= PropagateEvent;

			base.CleanupRoutine(item);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Used to subscribe to <see cref="VisualTransition.TransitionTriggered"/> events; Propagates the event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PropagateEvent(object sender, VisualTransitionTriggeredEventArgs e) => TransitionTriggered?.Invoke(sender, e);

		#endregion
	}
}