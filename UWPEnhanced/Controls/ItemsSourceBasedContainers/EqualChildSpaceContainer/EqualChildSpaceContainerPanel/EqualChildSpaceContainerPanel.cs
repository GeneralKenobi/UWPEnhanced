using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Panel for <see cref="EqualChildSpaceContainer"/>, positions children in 4 possible configurations and gives each child an
	/// equal amount of space in the flow direction's axis
	/// </summary>
	internal class EqualChildSpaceContainerPanel : BaseFlowDirectionContainerPanel
	{
		#region Private Methods

		/// <summary>
		/// Arranges the items in a vertical position, from top to bottom. Helper of <see cref="ArrangeOverride(Size)"/>.
		/// </summary>
		/// <param name="orderedChildren">Children ordered for placement</param>
		/// <param name="width">Width to assign to each child</param>
		/// <param name="height">Height to assign to each child</param>
		/// <returns></returns>
		private void ArrangeVerticalForEqualSpace(IEnumerable<UIElement> orderedChildren, double width, double height)
		{
			double cumulativeHeight = 0;
			
			// Arrange all children (give them their width, height and y offset that is accumulated based on already positioned children)
			foreach (var item in orderedChildren)
			{
				item.Arrange(new Rect(0, cumulativeHeight, width, height));

				cumulativeHeight += height;				
			}
		}

		/// <summary>
		/// Arranges the items in a vertical position, from top to bottom. Each children is assigned its desired height.
		/// Helper of <see cref="ArrangeOverride(Size)"/>.
		/// </summary>
		/// <param name="orderedChildren">Children ordered for placement</param>
		/// <param name="width">Width to assign to each child</param>		
		/// <returns></returns>
		private void ArrangeVerticalForDesiredSpace(IEnumerable<UIElement> orderedChildren, double width)
		{
			double cumulativeHeight = 0;

			// Arrange all children (give them their width, height and y offset that is accumulated based on already positioned children)
			foreach (var item in orderedChildren)
			{				
				item.Arrange(new Rect(0, cumulativeHeight, width, item.DesiredSize.Height));
				
				cumulativeHeight += item.DesiredSize.Height;
			}
		}

		/// <summary>
		/// Arranges the items in a horizontal position, from left to right. Helper of <see cref="ArrangeOverride(Size)"/>.
		/// </summary>
		/// <param name="orderedChildren">Children ordered for placement</param>
		/// <param name="width">Width to assign to each child</param>
		/// <param name="height">Height to assign to each child</param>
		/// <returns></returns>
		private void ArrangeHorizontalForEqualSpace(IEnumerable<UIElement> orderedChildren, double width, double height)
		{
			double cumulativeWidth = 0;

			// Arrange all children (give them their width, height and x offset that is accumulated based on already positioned children)
			foreach (var item in orderedChildren)
			{
				item.Arrange(new Rect(cumulativeWidth, 0, width, height));
				
				cumulativeWidth += width;
			}
		}

		/// <summary>
		/// Arranges the items in a horizontal position, from left to right. Each children is assigned its desired width.
		/// Helper of <see cref="ArrangeOverride(Size)"/>.
		/// </summary>
		/// <param name="orderedChildren">Children ordered for placement</param>
		/// <param name="height">Height to assign to each child</param>
		/// <returns></returns>
		private void ArrangeHorizontalForDesiredSpace(IEnumerable<UIElement> orderedChildren, double height)
		{
			double cumulativeWidth = 0;

			// Arrange all children (give them their width, height and x offset that is accumulated based on already positioned children)
			foreach (var item in orderedChildren)
			{
				item.Arrange(new Rect(cumulativeWidth, 0, item.DesiredSize.Width, height));
				
				cumulativeWidth += item.DesiredSize.Width;
			}
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
			// Arrange all children that have collapsed visibility
			ArrangeInvisibleChildren();

			// Order the children properly (RightToLeft and BottomToTop warrant reverse order for proper placement) and filter out
			// children that are collapsed (not visible)
			var orderedVisibleChildren = (FlowDirection == ItemsDirection.LeftToRight || FlowDirection == ItemsDirection.TopToBottom ?
				Children : Children.Reverse()).Where((child) => child.Visibility == Visibility.Visible);

			switch (FlowDirection)
			{
				case ItemsDirection.LeftToRight:
				case ItemsDirection.RightToLeft:
					{
						// Height is the maximum value out of desired height of any child and the available height
						double childHeight = Children.Count > 0 ?
							Math.Max(finalSize.Height, Children.Max((child) => child.DesiredSize.Height)) : finalSize.Height;

						if (double.IsInfinity(finalSize.Height))
						{
							ArrangeHorizontalForDesiredSpace(orderedVisibleChildren, childHeight);
						}
						else
						{
							ArrangeHorizontalForEqualSpace(
								orderedVisibleChildren, finalSize.Width / orderedVisibleChildren.Count(), childHeight);
						}
					}
					break;

				case ItemsDirection.BottomToTop:
				case ItemsDirection.TopToBottom:
					{
						// Width is the maximum value out of desired width of any child and the available width
						double childWidth = Children.Count > 0 ?
							Math.Max(finalSize.Width, Children.Max((child) => child.DesiredSize.Width)) : finalSize.Width;

						if (double.IsInfinity(finalSize.Height))
						{
							ArrangeVerticalForDesiredSpace(orderedVisibleChildren, childWidth);
						}
						else
						{
							ArrangeVerticalForEqualSpace(
								orderedVisibleChildren, childWidth, finalSize.Height / orderedVisibleChildren.Count());
						}
					} break;

				default:
					{
						throw new Exception("Unsupported case; This shouldn't happen");
					}
			}

			return finalSize;
		}

		/// <summary>
		/// Measures the total size of the container
		/// </summary>
		/// <param name="availableSize"></param>
		/// <returns></returns>
		protected override Size MeasureOverride(Size availableSize) => availableSize;

		#endregion
	}
}