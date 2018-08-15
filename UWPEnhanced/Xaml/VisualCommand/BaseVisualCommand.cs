using System.Windows.Input;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Base class for visual commands, contains a dependency property for the command and stores <see cref="IVisualTrigger"/>s
	/// </summary>
	/// <typeparam name="T">Specific type of the used triggers</typeparam>
	public abstract class BaseVisualCommand<T> : VisualAttachmentCollection<T>
		where T : class, IVisualTrigger
	{
		#region Command Dependency Property

		/// <summary>
		/// Command to execute
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
			typeof(BaseVisualCommand<T>), new PropertyMetadata(default(ICommand)));

		#endregion

		#region Protected methods

		/// <summary>
		/// Callback for when trigger fires (it should take care of executing the command)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected abstract void TriggerFiredCallback(object sender, object e);

		/// <summary>
		/// Aside from the base method subscribes to the new trigger
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override T NewElementCheckRoutine(DependencyObject item)
		{
			var initiallyCheckedItem = base.NewElementCheckRoutine(item);

			initiallyCheckedItem.Triggered += TriggerFiredCallback;

			return initiallyCheckedItem;
		}

		/// <summary>
		/// Before running the standard implementation unsubscribes from the trigger
		/// </summary>
		/// <param name="item"></param>
		protected override void CleanupRoutine(T item)
		{
			item.Triggered -= TriggerFiredCallback;

			base.CleanupRoutine(item);
		}

		#endregion
	}
}