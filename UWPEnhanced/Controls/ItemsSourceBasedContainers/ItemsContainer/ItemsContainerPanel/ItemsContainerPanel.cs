using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Panel for <see cref="ItemsContainer"/>, positions children in 4 possible configurations
	/// </summary>
	internal class ItemsContainerPanel : BaseFlowDirectionContainerPanel
	{
		#region Private properties

		/// <summary>
		/// Returns the number of spacing areas in the container
		/// </summary>
		private int SpacingAreasCount =>
			// Count all visible children. The number of spacings is the number of children minus 1 (one spacing between each two
			// neighbouring children). Additionally, if there should be spacings between edges and outer children, add two to the result)
			Children.Where((x) => x.Visibility == Visibility.Visible).Count() + (OuterSpacing ? 1 : -1);

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
			typeof(ItemsContainerPanel), new PropertyMetadata(ItemsContainer.DefaultItemSpacing, MeasureDeterminingPropertyChanged));

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
			typeof(ItemsContainerPanel), new PropertyMetadata(default(bool), MeasureDeterminingPropertyChanged));

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
			typeof(ItemsContainerPanel), new PropertyMetadata(default(bool), MeasureDeterminingPropertyChanged));

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
			typeof(ItemsContainerPanel), new PropertyMetadata(default(bool), new PropertyChangedCallback(MeasureDeterminingPropertyChanged)));

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
			CalculateTotalItemSpacing(finalSize) / SpacingAreasCount;

		/// <summary>
		/// Returns length of spacing between all items
		/// </summary>
		/// <param name="availableSize"></param>
		/// <returns></returns>
		private double CalculateTotalItemSpacing(Size availableSize)
		{
			// Get the number visible children
			int visibleChildren = Children.Where((x) => x.Visibility == Visibility.Visible).Count();

			// If there are no visible children or there is only one visible child but there are no outer spacing areas, return 0
			if(visibleChildren == 0 || (visibleChildren == 1 && !OuterSpacing))
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
							if (double.IsInfinity(availableSize.Height))
							{
								// If there's an unlimited amount of space it's not possible to determine uniform spacing, return 0
								return 0;
							}
							else
							{
								// The spacing is equal to the available height minus total height of visible children
								return (availableSize.Height - Children.Where((child) => child.Visibility == Visibility.Visible).
									Sum((element) => element.DesiredSize.Height));
							}
						}

					// In case it's a horizontal arrangement
					case ItemsDirection.LeftToRight:
					case ItemsDirection.RightToLeft:
						{
							if (double.IsInfinity(availableSize.Width))
							{
								// If there's an unlimited amount of space it's not possible to determine uniform spacing, return 0
								return 0;
							}
							else
							{
								// The spacing is equal to the available width minus total width of visible children
								return (availableSize.Width - Children.Where((child) => child.Visibility == Visibility.Visible).
									Sum((element) => element.DesiredSize.Width));
							}
						}
											
					default:
						{
							throw new Exception("Unexpected case");
						}
				}
			}
			else
			{
				// For non-uniform spacing return the number of expected spacing areas times ItemSpacing
				return SpacingAreasCount * ItemSpacing;
			}
		}

		/// <summary>
		/// Arranges the items in a top to bottom direction. Helper of <see cref="ArrangeOverride(Size)"/>.
		/// </summary>
		/// <param name="finalSize"></param>
		/// <returns></returns>
		private Size ArrangeTopToBottom(Size finalSize)
		{
			double maxWidth = 0;
			var spacing = CalculateItemSpacing(finalSize);			

			// If there is outer spacing, instead of starting at 0, add the first spacing
			double cumulativeHeight = OuterSpacing ? spacing : 0;

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
			var spacing = CalculateItemSpacing(finalSize);

			// If there is outer spacing, instead of starting at 0, add the first spacing
			double cumulativeHeight = OuterSpacing ? spacing : 0;

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
			var spacing = CalculateItemSpacing(finalSize);

			// If there is outer spacing, instead of starting at 0, add the first spacing
			double cumulativeWidth = OuterSpacing ? spacing : 0;

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
			var spacing = CalculateItemSpacing(finalSize);

			// If there is outer spacing, instead of starting at 0, add the first spacing
			double cumulativeWidth = OuterSpacing ? spacing : 0;

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
		protected override Size MeasureOverride(Size availableSize)
		{
			// Get the required size
			var requiredSize = (FlowDirection == ItemsDirection.RightToLeft || FlowDirection == ItemsDirection.LeftToRight) ?
				MeasureWhenHorizontal(availableSize) : MeasureWhenVertical(availableSize);

			// If using all available space is required
			if (UseAllAvailableSpace)
			{
				// If a dimension has more length available than required, use that length
				requiredSize =
					new Size(Math.Max(requiredSize.Width, availableSize.Width), Math.Max(requiredSize.Height, availableSize.Height));
			}

			return requiredSize;
		}

		#endregion

		#region Private static methods

		/// <summary>
		/// Callback for <see cref="ItemSpacingProperty"/> changed, updates the UI if the new value differs from the old one
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private static void MeasureDeterminingPropertyChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			if(s is ItemsContainerPanel panel && e.NewValue != e.OldValue)
			{
				panel.InvalidateMeasure();
			}
		}

		#endregion
	}
}