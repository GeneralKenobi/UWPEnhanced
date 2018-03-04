using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	public class VisualStateNavigator
	{

		/// <summary>
		/// Default Constructor
		/// </summary>
		public VisualStateNavigator()
		{

		}


		#region Navigations

		/// <summary>
		/// Getter for <see cref="Navigations"/>
		/// </summary>
		public static ObservableCollection<VisualStateNavigation> GetNavigations(DependencyObject obj)
		{
			var c = (ObservableCollection<VisualStateNavigation>)obj.GetValue(NavigationsProperty);

			if(c==null)
			{
				obj.SetValue(NavigationsProperty, new ObservableCollection<VisualStateNavigation>());
				return (ObservableCollection<VisualStateNavigation>)obj.GetValue(NavigationsProperty);
			}

			foreach(var item in c)
			{
				item.NavigateFor = obj as UIElement;
			}

			return c;
		}

		/// <summary>
		/// Setter for <see cref="Navigations"/>
		/// </summary>
		public static void SetNavigations(DependencyObject obj, ObservableCollection<VisualStateNavigation> value) => obj.SetValue(NavigationsProperty, value);

		/// <summary>
		/// Collection of all navigations defined for the given object
		/// </summary>
		public static readonly DependencyProperty NavigationsProperty =
			DependencyProperty.RegisterAttached("Navigations", typeof(ObservableCollection<VisualStateNavigation>),
			 typeof(VisualStateNavigator), new PropertyMetadata(null, new PropertyChangedCallback(t)));

		private static ObservableCollection<VisualStateNavigation> gen()
		{
			ObservableCollection<VisualStateNavigation> x = new ObservableCollection<VisualStateNavigation>();
			//x.CollectionChanged += t;
			return x;
		}

		private static void t(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if(e.NewValue is ObservableCollection<VisualStateNavigation> c)
			{
				foreach(var item in c)
				{
					item.NavigateFor = sender as UIElement;
				}
			}
		}


		private void ObjLoaded(object sender, RoutedEventArgs e)
		{
			if(sender is DependencyObject d)
			{
				foreach(var item in GetNavigations(d))
				{
					item.NavigateFor = d as UIElement;
				}
			}
		}


		#endregion
	}
}
