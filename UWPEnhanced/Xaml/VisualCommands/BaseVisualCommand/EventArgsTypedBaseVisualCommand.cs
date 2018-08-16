using System;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// <see cref="BaseVisualCommand{T}"/> that provides a TriggerFiredCallback method with the argument casted to
	/// <see cref="TEventArgs"/>. It will throw an exception if the cast is not possible.
	/// </summary>
	/// <typeparam name="TVisualTrigger">Specific type of the used triggers</typeparam>
	/// <typeparam name="TEventArgs">Type of the event to cast to</typeparam>
	public abstract class BaseVisualCommand<TVisualTrigger, TEventArgs> : BaseVisualCommand<TVisualTrigger>
		where TVisualTrigger : class, IVisualTrigger
	{
		#region Protected methods

		/// <summary>
		/// Tries to cast <paramref name="e"/> to <see cref="TEventArgs"/> and then call
		/// <see cref="TriggerFiredCallback(object, TEventArgs)"/>, if unsuccessful throws an exception
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected sealed override void TriggerFiredCallback(object sender, object e)
		{
			if (e is TEventArgs castedE)
			{
				TriggerFiredCallback(sender, castedE);
			}
			else
			{
				throw new Exception("A trigger fired with an argument that is not a " + nameof(TEventArgs));
			}
		}

		/// <summary>
		/// Callback for when trigger fires (it should take care of executing the command), fired with event argument casted to
		/// <see cref="TEventArgs"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected abstract void TriggerFiredCallback(object sender, TEventArgs e);

		#endregion
	}
}