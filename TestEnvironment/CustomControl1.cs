using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace TestEnvironment
{
	public sealed class CustomControl1 : Control
	{
		public CustomControl1()
		{
			this.DefaultStyleKey = typeof(CustomControl1);
			this.PointerEntered += (s, e) => Focus(FocusState.Programmatic);
			this.KeyDown += (s, e) => Debug.WriteLine("KeyDown");
		}

		public ICommand command { get; } = new RelayCommand(() => Debug.WriteLine("Control command"));
	}
}
