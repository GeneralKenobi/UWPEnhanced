using System;
using System.Windows.Input;

namespace TestEnvironment
{
	/// <summary>
	/// Class for a RelayCommand with a parameter
	/// </summary>
	public class RelayParametrizedCommand : ICommand
	{
		#region Private Members

		/// <summary>
		/// The action to run
		/// </summary>
		private Action<object> mAction;

		#endregion

		#region Public Events

		/// <summary>
		/// The event that is fired when the <see cref="CanExecute(object)"/> value has changed
		/// </summary>
		public event EventHandler CanExecuteChanged = (sender, e) => { };

		#endregion

		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="action">Action to perform</param>
		public RelayParametrizedCommand(Action<object> action)
		{
			mAction = action;
		}

		#endregion

		#region Command Methods

		/// <summary>
		/// A relay command can always execute
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public bool CanExecute(object parameter) => true;

		/// <summary>
		/// Executes mAction
		/// </summary>
		/// <param name="parameter"></param>
		public void Execute(object parameter)
		{			
			mAction(parameter);
		}

		#endregion
	}
}
