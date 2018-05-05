using CSharpEnhanced.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using UWPEnhanced.Controls;
using UWPEnhanced.Helpers;
using UWPEnhanced.Xaml;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestEnvironment
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
    {
        public MainPage()
        {			
			this.InitializeComponent();
			var a = sb1.Children.Count;
			var b = sb2.Children.Count;
			var c = sb1.Duration;
			var d = sb2.Duration;
			var e = sb1.Duration.HasTimeSpan;
			var f = sb2.Duration.HasTimeSpan;
		}

		public double D { get; set; } = 1;

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			menu.Position = MenuPosition.Left;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{			
			menu.Position = MenuPosition.Top;
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			menu.Position = MenuPosition.Right;
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			menu.Position = MenuPosition.Bottom;
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			menu.IsOpen = !menu.IsOpen;			
		}
	}
}




