using CSharpEnhanced.Maths;
using CSharpEnhanced.Helpers;
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
		/// Backing store for <see cref="DataDisplayPoints"/>
		/// </summary>
		private IEnumerable<KeyValuePair<double, double>> mDataDisplayPoints;

		/// <summary>
		/// Backing store for <see cref="HorizontalAxisLabels"/>
		/// </summary>
		private IEnumerable<string> mHorizontalAxisLabels;

		/// <summary>
		/// Backing store for <see cref="VerticalAxisLabels"/>
		/// </summary>
		private IEnumerable<string> mVerticalAxisLabels;

		#endregion

		#region Public fields

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
		public IEnumerable<KeyValuePair<double, double>> DataDisplayPoints
		{
			get => mDataDisplayPoints;
			set
			{
				if(mDataDisplayPoints != value)
				{
					mDataDisplayPoints = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DataDisplayPoints)));
				}
			}
		}

		/// <summary>
		/// Collection of points obtained by transforming <see cref="Data"/> so that they may be used to visualize the data with
		/// TranslateTransform. Their position relative to each other is not changed. 0,0 is considered to be the bottom left corner.
		/// </summary>
		public IEnumerable<string> HorizontalAxisLabels
		{
			get => mHorizontalAxisLabels;
			set
			{
				if (mHorizontalAxisLabels != value)
				{
					mHorizontalAxisLabels = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HorizontalAxisLabels)));
				}
			}
		}

		/// <summary>
		/// Collection of points obtained by transforming <see cref="Data"/> so that they may be used to visualize the data with
		/// TranslateTransform. Their position relative to each other is not changed. 0,0 is considered to be the bottom left corner.
		/// </summary>
		public IEnumerable<string> VerticalAxisLabels
		{
			get => mVerticalAxisLabels;
			set
			{
				if (mVerticalAxisLabels != value)
				{
					mVerticalAxisLabels = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VerticalAxisLabels)));
				}
			}
		}

		#endregion

		#region Dependency properties

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

		#region DataBackground Dependency Property

		/// <summary>
		/// Brush used directly behind the graph
		/// </summary>
		public Brush DataBackground
		{
			get => (Brush)GetValue(DataBackgroundProperty);
			set => SetValue(DataBackgroundProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="DataBackground"/>
		/// </summary>
		public static readonly DependencyProperty DataBackgroundProperty =
			DependencyProperty.Register(nameof(DataBackground), typeof(Brush),
			typeof(Graph), new PropertyMetadata(default(Brush)));

		#endregion

		#region DataForeground Dependency Property

		/// <summary>
		/// Brush used to paint the points/lines composing the graph
		/// </summary>
		public Brush DataForeground
		{
			get => (Brush)GetValue(DataForegroundProperty);
			set => SetValue(DataForegroundProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="DataForeground"/>
		/// </summary>
		public static readonly DependencyProperty DataForegroundProperty =
			DependencyProperty.Register(nameof(DataForeground), typeof(Brush),
			typeof(Graph), new PropertyMetadata(default(Brush)));

		#endregion

		#region HorizontalAxisLabelsCount Dependency Property

		/// <summary>
		/// Number of labels present on X axis
		/// </summary>
		public int HorizontalAxisLabelsCount
		{
			get => (int)GetValue(HorizontalAxisLabelsCountProperty);
			set => SetValue(HorizontalAxisLabelsCountProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="VerticalAxisLabelsCount"/>
		/// </summary>
		public static readonly DependencyProperty HorizontalAxisLabelsCountProperty =
			DependencyProperty.Register(nameof(HorizontalAxisLabelsCount), typeof(int),
			typeof(Graph), new PropertyMetadata(default(int), new PropertyChangedCallback(LabelConfigurationChangedCallback)));

		#endregion

		#region VerticalAxisLabelsCount Dependency Property

		/// <summary>
		/// Number of labels present on Y axis		
		/// </summary>
		public int VerticalAxisLabelsCount
		{
			get => (int)GetValue(VerticalAxisLabelsCountProperty);
			set => SetValue(VerticalAxisLabelsCountProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="VerticalAxisLabelsCount"/>
		/// </summary>
		public static readonly DependencyProperty VerticalAxisLabelsCountProperty =
			DependencyProperty.Register(nameof(VerticalAxisLabelsCount), typeof(int),
			typeof(Graph), new PropertyMetadata(default(int), new PropertyChangedCallback(LabelConfigurationChangedCallback)));

		#endregion

		#region RoundLabelToDigit Dependency Property

		/// <summary>
		/// Digit to round the values in labels to
		/// </summary>
		public int RoundLabelToDigit
		{
			get => (int)GetValue(RoundLabelToDigitProperty);
			set => SetValue(RoundLabelToDigitProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="RoundLabelToDigit"/>
		/// </summary>
		public static readonly DependencyProperty RoundLabelToDigitProperty =
			DependencyProperty.Register(nameof(RoundLabelToDigit), typeof(int),
			typeof(Graph), new PropertyMetadata(default(int), new PropertyChangedCallback(LabelConfigurationChangedCallback)));

		#endregion

		#region HorizontalAxisDescription Dependency Property

		/// <summary>
		/// Description displayed below the horizontal axis (name of the quantity, unit, etc.)
		/// </summary>
		public string HorizontalAxisDescription
		{
			get => (string)GetValue(HorizontalAxisDescriptionProperty);
			set => SetValue(HorizontalAxisDescriptionProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="HorizontalAxisDescription"/>
		/// </summary>
		public static readonly DependencyProperty HorizontalAxisDescriptionProperty =
			DependencyProperty.Register(nameof(HorizontalAxisDescription), typeof(string),
			typeof(Graph), new PropertyMetadata(default(string)));

		#endregion

		#region VerticalAxisDescription Dependency Property

		/// <summary>
		/// Description displayed on the left of the vertical axis (name of the quantity, unit, etc.)
		/// </summary>
		public string VerticalAxisDescription
		{
			get => (string)GetValue(VerticalAxisDescriptionProperty);
			set => SetValue(VerticalAxisDescriptionProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="VerticalAxisDescription"/>
		/// </summary>
		public static readonly DependencyProperty VerticalAxisDescriptionProperty =
			DependencyProperty.Register(nameof(VerticalAxisDescription), typeof(string),
			typeof(Graph), new PropertyMetadata(default(string)));

		#endregion

		#region Title Dependency Property

		/// <summary>
		/// Text to display above the graph as its title
		/// </summary>
		public string Title
		{
			get => (string)GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Title"/>
		/// </summary>
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register(nameof(Title), typeof(string),
			typeof(Graph), new PropertyMetadata(default(string)));

		#endregion

		#region TitleFontSize Dependency Property

		/// <summary>
		/// Font size of the title
		/// </summary>
		public double TitleFontSize
		{
			get => (double)GetValue(TitleFontSizeProperty);
			set => SetValue(TitleFontSizeProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="TitleFontSize"/>
		/// </summary>
		public static readonly DependencyProperty TitleFontSizeProperty =
			DependencyProperty.Register(nameof(TitleFontSize), typeof(double),
			typeof(Graph), new PropertyMetadata(default(double)));

		#endregion

		#region AutoLabelHorizontalFrequency Dependency Property

		/// <summary>
		/// The frequency of horizontal labels when they are generated automatically. One label is placed every
		/// <see cref="AutoLabelHorizontalFrequency"/> units of length. If this value is not positive then no lables are generated.
		/// </summary>
		public double AutoLabelHorizontalFrequency
		{
			get => (double)GetValue(AutoLabelHorizontalFrequencyProperty);
			set => SetValue(AutoLabelHorizontalFrequencyProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="AutoLabelHorizontalFrequency"/>
		/// </summary>
		public static readonly DependencyProperty AutoLabelHorizontalFrequencyProperty =
			DependencyProperty.Register(nameof(AutoLabelHorizontalFrequency), typeof(double),
			typeof(Graph), new PropertyMetadata(default(double), new PropertyChangedCallback(LabelConfigurationChangedCallback)));

		#endregion

		#region AutoLabelVerticalFrequency Dependency Property

		/// <summary>
		/// The frequency of vertical labels when they are generated automatically. One label is placed every
		/// <see cref="AutoLabelHorizontalFrequency"/> units of length. If this value is not positive then no lables are generated.
		/// </summary>
		public double AutoLabelVerticalFrequency
		{
			get => (double)GetValue(AutoLabelVerticalFrequencyProperty);
			set => SetValue(AutoLabelVerticalFrequencyProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="AutoLabelVerticalFrequency"/>
		/// </summary>
		public static readonly DependencyProperty AutoLabelVerticalFrequencyProperty =
			DependencyProperty.Register(nameof(AutoLabelVerticalFrequency), typeof(double),
			typeof(Graph), new PropertyMetadata(default(double), new PropertyChangedCallback(LabelConfigurationChangedCallback)));

		#endregion

		#region SIPrefixConfig Dependency Property

		/// <summary>
		/// If true, SI prefixes are used when displaying labels
		/// </summary>
		public SIPrefixConfiguration SIPrefixConfig
		{
			get => (SIPrefixConfiguration)GetValue(SIPrefixConfigProperty);
			set => SetValue(SIPrefixConfigProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="SIPrefixConfig"/>
		/// </summary>
		public static readonly DependencyProperty SIPrefixConfigProperty =
			DependencyProperty.Register(nameof(SIPrefixConfig), typeof(SIPrefixConfiguration),
			typeof(Graph), new PropertyMetadata(default(SIPrefixConfiguration),
				new PropertyChangedCallback(LabelConfigurationChangedCallback)));

		#endregion

		#endregion

		#region Private methods

		/// <summary>
		/// Returns the number of horizontal labels which is <see cref="HorizontalAxisLabelsCount"/> if that value is nonnegative,
		/// otherwise calculates 
		/// </summary>
		/// <returns></returns>
		private int GetHorizontalLabelsCount()
		{
			// If the fixed value is specified (greater than or equal to 0), return it
			if (HorizontalAxisLabelsCount >= 0)
			{
				return HorizontalAxisLabelsCount;
			}
			else
			{
				// If the frequency is smaller than or equal to 0 or the graph area was not resolved properly, return 0
				if(AutoLabelHorizontalFrequency <= 0 || _GraphArea == null)
				{
					return 0;
				}
				else
				{
					// Otherwise calculate the dynamic value which is width divided by frequency, rounded up
					return (int)Math.Ceiling(_GraphArea.ActualWidth / AutoLabelHorizontalFrequency);
				}
			}
		}

		/// <summary>
		/// Returns the number of vertical labels which is <see cref="VerticalAxisLabelsCount"/> if that value is nonnegative,
		/// otherwise calculates 
		/// </summary>
		/// <returns></returns>
		private int GetVerticalLabelsCount()
		{
			// If the fixed value is specified (greater than or equal to 0), return it
			if (VerticalAxisLabelsCount >= 0)
			{
				return VerticalAxisLabelsCount;
			}
			else
			{
				// If the frequency is smaller than or equal to 0 or the graph area was not resolved properly, return 0
				if (AutoLabelVerticalFrequency <= 0 || _GraphArea == null)
				{
					return 0;
				}
				else
				{
					// Otherwise calculate the dynamic value which is width divided by frequency, rounded up
					return (int)Math.Ceiling(_GraphArea.ActualHeight / AutoLabelVerticalFrequency);
				}
			}
		}

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

			// Generate labels
			GenerateLabels();
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
		private void SizeChangedCallback(object sender, SizeChangedEventArgs e)
		{
			TransformAndUpdateData();
			GenerateLabels();
		}

		/// <summary>
		/// Calculates new positions for display points (based on <see cref="Data"/>) and assigns them to <see cref="DataDisplayPoints"/>
		/// </summary>
		private void TransformAndUpdateData()
		{
			// Check if graph area was found
			if (_GraphArea == null)
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
		}

		/// <summary>
		/// Generates horizontal axis labels for the current data
		/// </summary>
		private void GenerateHorizontalAxisLabels() => HorizontalAxisLabels = ConvertToLabels(MathsHelpers.CalculateMidPoints(
			Data.Min((x) => x.Key), Data.Max((x) => x.Key), GetHorizontalLabelsCount()));

		/// <summary>
		/// Generates vertical axis labels for the current data
		/// </summary>
		private void GenerateVerticalAxisLabels() => VerticalAxisLabels = ConvertToLabels(MathsHelpers.CalculateMidPoints(
			Data.Min((x) => x.Value), Data.Max((x) => x.Value), GetVerticalLabelsCount()));

		/// <summary>
		/// Converts a data set to a set of labels constructed with <see cref="SIPrefixConfig"/> taken into account
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		private IEnumerable<string> ConvertToLabels(IEnumerable<double> data)
		{
			// Don't do anything for empty sequences
			if(data.Count() == 0)
			{				
				return Enumerable.Empty<string>();
			}

			switch(SIPrefixConfig)
			{
				case SIPrefixConfiguration.None:
					{
						// Round and convert to string
						return data.Select((x) => x.RoundToDigit(RoundLabelToDigit).ToString());
					}

				case SIPrefixConfiguration.MinForAll:
					{
						// Get the prefix of the smallest magnitude
						var prefix = SIHelpers.GetClosestPrefixExcludingSmall(data.Min((x) => Math.Abs(x)));
						// And use it to create SIStrings of all values
						return data.Select((x) => SIHelpers.ToSIString(x, prefix, roundToDigit:RoundLabelToDigit));
					}

				case SIPrefixConfiguration.MaxForAll:
					{
						// Get the prefix of the greatest magnitude
						var prefix = SIHelpers.GetClosestPrefixExcludingSmall(data.Max((x) => Math.Abs(x)));
						// And use it to create SIStrings of all values
						return data.Select((x) => SIHelpers.ToSIString(x, prefix, roundToDigit: RoundLabelToDigit));
					}

				case SIPrefixConfiguration.Adequate:
					{
						// Create each SIString with its own adequate prefix
						return data.Select((x) => SIHelpers.ToSIString(x, roundToDigit: RoundLabelToDigit));
					}

				default:
					{
						throw new Exception("Unexpected case");
					}
			}
		}
		
		/// <summary>
		/// Generates horizontal and vertical labels using <see cref="GenerateHorizontalAxisLabels"/> and
		/// <see cref="GenerateVerticalAxisLabels"/>
		/// </summary>
		private void GenerateLabels()
		{
			if(Data != null)
			{
				GenerateHorizontalAxisLabels();
				GenerateVerticalAxisLabels();
			}
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
		/// Callback for when a property that determines appearance of labels changes - updates all labels
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void LabelConfigurationChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is Graph g && e.NewValue != e.OldValue)
			{
				g.GenerateLabels();
			}
		}

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
				g.GenerateLabels();
			}
		}

		#endregion
	}
}