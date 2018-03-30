namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Setter based on binding of the target property, can restore the old value of the property
	/// </summary>
	public class TemporaryBindablePropertySetter : BindablePropertySetter, ITemporaryVisualSetter
	{
		#region Public Properties

		/// <summary>
		/// The old value remembered by the setter
		/// </summary>
		public object OldValue { get; private set; }

		#endregion

		#region Public Methods

		/// <summary>
		/// Sets the value on the property and remembers the old value
		/// </summary>
		public override void Set()
		{
			OldValue = Property;
			base.Set();
		}

		/// <summary>
		/// Restores the value of the property
		/// </summary>
		public void Reset() => Property = OldValue;

		#endregion
	}
}