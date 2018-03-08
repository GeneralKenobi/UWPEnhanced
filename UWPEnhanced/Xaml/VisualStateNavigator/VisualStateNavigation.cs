using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPEnhanced.Xaml
{
	public class VisualStateNavigationaa : DependencyObject
	{
		private async Task t()
		{
			await Task.Delay(3000);
		}

		/// <summary>
		/// Default Constructor
		/// </summary>
		public VisualStateNavigationaa()
		{
			VisualStateNavigationTriggers = new ObservableCollection<VisualStateNavigationTrigger>();
			VisualStateNavigationTriggers.CollectionChanged += VisualStateNavigationTriggersCollectionChanged;
			t();
		}

		private void VisualStateNavigationTriggersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			foreach(var item in e.NewItems)
			{
				if(item is VisualStateNavigationTrigger t)
				{
					//t.Navigate += (s) => VisualStateManager.GoToState(NavigateFor, TransferToState, true);
				}
			}
		}

		#region TransferToState Dependency Property

		/// <summary>
		/// State to transfer to
		/// </summary>
		public string TransferToState
		{
			get => (string)GetValue(TransferToStateProperty);
			set => SetValue(TransferToStateProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="TransferToState"/>
		/// </summary>
		public static readonly DependencyProperty TransferToStateProperty =
			DependencyProperty.Register(nameof(TransferToState), typeof(string),
			typeof(VisualStateNavigation), new PropertyMetadata(default(string)));

		#endregion

		#region VisualStateNavigationTriggers Dependency Property

		/// <summary>
		/// Triggers for this VisualStateNavigation
		/// </summary>
		public ObservableCollection<VisualStateNavigationTrigger> VisualStateNavigationTriggers
		{
			get => (ObservableCollection<VisualStateNavigationTrigger>)GetValue(VisualStateNavigationTriggersProperty);
			set => SetValue(VisualStateNavigationTriggersProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="VisualStateNavigationTriggers"/>
		/// </summary>
		public static readonly DependencyProperty VisualStateNavigationTriggersProperty =
			DependencyProperty.Register(nameof(VisualStateNavigationTriggers), typeof(ObservableCollection<VisualStateNavigationTrigger>),
			typeof(VisualStateNavigation), new PropertyMetadata(null));

		#endregion

		#region NavigateFor Dependency Property

		/// <summary>
		/// Navigate the given control
		/// </summary>
		public UIElement NavigateFor
		{
			get => (UIElement)GetValue(NavigateForProperty);
			set => SetValue(NavigateForProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="NavigateFor"/>
		/// </summary>
		public static readonly DependencyProperty NavigateForProperty =
			DependencyProperty.Register(nameof(NavigateFor), typeof(UIElement),
			typeof(VisualStateNavigation), new PropertyMetadata(default(UIElement)));

		#endregion
	}
}
