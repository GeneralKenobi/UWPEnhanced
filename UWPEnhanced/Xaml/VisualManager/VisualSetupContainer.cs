using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Class which handles <see cref="VisualSetup"/>s associated with a given <see cref="DependencyObject"/>
	/// </summary>
	internal class VisualSetupContainer
	{
		#region Private Members

		/// <summary>
		/// List containing all visual setups associated with the <see cref="DependencyObject"/>
		/// </summary>
		private readonly List<VisualSetup> _AssociatedVisualSetups = new List<VisualSetup>();

		#endregion

		#region Public Methods

		/// <summary>
		/// Adds a <see cref="VisualSetup"/> to the container
		/// </summary>
		/// <param name="setup"></param>
		public void AddVisualSetup(VisualSetup setup)
		{
			if(!_AssociatedVisualSetups.Contains(setup))
			{
				_AssociatedVisualSetups.Add(setup);
			}
		}

		#endregion
	}
}
