﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Class containing information taken from an <see cref="UIElement"/> used to craete an <see cref="Icon"/>
	/// </summary>
	internal class MenuContentIconInfo
	{
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public MenuContentIconInfo(string glyph, ImageSource image)
		{
			Glyph = glyph;
			Image = image;
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

		#endregion
	}
}
