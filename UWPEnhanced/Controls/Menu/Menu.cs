using CSharpEnhanced.ICommands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UWPEnhanced.Xaml;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace UWPEnhanced.Controls
{
	public sealed class Menu : Control, INotifyPropertyChanged
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public Menu()
        {
            this.DefaultStyleKey = typeof(Menu);
			Content = new ObservableCollection<UIElement>();		
			RecalculateContentTranslate();
			this.Loaded += OnLoaded;
			
			
			IconPressedCommand = new RelayParametrizedCommand(IconPressed);
			OpenCloseMenuCommand = new RelayCommand(OpenCloseMenu);
        }

		#endregion

		#region Private Members

		private Storyboard _TransitionOutContent = null;
		private Storyboard _TransitionInContent = null;

		#endregion

		#region Public Properties

		#region Control Size

		/// <summary>
		/// The length the menu has when its opened
		/// </summary>
		public double OpenLength => IconsPanelLength + ContentLength;

		#endregion

		#region Menu TranslateTransform		

		#endregion

		#region Content TranslateTransform

		/// <summary>
		/// Value to which Content will be moved horizontally using a <see cref="TranslateTransform"/> when Menu is closed.
		/// </summary>
		public double ContentTranslateTransformX { get; private set; }

		/// <summary>
		/// Value to which Content will be moved vertically using a <see cref="TranslateTransform"/> when Menu is closed.
		/// </summary>
		public double ContentTranslateTransformY { get; private set; }

		/// <summary>
		/// If <see cref="PropertyChanged"/> event is invoked for this property, the TranslateTransform of Content will be set
		/// to this value. Used to adjust manually the position of the Content when <see cref="Position"/> changes.
		/// </summary>
		public double ContentTranslateTransformXCorrection => ContentTranslateTransformX;

		/// <summary>
		/// If <see cref="PropertyChanged"/> event is invoked for this property, the TranslateTransform of Content will be set
		/// to this value. Used to adjust manually the position of the Content when <see cref="Position"/> changes.
		/// </summary>
		public double ContentTranslateTransformYCorrection => ContentTranslateTransformY;

		#endregion

		#region Selected Content

		/// <summary>
		/// Content to present
		/// </summary>
		public UIElement SelectedContent { get; private set; }

		#endregion

		#endregion

		#region ICommands

		public ICommand IconPressedCommand { get; private set; }
		public ICommand OpenCloseMenuCommand { get; private set; }

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

		#region Dependency Properties

		#region Position Dependency Property

		/// <summary>
		/// Position of the menu
		/// </summary>
		public MenuPosition Position
		{
			get => (MenuPosition)GetValue(PositionProperty);
			set => SetValue(PositionProperty, value);
		}

		/// <summary>
		/// Position of the menu
		/// </summary>
		public static readonly DependencyProperty PositionProperty =
			DependencyProperty.Register(nameof(Position), typeof(MenuPosition),
			typeof(Menu), new PropertyMetadata(MenuPosition.Left, new PropertyChangedCallback(MenuPositionChanged)));

		#endregion

		#region IconsPanelLength Dependency Property

		/// <summary>
		/// 
		/// </summary>
		public double IconsPanelLength
		{
			get => (double)GetValue(IconsPanelLengthProperty);
			set => SetValue(IconsPanelLengthProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="IconsPanelLength"/>
		/// </summary>
		public static readonly DependencyProperty IconsPanelLengthProperty =
			DependencyProperty.Register(nameof(IconsPanelLength), typeof(double),
			typeof(Menu), new PropertyMetadata(25d, new PropertyChangedCallback((s,e) =>
				(s as Menu)?.InvokePropertyChanged(nameof(IconsPanelLength), nameof(OpenLength)))));

		#endregion

		#region ContentLength Dependency Property

		/// <summary>
		/// 
		/// </summary>
		public double ContentLength
		{
			get => (double)GetValue(ContentLengthProperty);
			set => SetValue(ContentLengthProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="ContentLength"/>
		/// </summary>
		public static readonly DependencyProperty ContentLengthProperty =
			DependencyProperty.Register(nameof(ContentLength), typeof(double),
			typeof(Menu), new PropertyMetadata(100d, new PropertyChangedCallback((s,e)=>
			(s as Menu)?.InvokePropertyChanged(nameof(ContentLength), nameof(OpenLength)))));

		#endregion

		#region IsOpen Dependency Property

		/// <summary>
		/// Controls the open/close of the menu
		/// </summary>
		public bool IsOpen
		{
			get => (bool)GetValue(IsOpenProperty);
			set => SetValue(IsOpenProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="IsOpen"/>
		/// </summary>
		public static readonly DependencyProperty IsOpenProperty =
			DependencyProperty.Register(nameof(IsOpen), typeof(bool),
			typeof(Menu), new PropertyMetadata(false, new PropertyChangedCallback(
				(s,e) => (s as Menu)?.PropertyChanged?.Invoke(s, new PropertyChangedEventArgs(nameof(IsOpen))))));

		#endregion

		#region Content Dependency Property

		/// <summary>
		/// Collection of all elements that are placed in this menu
		/// </summary>
		public ObservableCollection<UIElement> Content
		{
			get => (ObservableCollection<UIElement>)GetValue(ContentProperty);
			set => SetValue(ContentProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Content"/>
		/// </summary>
		public static readonly DependencyProperty ContentProperty =
			DependencyProperty.Register(nameof(Content), typeof(ObservableCollection<UIElement>),
			typeof(Menu), new PropertyMetadata(default(ObservableCollection<UIElement>)));

		#endregion

		#region Glyph Attached Property

		/// <summary>
		/// Getter for <see cref="GlyphProperty"/>
		/// </summary>
		public static string GetGlyph(DependencyObject obj) => (string)obj.GetValue(GlyphProperty);

		/// <summary>
		/// Setter for <see cref="GlyphProperty"/>
		/// </summary>
		public static void SetGlyph(DependencyObject obj, string value) => obj.SetValue(GlyphProperty, value);

		/// <summary>
		///
		/// </summary>
		public static readonly DependencyProperty GlyphProperty =
			DependencyProperty.RegisterAttached("Glyph", typeof(string),
			 typeof(Menu), new PropertyMetadata(default(string)));

		#endregion

		#region Image Attached Property

		/// <summary>
		/// Getter for <see cref="ImageProperty"/>
		/// </summary>
		public static ImageSource GetImage(DependencyObject obj) => (ImageSource)obj.GetValue(ImageProperty);

		/// <summary>
		/// Setter for <see cref="ImageProperty"/>
		/// </summary>
		public static void SetImage(DependencyObject obj, ImageSource value) => obj.SetValue(ImageProperty, value);

		/// <summary>
		/// Image to present on the selecting Icon
		/// </summary>
		public static readonly DependencyProperty ImageProperty =
			DependencyProperty.RegisterAttached("Image", typeof(ImageSource),
			 typeof(Menu), new PropertyMetadata(default(ImageSource)));

		#endregion

		#endregion

		#region Private Static Methods

		#region Position Changed Handler

		/// <summary>
		/// Recalculates animation values which depend on the position of the menu (MenuTranslate, ContentTranslate).
		/// </summary>
		private static void MenuPositionChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			if (s is Menu menu)
			{
				menu.InvokePropertyChanged(nameof(Position));

				menu.RecalculateContentTranslate();
			}
		}

		#endregion

		#endregion

		#region Private Methods

		#region Content TranslateTransform

		/// <summary>
		/// Recalculates the <see cref="TranslateTransform"/> X and Y values for Content. Manually repositions the
		/// Content if the Menu was closed.
		/// </summary>
		private void RecalculateContentTranslate()
		{
			// Calculate the new values
			ContentTranslateTransformX = CalculateContentTranslateX();
			ContentTranslateTransformY = CalculateContentTranslateY();

			// Invoke the PropertyChangedEvent for them
			InvokePropertyChanged(nameof(ContentTranslateTransformX), nameof(ContentTranslateTransformY));

			// If the menu was closes its necessary to move the Content to the new calculated positions
			if (!IsOpen)
			{
				// So invoke the PropertyChanged for the adjusting properties as well
				InvokePropertyChanged(nameof(ContentTranslateTransformXCorrection),
					nameof(ContentTranslateTransformYCorrection));
			}
		}

		/// <summary>
		/// Returns the desired Content <see cref="TranslateTransform.X"/> which hides the content (closes the menu)
		/// Helper for <see cref="RecalculateContentTranslate"/>
		/// </summary>
		/// <returns></returns>
		private double CalculateContentTranslateX()
		{
			switch (Position)
			{
				case MenuPosition.Left:
					{
						return -ContentLength;
					}

				case MenuPosition.Right:
					{
						return ContentLength;
					}
			}

			return 0;
		}

		/// <summary>
		/// Returns the desired Content <see cref="TranslateTransform.X"/> which hides the content (closes the menu)
		/// Helper for <see cref="RecalculateContentTranslate"/>
		/// </summary>
		/// <returns></returns>
		private double CalculateContentTranslateY()
		{
			switch (Position)
			{
				case MenuPosition.Top:
					{
						return -ContentLength;
					}

				case MenuPosition.Bottom:
					{
						return ContentLength;
					}
			}

			return 0;
		}

		#endregion

		#region OnLoaded

		/// <summary>
		/// Routine ran when the menu is loaded
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private void OnLoaded(object s, RoutedEventArgs e)
		{
			GetContentStoryboards();
		}

		#endregion

		#region Content Animations

		/// <summary>
		/// Attempts to find the <see cref="Storyboard"/>s responsible for animating the Content from
		/// the RootGrid's resources.
		/// </summary>
		private void GetContentStoryboards()
		{
			_TransitionInContent = (GetTemplateChild("RootGrid") as Grid)?.
				Resources["TransitionInContentStoryboard"] as Storyboard;

			_TransitionOutContent = (GetTemplateChild("RootGrid") as Grid)?.
				Resources["TransitionOutContentStoryboard"] as Storyboard;
		}

		private void ConfigureContentStoryboards()
		{

		}

		#endregion

		#region Command Methods

		/// <summary>
		/// Method for <see cref="IconPressedCommand"/>, changes the presented content
		/// </summary>
		/// <param name="parameter"></param>
		private async void IconPressed(object parameter)
		{
			if(parameter is UIElement newContent)
			{
				Storyboard sbOut = new Storyboard();
				Storyboard sbIn = new Storyboard();

				var anOut = new DoubleAnimation()
				{
					To = 0,
					Duration = new Duration(TimeSpan.FromMilliseconds(75)),
				};

				var anIn = new DoubleAnimation()
				{
					To = 100,
					Duration = new Duration(TimeSpan.FromMilliseconds(75)),
				};
				var target = VisualTreeHelpers.FindChild<ContentPresenter>(this);
				Storyboard.SetTarget(anOut, target);
				Storyboard.SetTarget(anIn, target);
				Storyboard.SetTargetProperty(anOut, "Opacity");
				Storyboard.SetTargetProperty(anIn, "Opacity");
				sbOut.Children.Add(anOut);
				sbIn.Children.Add(anIn);
				sbOut.Completed += (s, e) =>
				{
					SelectedContent = newContent;
					InvokePropertyChanged(nameof(SelectedContent));
					sbIn.Begin();
				};
				sbOut.Begin();
			}
		}

		/// <summary>
		/// Method for <see cref="OpenCloseMenuCommand"/>, toggles the <see cref="IsOpen"/> property.
		/// </summary>
		private void OpenCloseMenu() => IsOpen = !IsOpen;

		private void ChangeContent(UIElement newContent)
		{
			_TransitionInContent.Begin();
		}
		
		private void SelfUnhookableTransitionInCallback(object sender, object e)
		{
			_TransitionInContent.Completed -= SelfUnhookableTransitionInCallback;
		}

		#endregion

		#endregion
	}
}