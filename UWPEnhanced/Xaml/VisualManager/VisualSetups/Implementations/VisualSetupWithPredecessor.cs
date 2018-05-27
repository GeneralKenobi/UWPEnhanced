using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// <see cref="VisualSetup"/> that first transitions through its <see cref="Predecessor"/>
	/// </summary>
	public class VisualSetupWithPredecessor : VisualSetup
    {
		#region Protected Properties

		/// <summary>
		/// The predecessor located in the parent group
		/// </summary>
		protected IVisualSetup FoundPredecessor { get; set; }

		#endregion

		#region Predecessor Dependency Property

		/// <summary>
		/// The predecessor to transition into this state through
		/// </summary>
		public string Predecessor
		{
			get => (string)GetValue(PredecessorProperty);
			set => SetValue(PredecessorProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Predecessor"/>
		/// </summary>
		public static readonly DependencyProperty PredecessorProperty =
			DependencyProperty.Register(nameof(Predecessor), typeof(string),
			typeof(VisualSetupWithPredecessor), new PropertyMetadata(default(string), PredecessorChanged));

		#endregion

		#region Protected methods

		/// <summary>
		/// Tries to find the predecessor setup in the parent visual setup group and store it in <see cref="FoundPredecessor"/>,
		/// returns true on sucess
		/// </summary>
		/// <returns></returns>
		protected bool TryFindPredecessorSetup()
		{
			if(Parent!= null)
			{
				FoundPredecessor = Parent.FirstOrDefault((x) => x is IVisualSetup setup && setup.ID == Predecessor) as IVisualSetup;
			}

			return FoundPredecessor != null;
		}

		#endregion

		#region Private static methods

		/// <summary>
		/// Attempts to find the new predecessor when the <see cref="PredecessorProperty"/> changes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void PredecessorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is VisualSetupWithPredecessor setup)
			{
				setup.TryFindPredecessorSetup();
			}
		}

		#endregion
		
		#region Public Methods

		public async override Task TransitionIn(VisualTransitionType type, bool useTransitions = true)
		{
			// If it's a transition to self and the transition in should be omitted return
			if (type == VisualTransitionType.ToTheSameSetup &&
				!(await GetRepeatedTransition()).HasFlag(RepeatedTransitionBehavior.TransitionIn))
			{
				return;
			}

			// If the predecessor is not null, transition through it and wait for completion
			if(FoundPredecessor != null || TryFindPredecessorSetup())
			{
				await FoundPredecessor.TransitionIn(type,useTransitions);
				await FoundPredecessor.TransitionOut(type, useTransitions);
			}

			// Finally transition self in using the VisualSetup method
			await base.TransitionIn(type, useTransitions);
		}

		#endregion
	}
}