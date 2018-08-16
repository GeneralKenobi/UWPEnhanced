using Windows.System;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Provides support for key modifiers and checks whether they were present during the specific routed event
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class VisualRoutedEventTriggerWithModifiers<T> : VisualRoutedEventTrigger<T>
		where T : RoutedEventArgs
	{
		#region Modifiers Dependency Property

		/// <summary>
		/// Modifiers that will warrant the trigger. Setting <see cref="uint.MaxValue"/> (which is the default value)
		/// will ignore modifiers
		/// </summary>
		public VirtualKeyModifiers Modifiers
		{
			get => (VirtualKeyModifiers)GetValue(ModifiersProperty);
			set => SetValue(ModifiersProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Modifiers"/>
		/// </summary>
		public static readonly DependencyProperty ModifiersProperty =
			DependencyProperty.Register(nameof(Modifiers), typeof(VirtualKeyModifiers),
			typeof(VisualRoutedEventTrigger<T>), new PropertyMetadata((VirtualKeyModifiers)uint.MaxValue));

		#endregion

		#region StrictModifierCheck Dependency Property

		/// <summary>
		/// If true, the event will be fired if and only if all modifiers match the current value.
		/// If false, the event will be fired if the modifiers contain the specified modifiers.
		/// Ex: If set modifier are Ctrl only, if true the trigger will fire if and only if the routed event has Ctrl modifier only,
		/// if false the trigger will trigger for, among others, Ctrl; Ctrl+Shift; Ctrl+Menu+Shift.
		/// Default value is false
		/// </summary>
		public bool StrictModifierCheck
		{
			get => (bool)GetValue(StrictModifierCheckProperty);
			set => SetValue(StrictModifierCheckProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="StrictModifierCheck"/>
		/// </summary>
		public static readonly DependencyProperty StrictModifierCheckProperty =
			DependencyProperty.Register(nameof(StrictModifierCheck), typeof(bool),
			typeof(VisualRoutedEventTrigger<T>), new PropertyMetadata(false));

		#endregion

		#region Private methods

		/// <summary>
		/// Returns true if the modifiers specified by <see cref="Modifiers"/> are found with appropriate test determined by
		/// <see cref="StrictModifierCheck"/> in the <see cref="Modifiers"/>
		/// </summary>
		/// <returns></returns>
		private bool ModifiersMatch(VirtualKeyModifiers modifiers)
		{
			// uint.Max means modifiers are not taken into account
			if ((uint)Modifiers == uint.MaxValue)
			{
				return true;
			}

			return StrictModifierCheck ? Modifiers == modifiers : modifiers.HasFlag(Modifiers);
		}

		#endregion

		#region Protected methods

		/// <summary>
		/// Returns the modifiers present at the moment of call, may be overriden (for example when modifiers are given with the
		/// <see cref="RoutedEventArgs"/>
		/// </summary>
		/// <returns></returns>
		protected virtual VirtualKeyModifiers GetModifiers(T e) => ModifiersHelpers.GetCurrentModifiers();

		/// <summary>
		/// Method checking whether the modifiers match and the trigger is eligible for firing (<see cref="TriggerConditionsMet(T)"/>),
		/// calling <see cref="AssignHandled(T)"/> and then firing the Triggered event. It is the default candidate to use
		/// when subscribing to event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void TriggerEvent(object sender, T e)
		{
			if (ModifiersMatch(GetModifiers(e)) && TriggerConditionsMet(e))
			{
				// Let the derived classes set the Handled property on their specific RoutedEventArgs
				AssignHandled(e);

				// Invoke the event
				Triggered?.Invoke(this, e);
			}
		}

		#endregion
	}
}