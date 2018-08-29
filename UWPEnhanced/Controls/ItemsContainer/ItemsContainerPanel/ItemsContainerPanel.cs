using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Panel for <see cref="ItemsContainer"/>, positions children in 4 possible configurations
	/// </summary>
	internal class ItemsContainerPanel : Panel
	{
		#region FlowDirection Dependency Property

		/// <summary>
		/// The direction to present the items in. Hides the inherited <see cref="FrameworkElement.FlowDirection"/>.
		/// Change in value will update the UI automatically
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
			typeof(ItemsContainerPanel), new PropertyMetadata(ItemsContainer.DefaultFlowDirection, FlowDirectionChanged));

		#endregion

		#region ItemSpacing Dependency Property

		/// <summary>
		/// Spacing between two neighbouring items. Change in value will update the UI automatically.
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
			typeof(ItemsContainerPanel), new PropertyMetadata(ItemsContainer.DefaultItemSpacing, ItemSpacingChanged));

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
			typeof(ItemsContainerPanel), new PropertyMetadata(default(bool), ItemSpacingChanged));

		#endregion

		#region Private Methods

		/// <summary>
		/// Calculates the value of item spacing (takes into account <see cref="UniformSpacing"/> and <see cref="ItemSpacing"/> that
		/// should be between two neighbouring items
		/// </summary>
		/// <param name="finalSize"></param>
		/// <returns></returns>
		private double CalculateItemSpacing(Size finalSize) =>
			// Calculate the total spacing and divide it by (n-1) because for n children there are n-1 gaps
			CalculateTotalItemSpacing(finalSize) / (Children.Where((x) => x.Visibility == Visibility.Visible).Count() - 1);

		/// <summary>
		/// Returns length of spacing between all items
		/// </summary>
		/// <param name="availableSize"></param>
		/// <returns></returns>
		private double CalculateTotalItemSpacing(Size availableSize)
		{
			// Get the number visible children
			int visibleChildren = Children.Where((x) => x.Visibility == Visibility.Visible).Count();

			// If there are less than 2 children then return 0
			if(visibleChildren < 2)
			{
				return 0;
			}

			// If the spacing is to be uniform
			if(UniformSpacing)
			{
				switch(FlowDirection)
				{
					// In case it's a vertical arrangement
					case ItemsDirection.TopToBottom:
					case ItemsDirection.BottomToTop:
						{
							// The spacing is equal to the available height minus total height of visible children
							return (availableSize.Height - Children.Where((child) => child.Visibility == Visibility.Visible).
								Sum((element) => element.DesiredSize.Height));
						}

					// In case it's a horizontal arrangement
					case ItemsDirection.LeftToRight:
					case ItemsDirection.RightToLeft:
						{
							// The spacing is equal to the available width minus total width of visible children
							return (availableSize.Width - Children.Where((child) => child.Visibility == Visibility.Visible).
								Sum((element) => element.DesiredSize.Width));
						}
											
					default:
						{
							throw new Exception("Unexpected case");
						}
				}
			}
			else
			{
				// For non-uniform spacing return (n-1)*ItemSpacing: for n items there is n-1 spacings. Remember to exclude collapsed
				// children.
				return (visibleChildren - 1) * ItemSpacing;
			}
		}

		/// <summary>
		/// Arranges the items in a top to bottom direction. Helper of <see cref="ArrangeOverride(Size)"/>.
		/// </summary>
		/// <param name="finalSize"></param>
		/// <returns></returns>
		private Size ArrangeTopToBottom(Size finalSize)
		{
			double cumulativeHeight = 0;
			double maxWidth = 0;
			var spacing = CalculateItemSpacing(finalSize);

			foreach(var item in Children)
			{
				maxWidth = Math.Max(item.DesiredSize.Width, Math.Max(finalSize.Width, maxWidth));

				// If the finalSize width is greater than the desired width, assign it instead so that the control may
				// position itself horizontally as it wishes
				item.Arrange(new Rect(0, cumulativeHeight, Math.Max(item.DesiredSize.Width, finalSize.Width),
					item.DesiredSize.Height));

				// If the item is not visible then treat it as non-existant and do not put spacing around it
				cumulativeHeight += item.DesiredSize.Height + (item.Visibility == Visibility.Visible ? spacing : 0);
			}
			
			return finalSize;
		}

		/// <summary>
		/// Arranges the items in a bottom to top direction. Helper of <see cref="ArrangeOverride(Size)"/>.
		/// </summary>
		/// <param name="finalSize"></param>
		/// <returns></returns>
		private Size ArrangeBottomToTop(Size finalSize)
		{
			double cumulativeHeight = 0;
			var spacing = CalculateItemSpacing(finalSize);

			foreach (var item in Children)
			{
				// If the item is not visible then treat it as non-existant and do not put spacing around it
				cumulativeHeight += item.DesiredSize.Height + (item.Visibility == Visibility.Visible ? spacing : 0);
			}

			foreach (var item in Children)
			{
				// If the item is not visible then treat it as non-existant and do not put spacing around it
				cumulativeHeight -= item.DesiredSize.Height + (item.Visibility == Visibility.Visible ? spacing : 0);

				// If the finalSize width is greater than the desired width, assign it instead so that the control may
				// position itself horizontally as it wishes
				item.Arrange(new Rect(0, cumulativeHeight, Math.Max(item.DesiredSize.Width, finalSize.Width),
					item.DesiredSize.Height));
			}

			return finalSize;
		}

		/// <summary>
		/// Arranges the items left to right. Helper of <see cref="ArrangeOverride(Size)"/>.
		/// </summary>
		/// <param name="finalSize"></param>
		/// <returns></returns>
		private Size ArrangeLeftToRight(Size finalSize)
		{
			double cumulativeWidth = 0;
			var spacing = CalculateItemSpacing(finalSize);

			foreach (var item in Children)
			{
				// If the finalSize height is greater than the desired height, assign it instead so that the control may
				// position itself vertically as it wishes
				item.Arrange(new Rect(cumulativeWidth, 0, item.DesiredSize.Width,
					Math.Max(item.DesiredSize.Height, finalSize.Height)));

				// If the item is not visible then treat it as non-existant and do not put spacing around it
				cumulativeWidth += item.DesiredSize.Width + (item.Visibility == Visibility.Visible ? spacing : 0);
			}

			return finalSize;
		}

		/// <summary>
		/// Arranges the items right to left. Helper of <see cref="ArrangeOverride(Size)"/>.
		/// </summary>
		/// <param name="finalSize"></param>
		/// <returns></returns>
		private Size ArrangeRightToLeft(Size finalSize)
		{
			double cumulativeWidth = 0;
			var spacing = CalculateItemSpacing(finalSize);

			foreach (var item in Children)
			{
				// If the item is not visible then treat it as non-existant and do not put spacing around it
				cumulativeWidth += item.DesiredSize.Width + (item.Visibility == Visibility.Visible ? spacing : 0);
			}

			foreach (var item in Children)
			{
				// If the item is not visible then treat it as non-existant and do not put spacing around it
				cumulativeWidth -= item.DesiredSize.Width + (item.Visibility == Visibility.Visible ? spacing : 0);

				// If the finalSize height is greater than the desired height, assign it instead so that the control may
				// position itself vertically as it wishes
				item.Arrange(new Rect(cumulativeWidth, 0, item.DesiredSize.Width,
					Math.Max(item.DesiredSize.Height, finalSize.Height)));
			}

			return finalSize;
		}

		/// <summary>
		/// Measures the total size of the panel in horizontal position. Helper of <see cref="MeasureOverride(Size)"/>.
		/// </summary>
		/// <param name="availableSize"></param>
		/// <returns></returns>
		private Size MeasureWhenHorizontal(Size availableSize)
		{
			double greatestHeight = 0;
			double cumulativeChildrenWidth = 0;

			// For each child
			foreach (var item in Children)
			{
				// Measure it
				item.Measure(availableSize);

				// Add its width to the total width of the panel
				cumulativeChildrenWidth += item.DesiredSize.Width;

				// If its height is the greatest save it
				greatestHeight = Math.Max(greatestHeight, item.DesiredSize.Height);
			}

			return new Size(cumulativeChildrenWidth + CalculateTotalItemSpacing(availableSize), greatestHeight);
		}

		/// <summary>
		/// Measures the total size in vertical position. Helper of <see cref="MeasureOverride(Size)"/>.
		/// </summary>
		/// <param name="availableSize"></param>
		/// <returns></returns>
		private Size MeasureWhenVertical(Size availableSize)
		{
			double greatestWidth = 0;
			double cumulativeChildrenHeight = 0;

			// For each child
			foreach (var item in Children)
			{
				// Measure it
				item.Measure(availableSize);

				// If its width is the greatest save it
				greatestWidth = Math.Max(greatestWidth, item.DesiredSize.Width);

				// Add its height to the total height of the panel
				cumulativeChildrenHeight += item.DesiredSize.Height;
			}
			
			return new Size(greatestWidth, cumulativeChildrenHeight + CalculateTotalItemSpacing(availableSize));
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Rearranges the items in the desired direction
		/// </summary>
		/// <param name="finalSize"></param>
		/// <returns></returns>
		protected override Size ArrangeOverride(Size finalSize)
		{
			switch (FlowDirection)
			{
				case ItemsDirection.RightToLeft:
					{
						return ArrangeRightToLeft(finalSize);
					}

				case ItemsDirection.BottomToTop:
					{
						return ArrangeBottomToTop(finalSize);
					}

				case ItemsDirection.LeftToRight:
					{
						return ArrangeLeftToRight(finalSize);
					}

				case ItemsDirection.TopToBottom:
					{
						return ArrangeTopToBottom(finalSize);
					}

				default:
					{
						throw new Exception("Unsupported case; This shouldn't happen");
					}
			}
		}

		/// <summary>
		/// Measures the total size of the container
		/// </summary>
		/// <param name="availableSize"></param>
		/// <returns></returns>
		protected override Size MeasureOverride(Size availableSize) =>
			(FlowDirection == ItemsDirection.RightToLeft || FlowDirection == ItemsDirection.LeftToRight) ?
				MeasureWhenHorizontal(availableSize) : MeasureWhenVertical(availableSize);

		#endregion

		#region Private static methods

		/// <summary>
		/// Callback for <see cref="ItemSpacingProperty"/> changed, updates the UI if the new value differs from the old one
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private static void ItemSpacingChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			if(s is ItemsContainerPanel panel && e.NewValue != e.OldValue)
			{
				panel.InvalidateMeasure();
			}
		}

		/// <summary>
		/// Callback for <see cref="FlowDirectionProperty"/> changed, updates the UI if the new value differs from the old one
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private static void FlowDirectionChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			if (s is ItemsContainerPanel panel && e.NewValue is ItemsDirection newDirection &&
				e.OldValue is ItemsDirection oldDirection && newDirection != oldDirection)
			{
				// If the change didn't change the axis of item placement (ex: old and new placement are both vertical)
				if((newDirection == ItemsDirection.LeftToRight && oldDirection == ItemsDirection.RightToLeft) ||
					(newDirection == ItemsDirection.RightToLeft && oldDirection == ItemsDirection.LeftToRight) ||
					(newDirection == ItemsDirection.TopToBottom && oldDirection == ItemsDirection.BottomToTop) ||
					(newDirection == ItemsDirection.BottomToTop && oldDirection == ItemsDirection.TopToBottom))
				{
					// Then only rearrange the items
					panel.InvalidateArrange();
				}
				else
				{
					// Otherwise remeasure and rearrange
					panel.InvalidateMeasure();
				}
			}
		}

		#endregion
	}
}