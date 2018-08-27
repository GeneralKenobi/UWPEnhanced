﻿using System.Collections.Generic;
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
		/// Transfrorms a value to into an absolute position inside this <see cref="Graph"/>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="smallest"></param>
		/// <param name="range"></param>
		/// <param name="graphAreaLength"></param>
		/// <returns></returns>
		private double ValueTransform(double value, double smallest, double range, double graphAreaLength) =>
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
			// Update the layout to make sure that width and height have up-to-date values
			UpdateLayout();

			// Get the dimensions of the graph area (subtract the PointDiameter so that all points are inside the area - their
			// coordinates are the left, bottom corner of the point)
			var graphAreaWidth = ActualWidth - PointDiameter;
			var graphAreaHeight = ActualHeight - PointDiameter;
			
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