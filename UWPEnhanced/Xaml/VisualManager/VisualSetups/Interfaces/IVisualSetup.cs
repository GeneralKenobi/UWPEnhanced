using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Interface for classes used in <see cref="VisualManager"/>
	/// </summary>
	public interface IVisualSetup
	{
		/// <summary>
		/// Name of the state. Has to be unique: it will be checked by <see cref="VisualSetupGroup"/>
		/// </summary>
		string ID { get; }

		/// <summary>
		/// <see cref="VisualSetupGroup"/> which owns this <see cref="IVisualSetup"/>
		/// </summary>
		VisualSetupGroup Parent { get; }

		/// <summary>
		/// Assigns the given <see cref="VisualSetupGroup"/> as a parent of this <see cref="IVisualSetup"/>
		/// </summary>
		/// <param name="parent"></param>
		void AssignParent(VisualSetupGroup parent);

		/// <summary>
		/// Removes the current parent from the <see cref="VisualSetupGroup"/>
		/// </summary>
		void RemoveParent();

		/// <summary>
		/// Transitions into the setup and returns a <see cref="Task"/> that will complete when the transition is done
		/// </summary>
		/// <param name="type">Type of the transition used to perform specific actions based on the implementation
		/// specific handling of repeated transitions</param>
		/// <param name="useTransitions"></param>
		/// <returns></returns>
		Task TransitionIn(VisualTransitionType type, bool useTransitions);

		/// <summary>
		/// Transitions out of the setup and returns a <see cref="Task"/> that will complete when the transition is done
		/// </summary>
		/// <param name="type">Type of the transition used to perform specific actions based on the implementation
		/// specific handling of repeated transitions</param>
		/// <param name="useTransitions"></param>
		/// <returns></returns>
		Task TransitionOut(VisualTransitionType type, bool useTransitions);

		/// <summary>
		/// If true, the visual setup will be navigated to when its created
		/// </summary>
		bool EnterWhenCreated { get; set; }
	}
}
