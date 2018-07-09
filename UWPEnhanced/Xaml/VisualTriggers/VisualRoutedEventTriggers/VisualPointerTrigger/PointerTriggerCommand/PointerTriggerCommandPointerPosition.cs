using System;
using System.Numerics;
using UWPEnhanced.ValueConverters;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// A class for in-xaml binding of commands that are triggered by pointer routed events and whose parameter is the pointer position
	/// relative to some entity casted to a <see cref="Complex"/>
	/// </summary>
	public class PointerTriggerCommandPointerPosition : PointerTriggerCommandBase
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
			typeof(TappedTriggerCommandPointerPosition), new PropertyMetadata(RelativeTo.AttachedTo));

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
			typeof(TappedTriggerCommandPointerPosition), new PropertyMetadata(default(UIElement)));

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
			typeof(PointerTriggerCommandPointerPosition), new PropertyMetadata(default(IComplexConverter)));

		#endregion

		#region Protected methods

		/// <summary>
		/// Because this class passes pointer position as a parameter, this method does not do anything and instead
		/// <see cref="OnTriggerEvent(object, TappedRoutedEventArgs)"/> is used to execute the command
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void ExecuteCommand(object sender, EventArgs e) { }

		/// <summary>
		/// Executes the command with the position of the cursor relative to <see cref="RelativeToElement"/>. If <see cref="RelativeToElement"/>
		/// is null, gets the position relative to <see cref="IAttachable.AttachedTo"/> (if it's an <see cref="UIElement"/>, if not
		/// then an exception is thrown). The position is passed as a <see cref="Complex"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnTriggerEvent(object sender, PointerRoutedEventArgs e)
		{
			PointerPoint pointerInfo = null;

			switch (RelativeToOption)
			{
				case RelativeTo.AttachedTo:
					{
						pointerInfo = e.GetCurrentPoint(AttachedTo as UIElement);
					}
					break;

				case RelativeTo.BoundObject:
					{
						pointerInfo = e.GetCurrentPoint(RelativeToElement);
					}
					break;

				case RelativeTo.Window:
					{
						pointerInfo = e.GetCurrentPoint(null);
					}
					break;
			}

			var param = new Complex(pointerInfo.Position.X, pointerInfo.Position.Y);

			Command?.Execute(Converter == null ? param : Converter.Convert(param));
		}

		#endregion
	}
}