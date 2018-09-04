using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Allows for easy filling of containers with UI elements that don't care what they represent - <see cref="ItemsSource"/> will
	/// be filled with <see cref="byte"/>s and may be bound to providing a fixed number of items displayed by some container, for
	/// example <see cref="ItemsControl"/>
	/// </summary>
	public class DummyItemsSource : VisualAttachment, INotifyPropertyChanged
	{
		#region Events

		/// <summary>
		/// Event fired whenever a property changes its value
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Public properties

		/// <summary>
		/// The items source to bind to
		/// </summary>
		public IEnumerable<byte> ItemsSource { get; private set; }

		#endregion

		#region ItemsCount Dependency Property

		/// <summary>
		/// Number of dummy items to include in the collection
		/// </summary>
		public int ItemsCount
		{
			get => (int)GetValue(ItemsCountProperty);
			set => SetValue(ItemsCountProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="ItemsCount"/>
		/// </summary>
		public static readonly DependencyProperty ItemsCountProperty =
			DependencyProperty.Register(nameof(ItemsCount), typeof(int),
			typeof(DummyItemsSource), new PropertyMetadata(default(int), new PropertyChangedCallback(ItemsCountChangedCallback)));

		#endregion

		#region Private static methods

		/// <summary>
		/// Callback for when <see cref="ItemsCount"/> changes, updates the <see cref="ItemsSource"/> collection and invokes the
		/// <see cref="PropertyChanged"/> event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void ItemsCountChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if(sender is DummyItemsSource source && e.NewValue != e.OldValue && e.NewValue is int newVal)
			{
				source.ItemsSource = new byte[newVal];
				source.PropertyChanged?.Invoke(source, new PropertyChangedEventArgs(nameof(ItemsSource)));
			}
		}

		#endregion
	}
}