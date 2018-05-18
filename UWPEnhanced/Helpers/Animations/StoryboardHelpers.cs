using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace UWPEnhanced.Helpers
{
	/// <summary>
	/// Class with helper methods related to <see cref="Storyboard"/>s
	/// </summary>
    public static class StoryboardHelpers
    {
		/// <summary>
		/// Returns true if the given <see cref="Storyboard"/> will fire the completed event in finite time
		/// if the <see cref="Storyboard.Begin"/> method is called
		/// </summary>
		/// <param name="storyboard"></param>
		/// <returns></returns>
		public static bool WillComplete(this Storyboard storyboard) =>		
			// If the storyboard is not null, has a defined at least one timeline object or a finite duration
			// it will complete in a finite time
			storyboard != null && (storyboard.Children.Count > 0 ||
				(storyboard.Duration != Duration.Automatic && storyboard.Duration != Duration.Forever));
		
    }
}