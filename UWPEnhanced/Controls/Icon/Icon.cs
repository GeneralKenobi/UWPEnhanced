using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace UWPEnhanced.Controls
{
	public sealed class Icon : Control
	{
		public Icon()
		{
			this.DefaultStyleKey = typeof(Icon);
		}

		#region Glyph Dependency Property

		/// <summary>
		/// Glyph shown by this Icon
		/// </summary>
		public string Glyph
		{
			get => (string)GetValue(GlyphProperty);
			set => SetValue(GlyphProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Glyph"/>
		/// </summary>
		public static readonly DependencyProperty GlyphProperty =
			DependencyProperty.Register(nameof(Glyph), typeof(string), typeof(Icon), new PropertyMetadata(string.Empty));

		#endregion

		#region ImageSource Dependency Property

		/// <summary>
		/// Image source for the image presenter
		/// </summary>
		public ImageSource ImageSource
		{
			get => (ImageSource)GetValue(ImageSourceProperty);
			set => SetValue(ImageSourceProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="ImageSource"/>
		/// </summary>
		public static readonly DependencyProperty ImageSourceProperty =
			DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(Icon), new PropertyMetadata(null));

		#endregion

		#region Image Stretch

		/// <summary>
		/// Image stretch
		/// </summary>
		public Stretch ImageStretch
		{
			get => (Stretch)GetValue(ImageStretchProperty);
			set => SetValue(ImageStretchProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="ImageStretch"/>
		/// </summary>
		public static readonly DependencyProperty ImageStretchProperty =
			DependencyProperty.Register(nameof(ImageStretch), typeof(Stretch),
				typeof(Icon), new PropertyMetadata(Stretch.None));

		#endregion
	}
}
