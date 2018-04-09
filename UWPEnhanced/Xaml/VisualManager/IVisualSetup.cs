using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPEnhanced.Xaml
{
	public interface IVisualSetup
	{
		string Name { get; }

		Task TransitionIn(bool useTransitions);
		Task TransitionOut(bool useTransitions);

	}
}
