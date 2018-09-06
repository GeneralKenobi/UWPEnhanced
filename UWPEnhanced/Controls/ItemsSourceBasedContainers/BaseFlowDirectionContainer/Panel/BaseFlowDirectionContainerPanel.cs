using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Base class for <see cref="Panel"/>s that position children based on <see cref="ItemsDirection"/>
	/// </summary>
	public class BaseFlowDirectionContainerPanel : Panel
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
			typeof(BaseFlowDirectionContainerPanel),
			new PropertyMetadata(EqualChildSpaceContainer.DefaultFlowDirection, FlowDirectionChanged));

		#endregion
		
		#region Protected methods

		/// <summary>
		/// Arranges all invisible children (with new rectangle located at 0,0 with width and height equal to 0)
		/// </summary>
		protected void ArrangeInvisibleChildren()
		{
			foreach(var child in Children.Where((child) => child.Visibility == Visibility.Collapsed))
			{
				child.Arrange(new Rect(0, 0, 0, 0));
			}
		}

		#endregion

		#region Private static methods

		/// <summary>
		/// Callback for <see cref="FlowDirectionProperty"/> changed, updates the UI if the new value differs from the old one
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private static void FlowDirectionChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			if (s is BaseFlowDirectionContainerPanel panel && e.NewValue is ItemsDirection newDirection &&
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