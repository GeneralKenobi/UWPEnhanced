using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Control presenting X-Y data on a graph
	/// </summary>
	[TemplatePart(Name ="DataPresenter", Type = typeof(ItemsControl))]
	public sealed class Graph : Control, INotifyPropertyChanged
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public Graph()
		{
			this.DefaultStyleKey = typeof(Graph);
			this.SizeChanged += (s, e) => TransformAndUpdateData();			
		}

		#endregion

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			
			_DataPresenter = GetTemplateChild("DataPresenter") as ItemsControl ?? 
				throw new Exception("Can't find template child: \"DataPresenter\" of type " + nameof(ItemsControl));

		}


		/// <summary>
		/// Collection of points obtained by transforming <see cref="Data"/> so that they may be used to visualize the data with
		/// TranslateTransform. Their position relative to each other is not changed. 0,0 is considered to be the bottom left corner.
		/// </summary>
		public IEnumerable<KeyValuePair<double, double>> DataDisplayPoints { get; private set; }

		private ItemsControl _DataPresenter { get; set; }

		private void TransformAndUpdateData()
		{
			UpdateLayout();
			var width = ActualWidth - 5;
			var first = Data.Min((point) => point.Key);
			var dataXRange = Data.Max((point) => point.Key) - first;
			DataDisplayPoints = Data.
				Select((point) => new KeyValuePair<double, double>(point.Key - first, point.Value)).
				Select((point) => new KeyValuePair<double, double>(point.Key * width / dataXRange, point.Value)).
				Select((point) => new KeyValuePair<double, double>(point.Key, -point.Value));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DataDisplayPoints)));
		}

		private void UpdateData(IEnumerable<KeyValuePair<double, double>> data)
		{
			_DataPresenter.ItemsSource = data;
		}

		#region Events

		/// <summary>
		/// Event fired whenever a property changes its value
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Data Dependency Property

		

		/// <summary>
		/// Data to present
		/// </summary>
		public IEnumerable<KeyValuePair<double, double>> Data
		{
			get => (IEnumerable<KeyValuePair<double, double>>)GetValue(DataProperty);
			set => SetValue(DataProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Data"/>
		/// </summary>
		public static readonly DependencyProperty DataProperty =
			DependencyProperty.Register(nameof(Data), typeof(IEnumerable<KeyValuePair<double, double>>),
			typeof(Graph),
			new PropertyMetadata(default(IEnumerable<KeyValuePair<double, double>>), new PropertyChangedCallback(DataChanged)));

		#endregion

		#region Private static methods


		private static void DataChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if(sender is Graph g && e.NewValue is IEnumerable<KeyValuePair<double, double>> nv)
			{
				g.TransformAndUpdateData();
			}
		}

		#endregion
	}
}