using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPEnhanced.Helpers;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Implementation identical to <see cref="VisualSetup"/> except when the TransitionIn is finished, the TransitionIn
	/// of <see cref="Cascade"/> is called. This recursion goes on until a null <see cref="Cascade"/> is found. TransitionOut
	/// works the same way except it starts from the most "inner" <see cref="VisualSetupCascade"/>. So for a structure like this:
	/// <para/>
	/// VisualSetupCascade ID = "A"	
	/// <para/>
	///	____VisualSetupCascade ID = "B" 
	///	<para/>
	///	________VisualSetupCascade ID = "C"
	///	<para/>
	///	The order of transitions is In_A -> Setters_A -> In_B -> Setters_B -> In_C -> Setters_C ... Reset_Temp_Setters_C ->
	///	Out_C -> Reset_Temp_Setters_B -> Out_B -> Reset_Temp_Setters_A -> Out_A
	/// </summary>
	public class VisualSetupCascade : VisualSetup
	{
		#region Cascade Dependency Property

		/// <summary>
		/// Another element of the cascade structure
		/// </summary>
		public VisualSetupCascade Cascade
		{
			get => (VisualSetupCascade)GetValue(CascadeProperty);
			set => SetValue(CascadeProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Cascade"/>
		/// </summary>
		public static readonly DependencyProperty CascadeProperty =
			DependencyProperty.Register(nameof(Cascade), typeof(VisualSetupCascade),
			typeof(VisualSetupCascade), new PropertyMetadata(default(VisualSetupCascade)));

		#endregion

		#region Private Methods

		/// <summary>
		/// Gets the <see cref="Cascade"/> using the Dispatcher (on the UI thread)
		/// </summary>
		/// <returns></returns>
		private async Task<VisualSetupCascade> GetCascadeUsingDispatcher()
		{
			VisualSetupCascade cascade = null;

			await DispatcherHelpers.RunAsync(() => cascade = Cascade);

			return cascade;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Transitions into the setup and all defined cascades. The Task finishes when all transitions have been performed.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="useTransitions"></param>
		/// <returns></returns>
		public async override Task TransitionIn(VisualTransitionType type, bool useTransitions = true)
		{
			// Transition into this setup
			await base.TransitionIn(type, useTransitions);

			// If the cascade is not null, transition into it as well (and wait for the transition to finish)
			await (await GetCascadeUsingDispatcher()).TransitionIn(type, useTransitions);
		}

		/// <summary>
		/// Transitions out of the setup and all defined cascades. The Task finishes when all transitions have been performed.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="useTransitions"></param>
		public async override Task TransitionOut(VisualTransitionType type, bool useTransitions = true)
		{
			// If the cascade is not null, transition into it as well (and wait for the transition to finish)
			await(await GetCascadeUsingDispatcher()).TransitionOut(type, useTransitions);

			// Transition into this setup
			await base.TransitionOut(type, useTransitions);
		}

		#endregion
	}
}