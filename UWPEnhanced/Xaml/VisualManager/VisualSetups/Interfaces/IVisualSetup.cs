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
		/// Transitions into the setup and returns a <see cref="Task"/> that will complete when the transition is done
		/// </summary>
		/// <param name="type">Type of the transition used to perform specific actions based on the implementation
		/// specific handling of repeated transitions</param>
		/// <param name="useTransitions"></param>
		/// <returns></returns>
		Task TransitionIn(VisualTransitionType type, bool useTransitions);
		Task TransitionOut(VisualTransitionType type, bool useTransitions);

	}
}
