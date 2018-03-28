using CSharpEnhanced.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UWPEnhanced.Xaml
{
	public class VisualSetter : VisualAttachment
	{			
		#region Property Dependency Property

		/// <summary>
		/// Property to assign using the setter
		/// </summary>
		public PropertyPath Property
		{
			get => (PropertyPath)GetValue(PropertyProperty);
			set => SetValue(PropertyProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Property"/>
		/// </summary>
		public static readonly DependencyProperty PropertyProperty =
			DependencyProperty.Register(nameof(Property), typeof(PropertyPath),
			typeof(VisualSetter), new PropertyMetadata(default(PropertyPath)));

		#endregion

		#region Target Dependency Property

		/// <summary>
		/// Target of the setter
		/// </summary>
		public object Target
		{
			get => GetValue(TargetProperty);
			set => SetValue(TargetProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Target"/>
		/// </summary>
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register(nameof(Target), typeof(object),	typeof(VisualSetter), new PropertyMetadata(default(object)));

		#endregion

		#region Value Dependency Property

		/// <summary>
		/// Value to apply with with the setter
		/// </summary>
		public object Value
		{
			get => GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Value"/>
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(nameof(Value), typeof(object),
			typeof(VisualSetter), new PropertyMetadata(default(object), new PropertyChangedCallback((s, e) =>
			{
				if(s is VisualSetter setter && e.NewValue is string str && TypeHelpers.TryParseNumber(str, out object castedNumber))
				{
					setter.Value = (int)castedNumber;
				}
			})));

		#endregion


		#region test Dependency Property

		/// <summary>
		/// 
		/// </summary>
		public object test
		{
			get => (object)GetValue(testProperty);
			set => SetValue(testProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="test"/>
		/// </summary>
		public static readonly DependencyProperty testProperty =
			DependencyProperty.Register(nameof(test), typeof(object),
			typeof(VisualSetter), new PropertyMetadata(null));

		#endregion



		public void Activate()
		{
			var a = Target.GetType();
			var aa = Target as DependencyObject;
			test = 1;
			
			
			var b = a.GetProperty(Property.Path);
			var c = a.GetProperty("Grid.Row");
			var cc = a.GetProperty("Grid.RowProperty");
			var ccc = a.GetProperty("(Grid.RowProperty)");
			var cccc = a.GetProperty("Border.(Grid.RowProperty)");
			var ccccc = a.GetProperty("(Border).(Grid.RowProperty)");
			var d = a.GetProperty("DependencyObject.Grid.Row");
			var e = a.GetProperty("(Grid.RowProperty)");
			var f = a.GetProperty("(Grid).(RowProperty)");
			var g = a.GetProperty("DependencyObject.(Grid.RowProperty)");
			var h = a.GetProperty("(DependencyObject).(Grid.RowProperty)");
			var i = a.GetProperty("DependencyObject.(Grid).(RowProperty)");
			var j = a.GetProperty("(DependencyObject).(Grid).(RowProperty)");
			var k = a.GetProperty("(DependencyObject).(Grid).RowProperty");
			var l = a.GetProperty("(DependencyObject).Grid.(RowProperty)");
			var m = a.GetProperty("(DependencyObject.Grid.RowProperty)");
			var o = a.GetProperty("Row)");
			//Target.GetType().GetProperty(Property.Path).SetValue(Target, Value);
		}

		public void Deactivate()
		{

		}

	}
}
