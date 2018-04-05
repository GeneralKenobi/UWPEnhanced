using CSharpEnhanced.Helpers;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Setter based on Target object and PropertyPath, can restore the old value of the property
	/// </summary>
	public class TemporaryPropertySetter : PropertySetter, ITemporaryVisualSetter
	{
		#region Public Properties

		/// <summary>
		/// The old value remembered by the setter
		/// </summary>
		public object OldValue { get; private set; }

		#endregion

		#region Public Methods

		/// <summary>
		/// Restores the old value on the property
		/// </summary>
		public void Reset() => PropertyHelpers.SetPropertyValue(Target, Property.Path, OldValue);

		/// <summary>
		/// Sets the value on the property and remembers the old value
		/// </summary>
		public override void Set()
		{
			OldValue = PropertyHelpers.GetPropertyValue(Target, Property.Path);
			base.Set();
		}

		#endregion
	}
}