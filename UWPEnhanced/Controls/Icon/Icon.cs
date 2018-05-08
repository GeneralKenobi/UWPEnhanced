using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace UWPEnhanced.Controls
{
	public sealed class Icon : Control, INotifyPropertyChanged
	{
		#region Private Static Members

		/// <summary>
		/// Name of the element scaled upon pointer press
		/// </summary>
		private static readonly string _ContainerName = "ScaledContainer";

		#endregion

		#region Private Members

		/// <summary>
		/// Container for main visual elements that is needed to determine the center of scale transform
		/// </summary>
		private FrameworkElement _Container = null;

		#endregion

		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public Icon()
		{
			this.DefaultStyleKey = typeof(Icon);
			this.PointerPressed += (s, e) => Command?.Execute(CommandParameter);

			// Whenever size changed notify that scale center also changes
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			FindContainerGrid();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Method which finds Container from the visual tree which is needed to determine the scale transform center.
		/// Additionally subsribes to its SizeChanged event in order to fire INotifyPropertyChanged for
		/// <see cref="ScaleCenterX"/> and <see cref="ScaleCenterY"/>
		/// </summary>
		/// <returns></returns>
		private async Task FindContainerGrid()
		{
			// Await so that the constructor ends and we can get the container grid
			await Task.Delay(20);

			// Try to get the Container element
			DependencyObject obj = GetTemplateChild(_ContainerName);

			// If the operation failed
			if (obj == null)
			{
				// Wait an additional second
				await Task.Delay(1000);

				// Try again
				obj = GetTemplateChild(_ContainerName);

				// If failed again, throw an exception to notify about that
				if(obj == null)
				{
					throw new Exception("Can't find \"Container\", visual structure may have been violated");	
				}
			}

			// If operation succeeded try to cast it to a FrameworkElement
			if(obj is FrameworkElement element)
			{
				// If success, assign it to private member
				_Container = element;

				// Subscribe to size changed
				element.SizeChanged+= (s, e) =>
				{
					InvokePropertyChanged(nameof(ScaleCenterX), nameof(ScaleCenterY));
				};

				// Fire event in order to notify about the first value
				InvokePropertyChanged(nameof(ScaleCenterX), nameof(ScaleCenterY));
			}
			else
			{
				// If failed, throw an exception to notify about that
				throw new Exception("\"Container\" is not a FrameworkElement - visual structure may have been violated");
			}
		}

		#endregion	

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
			DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource),
				typeof(Icon), new PropertyMetadata(null));

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

		#region INotifyPropertyChanged

		/// <summary>
		/// Event raised when property changes
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Invokes Property Changed Event for each string parameter
		/// </summary>
		/// <param name="propertyName">Properties to invoke for.
		/// Null or string.Empty will result in notification for all properties</param>
		public void InvokePropertyChanged(params string[] propertyNames)
		{
			foreach (var item in propertyNames)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(item));
			}
		}

		#endregion

		#region Command Dependency Property

		/// <summary>
		/// Command invoked when pointer is released on the icon
		/// </summary>
		public ICommand Command
		{
			get => (ICommand)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		/// <summary>
		/// Command invoked when pointer is released on the icon
		/// </summary>
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register(nameof(Command), typeof(ICommand),
			typeof(Icon), new PropertyMetadata(default(ICommand)));

		#endregion

		#region CommandParameter Dependency Property

		/// <summary>
		/// Parameter for <see cref="Command"/>
		/// </summary>
		public object CommandParameter
		{
			get => GetValue(CommandParameterProperty);
			set => SetValue(CommandParameterProperty, value);
		}

		/// <summary>
		/// Parameter for <see cref="Command"/>
		/// </summary>
		public static readonly DependencyProperty CommandParameterProperty =
			DependencyProperty.Register(nameof(CommandParameter), typeof(object),
			typeof(Icon), new PropertyMetadata(default(object)));

		#endregion

		#region HighlightBrush Dependency Property

		/// <summary>
		/// Brush presented when the Icon is highlighted, default is white
		/// </summary>
		public Brush HighlightBrush
		{
			get => (Brush)GetValue(HighlightBrushProperty);
			set => SetValue(HighlightBrushProperty, value);
		}

		/// <summary>
		/// Brush presented when the Icon is highlighted, default is white
		/// </summary>
		public static readonly DependencyProperty HighlightBrushProperty =
			DependencyProperty.Register(nameof(HighlightBrush), typeof(Brush),
			typeof(Icon), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255))));

		#endregion

		#region Public Properties

		/// <summary>
		/// X center for scale transform
		/// </summary>
		public double ScaleCenterX => _Container == null ? 0 : _Container.ActualWidth / 2;

		/// <summary>
		/// Y center for scale transform
		/// </summary>
		public double ScaleCenterY => _Container == null ? 0 : _Container.ActualHeight/ 2;

		#endregion
	}
}
