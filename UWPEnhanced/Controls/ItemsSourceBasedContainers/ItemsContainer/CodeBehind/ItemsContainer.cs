using Windows.UI.Xaml;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Basic container for items that puts them in specified order with specified spacing
	/// </summary>
	public sealed class ItemsContainer : BaseFlowDirectionContainer
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
		public static double DefaultItemSpacing { get; } = 5;

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

		#region UseAllAvailableSpace Dependency Property

		/// <summary>
		/// If true, uses all available space rather than only the minimum required to present all children
		/// </summary>
		public bool UseAllAvailableSpace
		{
			get => (bool)GetValue(UseAllAvailableSpaceProperty);
			set => SetValue(UseAllAvailableSpaceProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="UseAllAvailableSpace"/>
		/// </summary>
		public static readonly DependencyProperty UseAllAvailableSpaceProperty =
			DependencyProperty.Register(nameof(UseAllAvailableSpace), typeof(bool),
			typeof(ItemsContainer), new PropertyMetadata(default(bool), UseAllAvailableSpaceChanged));

		#endregion

		#region OuterSpacing Dependency Property

		/// <summary>
		/// If true, spacing will also be added between the edge of the container and the first (and last) child		
		/// </summary>
		public bool OuterSpacing
		{
			get => (bool)GetValue(OuterSpacingProperty);
			set => SetValue(OuterSpacingProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="OuterSpacing"/>
		/// </summary>
		public static readonly DependencyProperty OuterSpacingProperty =
			DependencyProperty.Register(nameof(OuterSpacing), typeof(bool),
			typeof(ItemsContainer), new PropertyMetadata(default(bool), new PropertyChangedCallback(OuterSpacingChanged)));

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
				
				UpdateItemSpacingOnUnderylingPanel();
				UpdateUniformSpacingOnUnderylingPanel();
				UpdateUseAllAvailableSpaceOnUnderylingPanel();
				UpdateOuterSpacingOnUnderylingPanel();
			}
		}

		/// <summary>
		/// Assigns the <see cref="UseAllAvailableSpace"/> value to the <see cref="UnderlyingPanel"/> and forces it to update UI
		/// </summary>
		private void UpdateOuterSpacingOnUnderylingPanel()
		{
			if (mUnderlyingPanel != null)
			{
				mUnderlyingPanel.OuterSpacing = OuterSpacing;
			}
		}

		/// <summary>
		/// Assigns the <see cref="UseAllAvailableSpace"/> value to the <see cref="UnderlyingPanel"/> and forces it to update UI
		/// </summary>
		private void UpdateUseAllAvailableSpaceOnUnderylingPanel()
		{
			if (mUnderlyingPanel != null)
			{
				mUnderlyingPanel.UseAllAvailableSpace = UseAllAvailableSpace;
			}
		}

		/// <summary>
		/// Assigns the <see cref="UniformSpacing"/> value to the <see cref="UnderlyingPanel"/> and forces it to update UI
		/// </summary>
		private void UpdateUniformSpacingOnUnderylingPanel()
		{
			if (mUnderlyingPanel != null)
			{
				mUnderlyingPanel.UniformSpacing = UniformSpacing;
			}
		}

		/// <summary>
		/// Assigns the <see cref="ItemSpacing"/> to the <see cref="UnderlyingPanel"/> and forces it to update UI
		/// </summary>
		private void UpdateItemSpacingOnUnderylingPanel()
		{			
			if (mUnderlyingPanel != null)
			{
				mUnderlyingPanel.ItemSpacing = ItemSpacing;				
			}
		}

		#endregion

		#region Private static methods

		/// <summary>
		/// Callback for when <see cref="OuterSpacingProperty"/> changes
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private static void OuterSpacingChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			if (s is ItemsContainer container && e.NewValue != e.OldValue)
			{
				container.UpdateOuterSpacingOnUnderylingPanel();
			}
		}

		/// <summary>
		/// Callback for when <see cref="UseAllAvailableSpaceProperty"/> changes
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private static void UseAllAvailableSpaceChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			if (s is ItemsContainer container && e.NewValue != e.OldValue)
			{
				container.UpdateUseAllAvailableSpaceOnUnderylingPanel();
			}
		}

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

		#endregion
	}
}