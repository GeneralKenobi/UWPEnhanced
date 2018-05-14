using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Class containing information taken from an <see cref="UIElement"/> used to craete an <see cref="Icon"/>
	/// </summary>
	internal struct MenuContentIconInfo
	{
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public MenuContentIconInfo(string glyph, ImageSource image, int index, ICommand iconPressedCommand)
		{
			Glyph = glyph;
			Image = image;
			Index = index;
			IconPressedCommand = iconPressedCommand;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Glyph to present with the <see cref="Icon"/>
		/// </summary>
		public string Glyph { get; private set; }

		/// <summary>
		/// Image to present on the <see cref="Icon"/>
		/// </summary>
		public ImageSource Image { get; private set; }

		/// <summary>
		/// Index of the element represented by the Icon
		/// </summary>
		public int Index { get; private set; }

		/// <summary>
		/// Command to execute when the Icon is clicked
		/// </summary>
		public ICommand IconPressedCommand { get; private set; }

		#endregion
	}
}
