using CSharpEnhanced.ICommands;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.Controls
{
	[ContentProperty(Name = nameof(Content))]
	public sealed class DropDownControl : Control, INotifyPropertyChanged
	{
		#region Events

		/// <summary>
		/// Event fired whenever a property changes its value
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public DropDownControl()
		{
			this.DefaultStyleKey = typeof(DropDownControl);

			ToggleControlStateCommand = new RelayCommand(ToggleControlState);
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Command that toggles the state of the control - control goes from collapsed/dropped to dropped/collapsed
		/// </summary>
		public ICommand ToggleControlStateCommand { get; }

		#endregion

		#region Dependency properties

		#region Header Dependency Property

		/// <summary>
		/// Text displayed in the header
		/// </summary>
		public string HeaderText
		{
			get => (string)GetValue(HeaderProperty);
			set => SetValue(HeaderProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="HeaderText"/>
		/// </summary>
		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register(nameof(HeaderText), typeof(string),
			typeof(DropDownControl), new PropertyMetadata(default(string)));

		#endregion

		#region HeaderHeight Dependency Property

		/// <summary>
		/// Height of the header
		/// </summary>
		public double HeaderHeight
		{
			get => (double)GetValue(HeaderHeightProperty);
			set => SetValue(HeaderHeightProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="HeaderHeight"/>
		/// </summary>
		public static readonly DependencyProperty HeaderHeightProperty =
			DependencyProperty.Register(nameof(HeaderHeight), typeof(double),
			typeof(DropDownControl), new PropertyMetadata(default(double),
				new PropertyChangedCallback((s, e) => (s as DropDownControl)?.PropertyChanged?.Invoke(s, new PropertyChangedEventArgs(nameof(HeaderHeight))))));
		#endregion

		#region IsDropped Dependency Property

		/// <summary>
		/// Represents the state of the control - true when it is dropped (expanded) and false when it is collapsed. It can be set to control
		/// the control programatically (though it still will be toggle'able from UI).
		/// </summary>
		public bool IsDropped
		{
			get => (bool)GetValue(IsDroppedProperty);
			set => SetValue(IsDroppedProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="IsDropped"/>
		/// </summary>
		public static readonly DependencyProperty IsDroppedProperty =
			DependencyProperty.Register(nameof(IsDropped), typeof(bool),
			typeof(DropDownControl), new PropertyMetadata(default(bool), NotifyPropertyChangedCallback(nameof(IsDropped))));

		#endregion

		#region Content Dependency Property

		/// <summary>
		/// Content presented upon drop-down
		/// </summary>
		public UIElement Content
		{
			get => (UIElement)GetValue(ContentProperty);
			set => SetValue(ContentProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Content"/>
		/// </summary>
		public static readonly DependencyProperty ContentProperty =
			DependencyProperty.Register(nameof(Content), typeof(UIElement),
			typeof(DropDownControl), new PropertyMetadata(default(UIElement)));

		#endregion

		#endregion

		#region Private methods

		/// <summary>
		/// Method for <see cref="ToggleControlStateCommand"/>
		/// </summary>
		private void ToggleControlState() => IsDropped = !IsDropped;

		#endregion

		#region Private static methods

		/// <summary>
		/// Generates a <see cref="PropertyChangedCallback"/> that invokes <see cref="PropertyChanged"/> event with <paramref name="propertyName"/>
		/// used to construct <see cref="PropertyChangedEventArgs"/> whenever the sender can be cast to <see cref="DropDownControl"/>.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		private static PropertyChangedCallback NotifyPropertyChangedCallback(string propertyName) =>
			new PropertyChangedCallback((s, e) => (s as DropDownControl)?.PropertyChanged?.Invoke(s, new PropertyChangedEventArgs(propertyName)));

		#endregion
	}
}