using CSharpEnhanced.Helpers;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Setter based on Target object and PropertyPath. For Attached Properties use <see cref="BindablePropertySetter"/>
	/// </summary>
	public class PropertySetter : BaseSetter
	{
		#region Property Dependency Property

		/// <summary>
		/// Property to assign using the setter
		/// </summary>
		public PropertyPath Property
		{
			get => (PropertyPath)GetValue(PropertyProperty);
			set => SetValue(PropertyProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Property"/>
		/// </summary>
		public static readonly DependencyProperty PropertyProperty =
			DependencyProperty.Register(nameof(Property), typeof(PropertyPath),
			typeof(PropertySetter), new PropertyMetadata(default(PropertyPath), TargetPropertyChanged));

		#endregion

		#region Target Dependency Property

		/// <summary>
		/// Target of the setter. Simple x:Bind is enough, TwoWay
		/// </summary>
		public object Target
		{
			get => GetValue(TargetProperty);
			set => SetValue(TargetProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Target"/>
		/// </summary>
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register(nameof(Target), typeof(object), typeof(PropertySetter),
				new PropertyMetadata(default(object), TargetPropertyChanged));

		#endregion

		#region Private Static Methods

		/// <summary>
		/// Callback for <see cref="Property"/> and <see cref="Target"/> which determines the expected type of <see cref="Value"/>
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private static void TargetPropertyChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			if(s is PropertySetter setter && e.NewValue != e.OldValue && setter.Target != null && setter.Property != null)
			{
				// Get the property
				var property = setter.Target.GetType().GetProperty(setter.Property.Path);
				// If successful (property != null), set it as the expected type
				setter.ExpectedType = property?.PropertyType;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Sets the value on the property
		/// </summary>
		public override void Set() => PropertyHelpers.TrySetPropertyValue(Target, Property.Path, Value);

		#endregion
	}
}