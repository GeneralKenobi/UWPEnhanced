using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Class allowing for creation and usage of objects that don't derive from <see cref="DependencyObject"/> (eg. because they have
	/// to derive from a different base class).
	/// </summary>
	[ContentProperty(Name = nameof(Value))]
	public class AttachableObjectWrapper : VisualAttachment
	{
		#region Value Dependency Property

		/// <summary>
		/// The object to create and wrap in xaml files.
		/// </summary>
		public object Value
		{
			get => (object)GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Value"/>
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(nameof(Value), typeof(object),
			typeof(AttachableObjectWrapper), new PropertyMetadata(default(object)));

		#endregion
	}
}