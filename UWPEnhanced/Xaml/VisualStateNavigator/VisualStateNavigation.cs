using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	public class VisualStateNavigation : DependencyObject
	{
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
			typeof(VisualStateNavigation), new PropertyMetadata(new ObservableCollection<VisualStateNavigationTrigger>()));

		#endregion

	}
}
