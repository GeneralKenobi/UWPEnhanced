using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWPEnhanced.Xaml;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

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
			RecalculateContentTranslate();
        }

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
		public DependencyObjectCollectionOfT<UIElement> Content
		{
			get => (DependencyObjectCollectionOfT<UIElement>)GetValue(ContentProperty);
			set => SetValue(ContentProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Content"/>
		/// </summary>
		public static readonly DependencyProperty ContentProperty =
			DependencyProperty.Register(nameof(Content), typeof(DependencyObjectCollectionOfT<UIElement>),
			typeof(Menu), new PropertyMetadata(default(DependencyObjectCollectionOfT<UIElement>)));

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

		#endregion
	}

}
