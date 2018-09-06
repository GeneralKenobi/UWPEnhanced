using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Each child is given equal length in the expanding dimension. If the container's length in the expanding dimension is infinite,
	/// each child is given its desired width in that direction. Non-expanding length given to each child is the maximum out of lengths
	/// desired by any of the children and the available length in that dimension.
	/// </summary>
	public sealed class EqualChildSpaceContainer : ItemsControl
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public EqualChildSpaceContainer()
        {
            this.DefaultStyleKey = typeof(EqualChildSpaceContainer);
			this.Loaded += ItemsContainerLoaded;
        }

		#endregion

		#region Private members

		/// <summary>
		/// Backing store for <see cref="UnderlyingPanel"/>
		/// </summary>
		private EqualChildSpaceContainerPanel mUnderlyingPanel = null;

		#endregion

		#region Public Properties

		/// <summary>
		/// The default direction of items
		/// </summary>
		public static ItemsDirection DefaultFlowDirection { get; } = ItemsDirection.TopToBottom;

		#endregion

		#region FlowDirection Dependency Property

		/// <summary>
		/// Direction of item placement in this container. Hides the inherited <see cref="FrameworkElement.FlowDirection"/>.
		/// </summary>
		public new ItemsDirection FlowDirection
		{
			get => (ItemsDirection)GetValue(FlowDirectionProperty);
			set => SetValue(FlowDirectionProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="FlowDirection"/>
		/// </summary>
		public new static readonly DependencyProperty FlowDirectionProperty =
			DependencyProperty.Register(nameof(FlowDirection), typeof(ItemsDirection),
			typeof(EqualChildSpaceContainer), new PropertyMetadata(DefaultFlowDirection, DirectionChanged));

		#endregion

		#region Private Methods

		/// <summary>
		/// Checks whether <see cref="ItemsControl.ItemsPanelRoot"/> is a <see cref="EqualChildSpaceContainerPanel"/>, if so assigns it
		/// to <see cref="mUnderlyingPanel"/> and assigns initial dependency property values to it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ItemsContainerLoaded(object sender, RoutedEventArgs e)
		{
			if (ItemsPanelRoot is EqualChildSpaceContainerPanel container)
			{
				mUnderlyingPanel = container;

				UpdateDirectionOnUnderylingPanel();
			}
		}

		/// <summary>
		/// Assigns the <see cref="FlowDirection"/> to the <see cref="UnderlyingPanel"/> and forces it to update UI
		/// </summary>
		private void UpdateDirectionOnUnderylingPanel()
		{
			if (mUnderlyingPanel != null)
			{
				mUnderlyingPanel.FlowDirection = FlowDirection;				
			}
		}

		#endregion

		#region Private static methods

		/// <summary>
		/// Callback for when <see cref="FlowDirectionProperty"/> changes
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private static void DirectionChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			if (s is EqualChildSpaceContainer container && e.NewValue != e.OldValue)
			{
				container.UpdateDirectionOnUnderylingPanel();
			}
		}

		#endregion
	}
}