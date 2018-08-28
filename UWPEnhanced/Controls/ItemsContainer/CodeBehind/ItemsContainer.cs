using UWPEnhanced.Xaml;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Basic container for items that puts them in specified order with specified spacing
	/// </summary>
	public sealed class ItemsContainer : ItemsControl
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public ItemsContainer()
        {
            this.DefaultStyleKey = typeof(ItemsContainer);
			this.Loaded += ItemsContainerLoaded;
        }

		#endregion

		#region Private members

		/// <summary>
		/// Backing store for <see cref="UnderlyingPanel"/>
		/// </summary>
		private ItemsContainerPanel _UnderlyingPanel = null;

		#endregion

		#region Private properties

		/// <summary>
		/// The <see cref="ItemsContainerPanel"/> that is associated with this control
		/// </summary>
		private ItemsContainerPanel UnderlyingPanel => (_UnderlyingPanel != null || this.TryFindChild(out _UnderlyingPanel)) ?
			_UnderlyingPanel : null;

		#endregion

		#region Public Properties

		/// <summary>
		/// The default spacing between items
		/// </summary>
		public static double DefaultItemSpacing => 5;

		/// <summary>
		/// The default direction of items
		/// </summary>
		public static ItemsDirection DefaultFlowDirection => ItemsDirection.TopToBottom;

		#endregion

		#region ItemSpacing Dependency Property

		/// <summary>
		/// Space between the items. Default is 5
		/// </summary>
		public double ItemSpacing
		{
			get => (double)GetValue(ItemSpacingProperty);
			set => SetValue(ItemSpacingProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="ItemSpacing"/>
		/// </summary>
		public static readonly DependencyProperty ItemSpacingProperty =
			DependencyProperty.Register(nameof(ItemSpacing), typeof(double),
			typeof(ItemsContainer), new PropertyMetadata(DefaultItemSpacing, ItemSpacingChanged));

		#endregion

		#region Direction Dependency Property

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
		public static readonly DependencyProperty FlowDirectionProperty =
			DependencyProperty.Register(nameof(FlowDirection), typeof(ItemsDirection),
			typeof(ItemsContainer), new PropertyMetadata(DefaultFlowDirection, DirectionChanged));

		#endregion

		#region Private Methods

		/// <summary>
		/// When the control is loaded assigns the first <see cref="ItemSpacing"/> value to the <see cref="UnderlyingPanel"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ItemsContainerLoaded(object sender, RoutedEventArgs e)
		{
			UpdateDirectionOnUnderylingPanel();
			UpdateItemSpacingOnUnderylingPanel();
		}

		/// <summary>
		/// Assigns the spacing to the <see cref="UnderlyingPanel"/> and forces it to update UI
		/// </summary>
		private void UpdateItemSpacingOnUnderylingPanel()
		{			
			if (UnderlyingPanel != null)
			{
				UnderlyingPanel.ItemSpacing = ItemSpacing;				
			}
		}

		/// <summary>
		/// Assigns the spacing to the <see cref="UnderlyingPanel"/> and forces it to update UI
		/// </summary>
		private void UpdateDirectionOnUnderylingPanel()
		{
			if (UnderlyingPanel != null)
			{
				UnderlyingPanel.FlowDirection = FlowDirection;				
			}
		}

		#endregion

		#region Private static methods

		/// <summary>
		/// Callback for when <see cref="ItemSpacingProperty"/> changes
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private static void ItemSpacingChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			if (s is ItemsContainer container && e.NewValue != e.OldValue)
			{
				container.UpdateItemSpacingOnUnderylingPanel();
			}
		}

		/// <summary>
		/// Callback for when <see cref="ItemSpacingProperty"/> changes
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private static void DirectionChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			if (s is ItemsContainer container && e.NewValue != e.OldValue)
			{
				container.UpdateDirectionOnUnderylingPanel();
			}
		}

		#endregion
	}
}