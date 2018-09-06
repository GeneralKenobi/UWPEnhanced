using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Base class for containers that position children based on <see cref="ItemsDirection"/>
	/// </summary>
	public abstract class BaseFlowDirectionContainer : ItemsControl
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public BaseFlowDirectionContainer()
        {
			this.Loaded += ItemsContainerLoaded;
        }

		#endregion

		#region Private members

		/// <summary>
		/// Backing store for <see cref="UnderlyingPanel"/>
		/// </summary>
		private BaseFlowDirectionContainerPanel mUnderlyingPanel = null;

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
			typeof(BaseFlowDirectionContainer), new PropertyMetadata(ItemsDirection.TopToBottom, DirectionChanged));

		#endregion

		#region Private Methods

		/// <summary>
		/// Checks whether <see cref="ItemsControl.ItemsPanelRoot"/> is a <see cref="BaseFlowDirectionContainerPanel"/>, if so assigns
		/// it to <see cref="mUnderlyingPanel"/> and assigns initial dependency property values to it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ItemsContainerLoaded(object sender, RoutedEventArgs e)
		{
			if (ItemsPanelRoot is BaseFlowDirectionContainerPanel container)
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
			if (s is BaseFlowDirectionContainer container && e.NewValue != e.OldValue)
			{
				container.UpdateDirectionOnUnderylingPanel();
			}
		}

		#endregion
	}
}