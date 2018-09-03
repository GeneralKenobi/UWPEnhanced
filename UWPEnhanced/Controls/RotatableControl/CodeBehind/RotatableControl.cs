using CSharpEnhanced.Maths;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Control that may rotate its child and which will update its Width/Height based on the position produced due to rotation
	/// (the rotation has to be performed using <see cref="RotationAngle"/>). ChildPresenter (named <see cref="ChildPresenterName"/>)
	/// will have a render transform assigned to it by this class, because of that it should not be given any other render transforms.
	/// <see cref="FrameworkElement"/> named <see cref="ChildPresenterName"/> is used to present and measure the child.
	/// <see cref="Canvas"/> named <see cref="CanvasContainerName"/> is used to store the <see cref="FrameworkElement"/> named
	/// <see cref="ChildPresenterName"/> (so that it's not clipped by going out of bounds, for example like in a border) and to
	/// extort sizing of the control that is exactly wrapping the rotated child in a rectangle shape.
	/// </summary>
	[ContentProperty(Name = nameof(Child))]	
	[TemplatePart(Name = ChildPresenterName, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = CanvasContainerName, Type = typeof(Canvas))]
	public sealed class RotatableControl : Control
	{
		#region Constructors
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public RotatableControl()
		{
			this.DefaultStyleKey = typeof(RotatableControl);
		}

		#endregion

		#region Private members

		/// <summary>
		/// Backing store for <see cref="_ChildPresenter"/>
		/// </summary>
		private FrameworkElement mChildPresenter;

		/// <summary>
		/// Stores the canvas that is responsible for size of this control and which stores the child
		/// </summary>
		private Canvas mCanvasContainer;

		#endregion

		#region Public constant members

		/// <summary>
		/// Name of the control storing the child
		/// </summary>
		public const string ChildPresenterName = "PART_ChildPresenter";

		/// <summary>
		/// Name of the canvas that is responsible for size of this control and which stores the child
		/// </summary>
		public const string CanvasContainerName = "PART_Canvas";

		#endregion

		#region Private properties

		/// <summary>
		/// Stores the presenter containing <see cref="Child"/>
		/// </summary>
		private FrameworkElement _ChildPresenter
		{
			get => mChildPresenter;
			set
			{
				if(mChildPresenter == value)
				{
					return;
				}

				// If there was an old container, unsubscribe from its size changed event and remove its render transform
				if (mChildPresenter != null)
				{
					mChildPresenter.SizeChanged -= ChildPresenterSizeChangedCallback;
					mChildPresenter.RenderTransform = null;
				}

				mChildPresenter = value;

				// If the new container is not null, subscribe to its size changed event and assign render transform
				if (mChildPresenter != null)
				{
					mChildPresenter.SizeChanged += ChildPresenterSizeChangedCallback;
					mChildPresenter.RenderTransform = _ChildPresenterRenderTransform;
				}
			}
		}

		/// <summary>
		/// True if the control is operational (necessary template parts were found (respective fields are not null)
		/// </summary>
		private bool _Operational => _ChildPresenter != null && mCanvasContainer != null;

		/// <summary>
		/// Render transform applied to child presenter to position it inside the bounds of this control.
		/// </summary>
		private CompositeTransform _ChildPresenterRenderTransform { get; } = new CompositeTransform();

		#endregion

		#region Child Dependency Property

		/// <summary>
		/// Child to rotate using this control
		/// </summary>
		public UIElement Child
		{
			get => (UIElement)GetValue(ChildProperty);
			set => SetValue(ChildProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Child"/>
		/// </summary>
		public static readonly DependencyProperty ChildProperty =
			DependencyProperty.Register(nameof(Child), typeof(UIElement),
			typeof(RotatableControl), new PropertyMetadata(default(UIElement),
				new PropertyChangedCallback(DependencyPropertyDeterminingSizeChanged)));

		#endregion

		#region RotationAngle Dependency Property

		/// <summary>
		/// Rotation angle, in degrees, anticlockwise direction of rotation
		/// </summary>
		public double RotationAngle
		{
			get => (double)GetValue(RotationAngleProperty);
			set => SetValue(RotationAngleProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="RotationAngle"/>
		/// </summary>
		public static readonly DependencyProperty RotationAngleProperty =
			DependencyProperty.Register(nameof(RotationAngle), typeof(double),
			typeof(RotatableControl), new PropertyMetadata(default(double),
				new PropertyChangedCallback(DependencyPropertyDeterminingSizeChanged)));

		#endregion

		#region Private methods

		/// <summary>
		/// Assigns appropriate values to <see cref="_ChildPresenterRenderTransform"/> based on newly calculated size and
		/// <see cref="RotationAngle"/>
		/// </summary>
		/// <param name="widthFromWidth">Rotated width as a function of normal width</param>
		/// <param name="widthFromHeight">Rotated width as a function of normal height</param>
		/// <param name="heightFromWidth">Rotated height as a function of normal width</param>
		/// <param name="heightFromHeight">Rotated height as a function of normal height</param>
		private void AssignRenderTransformTranslates(double widthFromWidth, double widthFromHeight, double heightFromWidth,
			double heightFromHeight)
		{
			// Reduce the angle to work in the 0-360 range
			var angle = MathsHelpers.ReduceAngle(RotationAngle, AngleUnit.Degrees);

			// Reset translates
			_ChildPresenterRenderTransform.TranslateX = 0;
			_ChildPresenterRenderTransform.TranslateY = 0;

			// Assign the angle
			_ChildPresenterRenderTransform.Rotation = -angle;

			// For first and second quadrant
			if (angle <= 180)
			{
				// Add height calculated from normal width to y translate
				_ChildPresenterRenderTransform.TranslateY += heightFromWidth;
			}

			// For second and third quadrant
			if (angle > 90 && angle <= 270)
			{
				// Add height calculated from height to y translate
				_ChildPresenterRenderTransform.TranslateY += heightFromHeight;
				// And add width calculated from width to x translate
				_ChildPresenterRenderTransform.TranslateX += widthFromWidth;
			}

			// For third and fourth quadrant
			if(angle > 180)
			{
				// Add width calculated from height to x translate
				_ChildPresenterRenderTransform.TranslateX += widthFromHeight;
			}
		}

		/// <summary>
		/// Updates <see cref="mCanvasContainer"/> size, <see cref="_ChildPresenter"/> render transform.
		/// </summary>
		private void UpdateSize()
		{
			if (_Operational)
			{
				// Reduce the angle to work in the 0-360 degree range
				var angle = MathsHelpers.ReduceAngle(RotationAngle, AngleUnit.Degrees);

				// Update the layout
				_ChildPresenter.UpdateLayout();

				// All values are calculated based on model developed using pen and paper and trigonometric functions

				// Rotated width as a function of normal width
				double widthFromWidth = Math.Abs(_ChildPresenter.ActualWidth *
					Math.Cos(MathsHelpers.ConvertAngle(RotationAngle, AngleUnit.Degrees, AngleUnit.Radians)));

				// Rotated width as a function of normal height
				double widthFromHeight = Math.Abs(_ChildPresenter.ActualHeight *
					Math.Sin(MathsHelpers.ConvertAngle(RotationAngle, AngleUnit.Degrees, AngleUnit.Radians)));

				// Rotated height as a function of normal width
				double heightFromWidth = Math.Abs(_ChildPresenter.ActualWidth *
					Math.Sin(MathsHelpers.ConvertAngle(RotationAngle, AngleUnit.Degrees, AngleUnit.Radians)));

				// Rotated height as a function of normal height
				double heightFromHeight = Math.Abs(_ChildPresenter.ActualHeight *
					Math.Cos(MathsHelpers.ConvertAngle(RotationAngle, AngleUnit.Degrees, AngleUnit.Radians)));

				// Assign new widths/heights 
				mCanvasContainer.Width = widthFromWidth + widthFromHeight;
				mCanvasContainer.Height = heightFromWidth + heightFromHeight;

				// Finally assign render transform values
				AssignRenderTransformTranslates(widthFromWidth, widthFromHeight, heightFromWidth, heightFromHeight);
			}
		}

		/// <summary>
		/// Callback for SizeChanged event on <see cref="_ChildPresenter"/>, if the new value differs from the old one calls
		/// <see cref="UpdateSize"/> method.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ChildPresenterSizeChangedCallback(object sender, SizeChangedEventArgs e)
		{
			if (e.NewSize != e.PreviousSize)
			{
				UpdateSize();
			}
		}

		#endregion

		#region Protected methods

		/// <summary>
		/// Attempts to find controls matching <see cref="ChildPresenterName"/> and <see cref="CanvasContainerName"/>, subscribes to
		/// the child presenter's size changed event.
		/// </summary>
		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_ChildPresenter = GetTemplateChild(ChildPresenterName) as FrameworkElement;

			mCanvasContainer = GetTemplateChild(CanvasContainerName) as Canvas;

			UpdateSize();
		}

		#endregion

		#region Private static methods

		/// <summary>
		/// Callback for dependency properties on which the size/rotation depend. If the new value differs from the old one, calls
		/// <see cref="UpdateSize"/> method.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void DependencyPropertyDeterminingSizeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if(sender is RotatableControl c && e.NewValue != e.OldValue)
			{
				c.UpdateSize();
			}
		}

		#endregion
	}
}