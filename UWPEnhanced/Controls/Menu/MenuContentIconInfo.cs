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
		public MenuContentIconInfo(string glyph, ImageSource image, ICommand iconPressedCommand,
			UIElement representedElement, double fontSize)
		{
			Glyph = glyph;
			Image = image;
			IconPressedCommand = iconPressedCommand;
			RepresentedElement = representedElement;
			FontSize = fontSize;
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
		/// <see cref="UIElement"/> deconstructed by this <see cref="MenuContentIconInfo"/>
		/// </summary>
		public UIElement RepresentedElement { get; private set; }

		/// <summary>
		/// Command to execute when the Icon is clicked
		/// </summary>
		public ICommand IconPressedCommand { get; private set; }

		/// <summary>
		/// FontSize for the icon's glyph
		/// </summary>
		public double FontSize { get; private set; }

		#endregion
	}
}