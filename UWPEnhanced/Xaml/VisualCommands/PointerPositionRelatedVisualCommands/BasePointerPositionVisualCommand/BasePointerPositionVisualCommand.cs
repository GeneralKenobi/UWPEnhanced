using UWPEnhanced.ValueConverters;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Base class for pointer commands that use the position of the click as Command's parameter. Introduces dependency properties
	/// that allow for customization of how the pointer position is determined.
	/// </summary>
	/// <typeparam name="TVisualTrigger">Specific type of the visual trigger to use</typeparam>
	public abstract class BasePointerPositionVisualCommand<TVisualTrigger, TEventArgs> : BaseVisualCommand<TVisualTrigger, TEventArgs>
		where TVisualTrigger : class, IVisualTrigger
	{
		#region RelativeTo Dependency Property

		/// <summary>
		/// Determines what the pointer position is related to when determining it (default value of <see cref="RelativeTo.AttachedTo"/>
		/// </summary>
		public RelativeTo RelativeToOption
		{
			get => (RelativeTo)GetValue(RelativeToOptionProperty);
			set => SetValue(RelativeToOptionProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="RelativeToOption"/>
		/// </summary>
		public static readonly DependencyProperty RelativeToOptionProperty =
			DependencyProperty.Register(nameof(RelativeToOption), typeof(RelativeTo),
			typeof(BasePointerPositionVisualCommand<TVisualTrigger, TEventArgs>), new PropertyMetadata(RelativeTo.AttachedTo));

		#endregion

		#region RelativeTo Dependency Property

		/// <summary>
		/// Element relative to which the pointer position will be determined
		/// </summary>
		public UIElement RelativeToElement
		{
			get => (UIElement)GetValue(RelativeToElementProperty);
			set => SetValue(RelativeToElementProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="RelativeToElement"/>
		/// </summary>
		public static readonly DependencyProperty RelativeToElementProperty =
			DependencyProperty.Register(nameof(RelativeToElement), typeof(UIElement),
			typeof(BasePointerPositionVisualCommand<TVisualTrigger, TEventArgs>), new PropertyMetadata(default(UIElement)));

		#endregion

		#region Converter Dependency Property

		/// <summary>
		/// Converter which is used to converter the position before executing the method. If no converter is specified then
		/// no conversion occurs
		/// </summary>
		public IComplexConverter Converter
		{
			get => (IComplexConverter)GetValue(ConverterProperty);
			set => SetValue(ConverterProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Converter"/>
		/// </summary>
		public static readonly DependencyProperty ConverterProperty =
			DependencyProperty.Register(nameof(Converter), typeof(IComplexConverter),
			typeof(BasePointerPositionVisualCommand<TVisualTrigger, TEventArgs>), new PropertyMetadata(default(IComplexConverter)));

		#endregion
	}
}