using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Visual command which has its parameter as a dependency property
	/// </summary>
	public class ParametrizedVisualCommand : BaseVisualCommand<IVisualTrigger>
	{
		#region CommandParameter Dependency Property

		/// <summary>
		/// Parameter to pass when invoking <see cref="Command"/>
		/// </summary>
		public object CommandParameter
		{
			get => GetValue(CommandParameterProperty);
			set => SetValue(CommandParameterProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="CommandParameter"/>
		/// </summary>
		public static readonly DependencyProperty CommandParameterProperty =
			DependencyProperty.Register(nameof(CommandParameter), typeof(object),
			typeof(ParametrizedVisualCommand), new PropertyMetadata(default(object)));

		#endregion

		#region Protected methods

		/// <summary>
		/// Executes the command and passes <see cref="CommandParameter"/> as its parameter
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void TriggerFiredCallback(object sender, object e) => Command?.Execute(CommandParameter);

		#endregion
	}
}