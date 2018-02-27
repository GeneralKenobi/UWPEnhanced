using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Base class for triggers governed by pointer events on a <see cref="UIElement"/>
	/// </summary>
    public abstract class PointerEventStateTrigger : StateTriggerBase
    {
		#region TriggerForElement Dependency Property

		/// <summary>
		/// Element for which the pointer events will be caught
		/// </summary>
		public UIElement TriggerForElement
		{
			get => (UIElement)GetValue(TriggerForElementProperty);
			set => SetValue(TriggerForElementProperty, value);
		}

		/// <summary>
		/// Element for which the pointer events will be caught
		/// </summary>
		public static readonly DependencyProperty TriggerForElementProperty =
			DependencyProperty.Register(nameof(TriggerForElement), typeof(UIElement), typeof(PointerEventStateTrigger),
				new PropertyMetadata(null, new PropertyChangedCallback(TriggerForElementChanged)));

		/// <summary>
		/// If old value wasn't null, calls <see cref="UnsubscribeFromEvents(UIElement)"/>
		/// If new value isn't null, calls <see cref="SubscribeToEvents(UIElement)"/>
		/// </summary>
		/// <param name="d"></param>
		/// <param name="args"></param>
		private static void TriggerForElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			if (d is PointerEventStateTrigger stateTrigger)
			{
				if (args.OldValue != null && args.OldValue is UIElement oldElement)
				{
					stateTrigger.UnsubscribeFromEvents(oldElement);
				}

				if (args.NewValue!= null && args.NewValue is UIElement newElement)
				{
					stateTrigger.SubscribeToEvents(newElement);
				}
			}
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Implementation should subscribe to desired events with <see cref="SetTrigger"/> and <see cref="ResetTrigger"/>
		/// </summary>
		/// <param name="element"></param>
		protected abstract void SubscribeToEvents(UIElement element);

		/// <summary>
		/// Implementation should unsubscribe <see cref="SetTrigger"/> and <see cref="ResetTrigger"/> from desired events
		/// </summary>
		/// <param name="element"></param>
		protected abstract void UnsubscribeFromEvents(UIElement element);

		/// <summary>
		/// Sets the trigger to active
		/// </summary>
		protected void SetTrigger() => SetActive(true);

		/// <summary>
		/// Sets the trigger to inactive
		/// </summary>
		protected void ResetTrigger() => SetActive(false);

		#endregion
	}
}
