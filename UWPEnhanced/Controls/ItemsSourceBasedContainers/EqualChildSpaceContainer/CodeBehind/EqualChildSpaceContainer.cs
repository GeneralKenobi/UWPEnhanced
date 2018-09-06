namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Each child is given equal length in the expanding dimension. If the container's length in the expanding dimension is infinite,
	/// each child is given its desired width in that direction. Non-expanding length given to each child is the maximum out of lengths
	/// desired by any of the children and the available length in that dimension.
	/// </summary>
	public sealed class EqualChildSpaceContainer : BaseFlowDirectionContainer
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public EqualChildSpaceContainer()
        {
            this.DefaultStyleKey = typeof(EqualChildSpaceContainer);
        }

		#endregion
	}
}