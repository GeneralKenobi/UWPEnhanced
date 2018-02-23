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
	}
}
