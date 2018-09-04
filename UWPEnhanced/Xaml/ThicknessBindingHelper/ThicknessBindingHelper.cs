using System.ComponentModel;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Helper class that allows for binding 
	/// </summary>
	public class ThicknessBindingHelper : VisualAttachment, INotifyPropertyChanged
	{
		#region Events

		/// <summary>
		/// Event fired whenever a property changes its value
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Public properties

		/// <summary>
		/// Thickness constructed from dependency properties
		/// </summary>
		public Thickness Value => new Thickness(Left, Top, Right, Bottom);

		#endregion

		#region Left Dependency Property

		/// <summary>
		/// Value assigned to the left property of <see cref="Value"/>
		/// </summary>
		public double Left
		{
			get => (double)GetValue(LeftProperty);
			set => SetValue(LeftProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Left"/>
		/// </summary>
		public static readonly DependencyProperty LeftProperty =
			DependencyProperty.Register(nameof(Left), typeof(double),
			typeof(ThicknessBindingHelper), new PropertyMetadata(default(double),
				new PropertyChangedCallback(NotifyValueChangedCallback)));

		#endregion

		#region Top Dependency Property

		/// <summary>
		/// Value assigned to the top property of <see cref="Value"/>
		/// </summary>
		public double Top
		{
			get => (double)GetValue(TopProperty);
			set => SetValue(TopProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Top"/>
		/// </summary>
		public static readonly DependencyProperty TopProperty =
			DependencyProperty.Register(nameof(Top), typeof(double),
			typeof(ThicknessBindingHelper), new PropertyMetadata(default(double),
				new PropertyChangedCallback(NotifyValueChangedCallback)));

		#endregion

		#region Right Dependency Property

		/// <summary>
		/// Value assigned to the right property of <see cref="Value"/>
		/// </summary>
		public double Right
		{
			get => (double)GetValue(RightProperty);
			set => SetValue(RightProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Right"/>
		/// </summary>
		public static readonly DependencyProperty RightProperty =
			DependencyProperty.Register(nameof(Right), typeof(double),
			typeof(ThicknessBindingHelper), new PropertyMetadata(default(double),
				new PropertyChangedCallback(NotifyValueChangedCallback)));

		#endregion

		#region Bottom Dependency Property

		/// <summary>
		/// Value assigned to the bottom property of <see cref="Value"/>
		/// </summary>
		public double Bottom
		{
			get => (double)GetValue(BottomProperty);
			set => SetValue(BottomProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Bottom"/>
		/// </summary>
		public static readonly DependencyProperty BottomProperty =
			DependencyProperty.Register(nameof(Bottom), typeof(double),
			typeof(ThicknessBindingHelper), new PropertyMetadata(default(double),
				new PropertyChangedCallback(NotifyValueChangedCallback)));

		#endregion

		#region Private static methods

		/// <summary>
		/// If sender is a <see cref="ThicknessBindingHelper"/> and new value differs from the old value, calls property changed event
		/// on sender with <see cref="Value"/> name as argument
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void NotifyValueChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is ThicknessBindingHelper thicknessBindingHelper && e.NewValue != e.OldValue)
			{
				thicknessBindingHelper.PropertyChanged?.Invoke(thicknessBindingHelper, new PropertyChangedEventArgs(nameof(Value)));
			}
		}

		#endregion
	}
}