using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Control presenting X-Y data on a graph
	/// </summary>
	public sealed class Graph : Control, INotifyPropertyChanged
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public Graph()
		{
			this.DefaultStyleKey = typeof(Graph);
			this.SizeChanged += SizeChangedCallback;
		}

		#endregion

		#region Events

		/// <summary>
		/// Event fired whenever a property changes its value
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Public properties

		/// <summary>
		/// Collection of points obtained by transforming <see cref="Data"/> so that they may be used to visualize the data with
		/// TranslateTransform. Their position relative to each other is not changed. 0,0 is considered to be the bottom left corner.
		/// </summary>
		public IEnumerable<KeyValuePair<double, double>> DataDisplayPoints { get; private set; }

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
			new PropertyMetadata(default(IEnumerable<KeyValuePair<double, double>>), new PropertyChangedCallback(DataChangedCallback)));

		#endregion

		#region PointDiameter Dependency Property

		/// <summary>
		/// Diameter of a displayed point
		/// </summary>
		public double PointDiameter
		{
			get => (double)GetValue(PointDiameterProperty);
			set => SetValue(PointDiameterProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="PointDiameter"/>
		/// </summary>
		public static readonly DependencyProperty PointDiameterProperty =
			DependencyProperty.Register(nameof(PointDiameter), typeof(double),
			typeof(Graph), new PropertyMetadata(default(double)));

		#endregion

		#region Private methods

		/// <summary>
		/// Callback for when size changes (recalculates <see cref="DataDisplayPoints"/>)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SizeChangedCallback(object sender, SizeChangedEventArgs e) => TransformAndUpdateData();

		/// <summary>
		/// Calculates new positions for display points (based on <see cref="Data"/>) and assigns them to <see cref="DataDisplayPoints"/>
		/// </summary>
		private void TransformAndUpdateData()
		{
			UpdateLayout();
			var width = ActualWidth - PointDiameter;
			var first = Data.Min((point) => point.Key);
			var dataXRange = Data.Max((point) => point.Key) - first;
			DataDisplayPoints = Data.
				Select((point) => new KeyValuePair<double, double>(point.Key - first, point.Value)).
				Select((point) => new KeyValuePair<double, double>(point.Key * width / dataXRange, point.Value)).
				Select((point) => new KeyValuePair<double, double>(point.Key, -point.Value));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DataDisplayPoints)));
		}

		#endregion

		#region Private static methods

		/// <summary>
		/// Callback for when <see cref="Data"/> changes, if new value is the same type as <see cref="Data"/>, calls
		/// <see cref="TransformAndUpdateData"/>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void DataChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if(sender is Graph g && e.NewValue is IEnumerable<KeyValuePair<double, double>> nv)
			{
				g.TransformAndUpdateData();
			}
		}

		#endregion
	}
}