using System;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace UWPEnhanced.Xaml
{
	/// <summary>
	/// Class providing a possibility to bind up to 3 properties, manipulate them using a converter and set a dependency property that
	/// can then be used to bind to a single property. It does support maximum of 3 properties because, if there's a necessity to
	/// resolve a value based on more properties then probably the design is faulty or a custom/user control would be better.
	/// </summary>
	public class MultiBinding : VisualAttachment, INotifyPropertyChanged
	{
		#region Events

		/// <summary>
		/// Event fired whenever a property changes its value
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Public properties

		/// <summary>
		/// The value obtained by combining <see cref="Value1"/>, <see cref="Value2"/>, <see cref="Value3"/> and 
		/// </summary>
		public object CombinedValue { get; private set; }

		#endregion

		#region Value1 Dependency Property

		/// <summary>
		/// First property to bind to something
		/// </summary>
		public object Value1
		{
			get => GetValue(Value1Property);
			set => SetValue(Value1Property, value);
		}

		/// <summary>
		/// Backing store for <see cref="Value1"/>
		/// </summary>
		public static readonly DependencyProperty Value1Property =
			DependencyProperty.Register(nameof(Value1), typeof(object),
			typeof(MultiBinding), new PropertyMetadata(default(object), new PropertyChangedCallback(ValueChangedCallback)));

		#endregion

		#region Value2 Dependency Property

		/// <summary>
		/// Second property to bind to something
		/// </summary>
		public object Value2
		{
			get => GetValue(Value2Property);
			set => SetValue(Value2Property, value);
		}

		/// <summary>
		/// Backing store for <see cref="Value2"/>
		/// </summary>
		public static readonly DependencyProperty Value2Property =
			DependencyProperty.Register(nameof(Value2), typeof(object),
			typeof(MultiBinding), new PropertyMetadata(default(object), new PropertyChangedCallback(ValueChangedCallback)));

		#endregion

		#region Value3 Dependency Property

		/// <summary>
		/// Third property to bind to something
		/// </summary>
		public object Value3
		{
			get => (object)GetValue(Value3Property);
			set => SetValue(Value3Property, value);
		}

		/// <summary>
		/// Backing store for <see cref="Value3"/>
		/// </summary>
		public static readonly DependencyProperty Value3Property =
			DependencyProperty.Register(nameof(Value3), typeof(object),
			typeof(MultiBinding), new PropertyMetadata(default(object), new PropertyChangedCallback(ValueChangedCallback)));

		#endregion

		#region Converter Dependency Property

		/// <summary>
		/// 
		/// </summary>
		public IMultiBindingValueConverter Converter
		{
			get => (IMultiBindingValueConverter)GetValue(ConverterProperty);
			set => SetValue(ConverterProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Converter"/>
		/// </summary>
		public static readonly DependencyProperty ConverterProperty =
			DependencyProperty.Register(nameof(Converter), typeof(IMultiBindingValueConverter),
			typeof(MultiBinding), new PropertyMetadata(default(IMultiBindingValueConverter)));

		#endregion

		#region ConverterParameter Dependency Property

		/// <summary>
		/// Parameter to pass to <see cref="Converter"/> for conversion
		/// </summary>
		public object ConverterParameter
		{
			get => (object)GetValue(ConverterParameterProperty);
			set => SetValue(ConverterParameterProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="ConverterParameter"/>
		/// </summary>
		public static readonly DependencyProperty ConverterParameterProperty =
			DependencyProperty.Register(nameof(ConverterParameter), typeof(object),
			typeof(MultiBinding), new PropertyMetadata(default(object)));

		#endregion

		#region Private methods

		/// <summary>
		/// Updates <see cref="CombinedValue"/>, 
		/// </summary>
		private void UpdateCombinedValue()
		{
			// Check if converter is not null
			if (Converter == null)
			{
				// It if is, throw an exception (as it's necessary to operate)
				throw new Exception(nameof(Converter) + " cannot be null");
			}

			// Compute new value
			CombinedValue = Converter.Convert(Value1, Value2, Value3, ConverterParameter);

			// Notify about it
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CombinedValue)));
		}

		#endregion

		#region Private static methods

		/// <summary>
		/// Callback for when <see cref="Value1"/>, <see cref="Value2"/> or <see cref="Value3"/> changes.
		/// Updates <see cref="CombinedValue"/>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void ValueChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			// Check if sender is a multibinding and if the new value differs from the old one
			if(sender is MultiBinding binding && !object.Equals(e.OldValue, e.NewValue))
			{
				binding.UpdateCombinedValue();
			}
		}

		#endregion
	}
}