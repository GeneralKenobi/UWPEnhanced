using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Control presenting X-Y data on a graph
	/// </summary>
	[TemplatePart(Name = mGraphAreaName, Type = typeof(FrameworkElement))]
	public sealed class Graph : Control, INotifyPropertyChanged
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public Graph()
		{
			this.DefaultStyleKey = typeof(Graph);
		}

		#endregion
		
		#region Events

		/// <summary>
		/// Event fired whenever a property changes its value
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Private fields

		/// <summary>
		/// Name of the control containing the graph visuals (without any margin/padding)
		/// </summary>
		public const string mGraphAreaName = "PART_GraphArea";

		#endregion

		#region Private properties

		/// <summary>
		/// Control wrapping the area on which points are presented. Its ActualWidth and ActualHeight should correspond exactly to the
		/// on-screen area in which graph points are to be rendered
		/// </summary>
		private FrameworkElement _GraphArea { get; set; }

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

		#region GraphAreaBackground Dependency Property

		/// <summary>
		/// Brush used directly behind the graph
		/// </summary>
		public Brush GraphAreaBackground
		{
			get => (Brush)GetValue(GraphAreaBackgroundProperty);
			set => SetValue(GraphAreaBackgroundProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="GraphAreaBackground"/>
		/// </summary>
		public static readonly DependencyProperty GraphAreaBackgroundProperty =
			DependencyProperty.Register(nameof(GraphAreaBackground), typeof(Brush),
			typeof(Graph), new PropertyMetadata(default(Brush)));

		#endregion

		#region GraphForeground Dependency Property

		/// <summary>
		/// Brush used to paint the points/lines composing the graph
		/// </summary>
		public Brush GraphForeground
		{
			get => (Brush)GetValue(GraphForegroundProperty);
			set => SetValue(GraphForegroundProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="GraphForeground"/>
		/// </summary>
		public static readonly DependencyProperty GraphForegroundProperty =
			DependencyProperty.Register(nameof(GraphForeground), typeof(Brush),
			typeof(Graph), new PropertyMetadata(default(Brush)));

		#endregion

		#region Private methods

		/// <summary>
		/// Sets new value to <see cref="_GraphArea"/>. Additonally calls <see cref="TransformAndUpdateData"/>
		/// </summary>
		private void SetGraphArea(FrameworkElement newGraphArea)
		{
			// If value is the same, simply return
			if (_GraphArea == newGraphArea)
			{
				return;
			}

			// If the old value was not null, unsubscribe from its SizeChanged
			if (_GraphArea != null)
			{
				_GraphArea.SizeChanged -= SizeChangedCallback;
			}

			_GraphArea = newGraphArea;

			// If the new value is not null, subscribe to its SizeChanged
			if (_GraphArea != null)
			{
				_GraphArea.SizeChanged += SizeChangedCallback;
			}

			// Transform and update the data
			TransformAndUpdateData();
		}

		/// <summary>
		/// Transfrorms a value to into an absolute position inside this <see cref="Graph"/>
		/// </summary>
		/// <param name="value">Value to transform</param>
		/// <param name="smallest">The smallest value in the dimension</param>
		/// <param name="range">Difference between the greatest and the smallest value in the dimension</param>
		/// <param name="graphAreaLength">Length of the dimension assigned to the <see cref="_GraphArea"/> control</param>
		/// <returns></returns>
		private double ValueTransform(double value, double smallest, double range, double graphAreaLength) =>
			// First subtract the smallest value (so that the data starts from the left), then divide by range (which causes
			// the greatest value in the set to be 1) and multiply by graphAreaLength (which causes the greatest value in the set to
			// be equal to the total length of the control - thus spanning the data fully from left to right).
			(value - smallest) * graphAreaLength / range;

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
			// Check if graph area was found
			if(_GraphArea == null)
			{
				return;
			}

			// Update the layout to make sure that width and height have up-to-date values
			UpdateLayout();

			// Get the dimensions of the graph area (subtract the PointDiameter so that all points are inside the area - their
			// coordinates are the left, bottom corner of the point)
			var graphAreaWidth = _GraphArea.ActualWidth - PointDiameter;
			var graphAreaHeight = _GraphArea.ActualHeight - PointDiameter;
			
			// Get the minimum values on both axes
			var minX = Data.Min((point) => point.Key);
			var minY = Data.Min((point) => point.Value);

			// Get the range of values on botx axes (maximum value - minimum value)
			var xRange = Data.Max((point) => point.Key) - minX;
			var yRange = Data.Max((point) => point.Value) - minY;

			// Assign to DataDisplayPoints the 
			DataDisplayPoints = Data.
				Select((point) => new KeyValuePair<double, double>(ValueTransform(point.Key, minX, xRange, graphAreaWidth),
				ValueTransform(point.Value, minY, yRange, graphAreaHeight)));

			// Notify that data changed
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DataDisplayPoints)));
		}

		#endregion

		#region Protected methods

		/// <summary>
		/// Additionally finds the control with name <see cref="mGraphAreaName"/>
		/// </summary>
		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			SetGraphArea((GetTemplateChild(mGraphAreaName) as FrameworkElement) ?? throw new Exception("Graph area control not found"));
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