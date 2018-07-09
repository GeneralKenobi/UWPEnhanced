using System;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// A class for in-xaml binding of commands that are triggered by pointer tapped event and whose parameter is the pointer position
	/// relative to some entity casted to a <see cref="Complex"/>
	/// </summary>
	public class TappedTriggerCommand : TappedTriggerCommandBase
	{
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public TappedTriggerCommand()
		{
			this.Triggered += ExecuteCommand;
		}

		#endregion	

		#region CommandParameter Dependency Property

		/// <summary>
		/// Parameter to pass to the command
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
			typeof(TappedTriggerCommand), new PropertyMetadata(default(object)));

		#endregion

		#region Protected methods

		/// <summary>
		/// Executes the command whenever <see cref="IVisualTrigger.Triggered"/> fires
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>		
		protected override void ExecuteCommand(object sender, EventArgs e) => Command?.Execute(CommandParameter);

		#endregion
	}
}