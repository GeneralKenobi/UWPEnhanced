using System;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Defines the behavior in case of a repeated transition
	/// (<see cref="IVisualSetup"/> A to <see cref="IVisualSetup"/> A)
	/// </summary>
	[Flags]
	public enum RepeatedTransitionBehavior
	{
		/// <summary>
		/// Skips both <see cref="IVisualSetup.TransitionIn(VisualTransitionType, bool)"/> and 
		/// <see cref="IVisualSetup.TransitionOut(VisualTransitionType, bool)"/>
		/// </summary>
		Skip = 0,

		/// <summary>
		/// Repeats <see cref="IVisualSetup.TransitionIn(VisualTransitionType, bool)"/> but skips
		/// <see cref="IVisualSetup.TransitionOut(VisualTransitionType, bool)"/>
		/// </summary>
		TransitionIn = 1,

		/// <summary>
		/// Skips <see cref="IVisualSetup.TransitionIn(VisualTransitionType, bool)"/> but repeats
		/// <see cref="IVisualSetup.TransitionOut(VisualTransitionType, bool)"/>
		/// </summary>
		TransitionOut = 2,

		/// <summary>
		/// Repeats both <see cref="IVisualSetup.TransitionIn(VisualTransitionType, bool)"/> and 
		/// <see cref="IVisualSetup.TransitionOut(VisualTransitionType, bool)"/>
		/// </summary>
		Both = 3,
	}
}