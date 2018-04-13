namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Type of transition that may occur in <see cref="IVisualSetup"/>
	/// </summary>
	public enum VisualTransitionType
	{
		/// <summary>
		/// Transition was called from <see cref="IVisualSetup"/> A to <see cref="IVisualSetup"/> B
		/// </summary>
		BetweenSetups = 0,

		/// <summary>
		/// Transition was called from <see cref="IVisualSetup"/> A to <see cref="IVisualSetup"/> A
		/// </summary>
		ToTheSameSetup = 1,
	}
}