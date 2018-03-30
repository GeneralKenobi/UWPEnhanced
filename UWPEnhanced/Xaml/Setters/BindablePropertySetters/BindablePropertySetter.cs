using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Setter based on binding of the target property
	/// </summary>
	public class BindablePropertySetter : BaseSetter
	{
		#region Property Dependency Property

		/// <summary>
		/// Property to assign. The binding should be done like this (TwoWay mode is necessary):
		/// Property = {Binding ElementName=*ObjectName*, Path=*Property*, Mode=TwoWay}
		/// In case of attached properties the Path should look like this (example): Path = (Grid.Row)
		/// </summary>
		public object Property
		{ 
			get => (object)GetValue(PropertyProperty);
			set => SetValue(PropertyProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Property"/>
		/// </summary>
		public static readonly DependencyProperty PropertyProperty =
			DependencyProperty.Register(nameof(Property), typeof(object),
			typeof(BindablePropertySetter), new PropertyMetadata(null));

		#endregion

		#region Private Static Methods

		/// <summary>
		/// Callback for <see cref="Property"/> and <see cref="Target"/> which determines the expected type of <see cref="Value"/>
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private static void PropertyChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			if (s is BindablePropertySetter setter && e.NewValue != e.OldValue && setter.Property != null)
			{
				setter.ExpectedType = setter.Property.GetType();
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Sets the value on the property
		/// </summary>
		public override void Set() => Property = Value;

		#endregion
	}
}