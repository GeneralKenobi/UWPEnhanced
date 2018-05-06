using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace UWPEnhanced.Controls
{
	internal class ItemsContainerPanel : Panel
	{
		public ItemsDirection Direction { get; set; } = ItemsDirection.ToBottom;

		public double ItemSpacing { get; set; } = 0;


		protected override Size ArrangeOverride(Size finalSize)
		{
			return ArrangeToRight(finalSize);

		}

		private Size ArrangeToBottom(Size finalSize)
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

		private Size ArrangeToTop(Size finalSize)
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



		private Size ArrangeToLeft(Size finalSize)
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

		private Size ArrangeToRight(Size finalSize)
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

		protected override Size MeasureOverride(Size availableSize)
		{
			return (Direction == ItemsDirection.ToLeft || Direction == ItemsDirection.ToRight) ?
				MeasureWhenHorizontal(availableSize) : MeasureWhenVertical(availableSize);
		}

		private Size MeasureWhenHorizontal(Size availableSize)
		{
			double greatestHeight = 0;
			double cumulativeWidth = 0;

			// For each child
			foreach (var item in Children)
			{
				// Measure it
				item.Measure(availableSize);

				// Add its width to the total width of the panel
				cumulativeWidth += item.DesiredSize.Width;

				// If its height is the greatest save it
				greatestHeight = Math.Max(greatestHeight, item.DesiredSize.Height);
			}

			return new Size(cumulativeWidth, greatestHeight);
		}

		private Size MeasureWhenVertical(Size availableSize)
		{
			double greatestWidth = 0;
			double cumulativeHeight = 0;

			// For each child
			foreach (var item in Children)
			{
				// Measure it
				item.Measure(availableSize);
				
				// If its width is the greatest save it
				greatestWidth = Math.Max(greatestWidth, item.DesiredSize.Width);

				// Add its height to the total height of the panel
				cumulativeHeight += item.DesiredSize.Height;
			}

			return new Size(greatestWidth, cumulativeHeight);
		}
	}
}
