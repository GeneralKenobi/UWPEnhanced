using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWPEnhanced.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace UWPEnhanced.Controls
{
    public sealed class UniformItemsContainer : ItemsControl
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public UniformItemsContainer()
        {
            this.DefaultStyleKey = typeof(UniformItemsContainer);
			this.Loaded += ItemsContainerLoaded;
        }

		/// <summary>
		/// When the control is loaded assigns the first <see cref="ItemSpacing"/> value to the <see cref="UnderlyingPanel"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ItemsContainerLoaded(object sender, RoutedEventArgs e)
		{
			UpdateDirectionOnUnderylingPanel();
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
		public new static readonly DependencyProperty FlowDirectionProperty =
			DependencyProperty.Register(nameof(FlowDirection), typeof(ItemsDirection),
			typeof(UniformItemsContainer), new PropertyMetadata(DefaultFlowDirection, DirectionChanged));

		#endregion

		#region Private static methods

		/// <summary>
		/// Callback for when <see cref="ItemSpacingProperty"/> changes
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private static void DirectionChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			if (s is UniformItemsContainer container && e.NewValue != e.OldValue)
			{
				container.UpdateDirectionOnUnderylingPanel();
			}
		}

		#endregion

		#region Private Methods

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
	}
}