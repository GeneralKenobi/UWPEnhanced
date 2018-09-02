using CSharpEnhanced.Maths;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace UWPEnhanced.Controls
{
	/// <summary>
	/// Control that may rotate its child and which will update its Width/Height based on the position produced due to rotation
	/// (the rotation has to be performed using <see cref="RotationAngle"/>)
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

				// If there was an old container, unsubscribe from its size changed event
				if (mChildPresenter != null)
				{
					mChildPresenter.SizeChanged -= ChildPresenterSizeChangedCallback;
				}

				mChildPresenter = value;

				// If the new container is not null, subscribe to its size changed event
				if (mChildPresenter != null)
				{
					mChildPresenter.SizeChanged += ChildPresenterSizeChangedCallback;
				}
			}
		}

		/// <summary>
		/// True if the control is operational (necessary template parts were found (respective fields are not null)
		/// </summary>
		private bool _Operational => _ChildPresenter != null && mCanvasContainer != null;

		#endregion

		#region Public properties

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
		/// Updates <see cref="mCanvasContainer"/> size, <see cref="_ChildPresenter"/> render transform.
		/// </summary>
		private void UpdateSize()
		{
			if (_Operational)
			{
				// Update the layout
				_ChildPresenter.UpdateLayout();
				
				// Simple calculus based on trigonometric functions
				mCanvasContainer.Width = Math.Abs(_ChildPresenter.ActualWidth *
					Math.Cos(MathsHelpers.ConvertAngle(RotationAngle, AngleUnit.Degrees, AngleUnit.Radians))) +
					Math.Abs(_ChildPresenter.ActualHeight *
					Math.Sin(MathsHelpers.ConvertAngle(RotationAngle, AngleUnit.Degrees, AngleUnit.Radians)));

				// Simple calculus based on trigonometric functions
				mCanvasContainer.Height = Math.Abs(_ChildPresenter.ActualWidth *
					Math.Sin(MathsHelpers.ConvertAngle(RotationAngle, AngleUnit.Degrees, AngleUnit.Radians))) +
					Math.Abs(_ChildPresenter.ActualHeight *
					Math.Cos(MathsHelpers.ConvertAngle(RotationAngle, AngleUnit.Degrees, AngleUnit.Radians)));
				
				// Create render transform for the content presenter
				_ChildPresenter.RenderTransform = new CompositeTransform()
				{
					// Negate the angle because the render transform counts it clockwise
					Rotation = -RotationAngle,
					// Simple calculus based on trigonometric functions
					TranslateY = _ChildPresenter.ActualWidth * Math.Sin(MathsHelpers.ConvertAngle(RotationAngle, AngleUnit.Degrees, AngleUnit.Radians)),
				};
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