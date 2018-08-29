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
		private ItemsContainerPanel mUnderlyingPanel = null;

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
			typeof(ItemsContainer), new PropertyMetadata(DefaultFlowDirection, DirectionChanged));

		#endregion

		#region UniformSpacing Dependency Property

		/// <summary>
		/// If true, items positioned so that space between each two neighbouring items is the same (and <see cref="ItemSpacing"/> will
		/// be ignored). If the container has infinite available width/length (depending on <see cref="FlowDirection"/>) uniform
		/// spacing will not be performed (a finite value is needed to determine uniform spacing value)
		/// </summary>
		public bool UniformSpacing
		{
			get => (bool)GetValue(UniformSpacingProperty);
			set => SetValue(UniformSpacingProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="UniformSpacing"/>
		/// </summary>
		public static readonly DependencyProperty UniformSpacingProperty =
			DependencyProperty.Register(nameof(UniformSpacing), typeof(bool),
			typeof(ItemsContainer), new PropertyMetadata(default(bool), UniformSpacingChanged));

		#endregion

		#region Private Methods

		/// <summary>
		/// Checks whether <see cref="ItemsControl.ItemsPanelRoot"/> is a <see cref="ItemsContainerPanel"/>, if so assigns it to
		/// <see cref="mUnderlyingPanel"/> and assigns initial dependency property values to it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ItemsContainerLoaded(object sender, RoutedEventArgs e)
		{
			if (ItemsPanelRoot is ItemsContainerPanel container)
			{
				mUnderlyingPanel = container;

				UpdateDirectionOnUnderylingPanel();
				UpdateItemSpacingOnUnderylingPanel();
				UpdateUniformSpacingOnUnderylingPanel();
			}
		}

		/// <summary>
		/// Assigns the spacing to the <see cref="UnderlyingPanel"/> and forces it to update UI
		/// </summary>
		private void UpdateUniformSpacingOnUnderylingPanel()
		{
			if (mUnderlyingPanel != null)
			{
				mUnderlyingPanel.UniformSpacing = UniformSpacing;
			}
		}

		/// <summary>
		/// Assigns the spacing to the <see cref="UnderlyingPanel"/> and forces it to update UI
		/// </summary>
		private void UpdateItemSpacingOnUnderylingPanel()
		{			
			if (mUnderlyingPanel != null)
			{
				mUnderlyingPanel.ItemSpacing = ItemSpacing;				
			}
		}

		/// <summary>
		/// Assigns the spacing to the <see cref="UnderlyingPanel"/> and forces it to update UI
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
		/// Callback for when <see cref="UniformSpacingProperty"/> changes
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private static void UniformSpacingChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			if (s is ItemsContainer container && e.NewValue != e.OldValue)
			{
				container.UpdateUniformSpacingOnUnderylingPanel();
			}
		}

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