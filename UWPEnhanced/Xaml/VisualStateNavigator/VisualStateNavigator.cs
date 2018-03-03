using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	public static class VisualStateNavigator
	{

		/// <summary>
		/// Default Constructor
		/// </summary>
		static VisualStateNavigator()
		{

		}


		#region Navigations

		/// <summary>
		/// Getter for <see cref="Navigations"/>
		/// </summary>
		public static ObservableCollection<VisualStateNavigation> GetNavigations(DependencyObject obj) => (ObservableCollection<VisualStateNavigation>)obj.GetValue(NavigationsProperty);

		/// <summary>
		/// Setter for <see cref="Navigations"/>
		/// </summary>
		public static void SetNavigations(DependencyObject obj, ObservableCollection<VisualStateNavigation> value) => obj.SetValue(NavigationsProperty, value);

		/// <summary>
		/// Collection of all navigations defined for the given object
		/// </summary>
		public static readonly DependencyProperty NavigationsProperty =
			DependencyProperty.RegisterAttached("Navigations", typeof(ObservableCollection<VisualStateNavigation>),
			 typeof(VisualStateNavigator), new PropertyMetadata(gen()));

		private static ObservableCollection<VisualStateNavigation> gen()
		{
			ObservableCollection<VisualStateNavigation> x = new ObservableCollection<VisualStateNavigation>();
			x.CollectionChanged += t;
			return x;
		}

		private static void t(object a, object b)
		{

		}

		#endregion
	}
}
