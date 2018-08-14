using System;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Event args for <see cref="GenericVisualDataTrigger{T}"/>
	/// </summary>
	public class VisualDataTriggerEventArgs<T> : EventArgs
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public VisualDataTriggerEventArgs(T value)
		{
			Value = value;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Value that triggered the <see cref="IVisualTrigger{T}"/>
		/// </summary>
		public T Value { get; }

		#endregion
	}
}