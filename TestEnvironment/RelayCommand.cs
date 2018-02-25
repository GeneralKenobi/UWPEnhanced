using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestEnvironment
{
	public class RelayCommand : ICommand
	{
		#region Private Members

		/// <summary>
		/// The action to run
		/// </summary>
		private Action mAction;

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
		public RelayCommand(Action action)
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
			mAction();
		}

		#endregion		
	}
}
