using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPEnhanced.Controls
{
	internal class ItemsContainerPanel : Panel
	{
		#region Private Properties

		/// <summary>
		/// The total space between the items in the panel
		/// </summary>
		/// <returns></returns>
		private double TotalItemSpacing => Children.Count > 1 ? (Children.Count - 1) * ItemSpacing : 0;

		#endregion

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

		/// <summary>
		/// Arranges the items in a top to bottom direction. Helper of <see cref="ArrangeOverride(Size)"/>.
		/// </summary>
		/// <param name="finalSize"></param>
		/// <returns></returns>
		private Size ArrangeTopToBottom(Size finalSize)
		{
			double cumulativeHeight = 0;

			foreach(var item in Children)
			{
				item.Measure(finalSize);
				item.Arrange(new Rect(0, cumulativeHeight, item.DesiredSize.Width, item.DesiredSize.Height));
				cumulativeHeight += item.DesiredSize.Height + ItemSpacing;
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

			foreach (var item in Children)
			{
				cumulativeHeight += item.DesiredSize.Height + ItemSpacing;
			}

				foreach (var item in Children)
			{
				item.Measure(finalSize);
				cumulativeHeight -= item.DesiredSize.Height + ItemSpacing;
				item.Arrange(new Rect(0, cumulativeHeight, item.DesiredSize.Width, item.DesiredSize.Height));
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

			foreach(var item in Children)
			{
				item.Measure(finalSize);
				item.Arrange(new Rect(cumulativeWidth, 0, item.DesiredSize.Width, item.DesiredSize.Height));
				cumulativeWidth += item.DesiredSize.Width + ItemSpacing;
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

			foreach (var item in Children)
			{
				cumulativeWidth += item.DesiredSize.Width + ItemSpacing;
			}

			foreach (var item in Children)
			{
				item.Measure(finalSize);
				cumulativeWidth -= item.DesiredSize.Width + ItemSpacing;
				item.Arrange(new Rect(cumulativeWidth, 0, item.DesiredSize.Width, item.DesiredSize.Height));
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

			return new Size(cumulativeChildrenWidth + TotalItemSpacing, greatestHeight);
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

			return new Size(greatestWidth, cumulativeChildrenHeight + TotalItemSpacing);
		}
		
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
		protected override Size MeasureOverride(Size availableSize)
		{
			return (FlowDirection == ItemsDirection.RightToLeft || FlowDirection == ItemsDirection.LeftToRight) ?
				MeasureWhenHorizontal(availableSize) : MeasureWhenVertical(availableSize);
		}

		#endregion		
	}
}
