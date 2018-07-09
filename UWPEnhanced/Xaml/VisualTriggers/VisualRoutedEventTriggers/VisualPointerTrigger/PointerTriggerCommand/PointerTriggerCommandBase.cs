using System;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// A base class for in-xaml binding of commands that are triggered by pointer routed events
	/// </summary>
	public abstract class PointerTriggerCommandBase : VisualPointerTrigger
	{
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public PointerTriggerCommandBase()
		{
			this.Triggered += ExecuteCommand;
		}

		#endregion

		#region Command Dependency Property

		/// <summary>
		/// The command to execute
		/// </summary>
		public ICommand Command
		{
			get => (ICommand)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Command"/>
		/// </summary>
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register(nameof(Command), typeof(ICommand),
			typeof(PointerTriggerCommandBase), new PropertyMetadata(default(ICommand)));

		#endregion

		#region Protected methods

		/// <summary>
		/// Executes the command whenever <see cref="IVisualTrigger.Triggered"/> fires
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected abstract void ExecuteCommand(object sender, EventArgs e);

		#endregion
	}
}