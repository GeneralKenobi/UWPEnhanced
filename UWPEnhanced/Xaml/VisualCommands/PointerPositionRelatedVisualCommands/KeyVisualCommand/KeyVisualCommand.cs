using CSharpEnhanced.CoreClasses;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Executes the command whenever the contained KeyRoutedEvents are fired, as the command's parameter uses the string version of
	/// the pressed key and the modifiers active during the click (contained in a <see cref="KeyArgument"/>)
	/// </summary>
	public class KeyVisualCommand : BaseVisualCommand<VisualKeyTrigger, KeyRoutedEventArgs>
	{
		#region KeyConverter Dependency Property

		/// <summary>
		/// Converter used to convert a <see cref="VirtualKey"/> to a <see cref="string"/>. If it's not specified, ToString method is
		/// used
		/// </summary>
		public IVirtualKeyConverter KeyConverter
		{
			get => (IVirtualKeyConverter)GetValue(KeyConverterProperty);
			set => SetValue(KeyConverterProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="KeyConverter"/>
		/// </summary>
		public static readonly DependencyProperty KeyConverterProperty =
			DependencyProperty.Register(nameof(KeyConverter), typeof(IVirtualKeyConverter),
			typeof(KeyVisualCommand), new PropertyMetadata(default(IVirtualKeyConverter)));

		#endregion

		#region Protected methods

		/// <summary>
		/// Executes the command, constructs the parameter based on the pressed key and active modifiers
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void TriggerFiredCallback(object sender, KeyRoutedEventArgs e) =>
			Command?.Execute(new KeyArgument(KeyConverter == null ? e.Key.ToString() : KeyConverter.Convert(e.Key),
				// Because the enumeration of KeyModifiers is the same, the cast may be done in a straightforward way
				(KeyModifiers)ModifiersHelpers.GetCurrentModifiersWithoutWindows()));

		#endregion
	}
}