﻿using CSharpEnhanced.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using UWPEnhanced.Controls;
using UWPEnhanced.Helpers;
using UWPEnhanced.Xaml;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestEnvironment
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public MainPage()
        {
			this.InitializeComponent();

		}

		#region Events

		/// <summary>
		/// Event fired whenever a property changes its value
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		private Complex d = new Complex(0,0);

		public ObservableCollection<string> collection = new ObservableCollection<string>();

		public Complex D
		{
			get => d;
			set
			{
				d = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(D)));
			}
		} 


		public ICommand command { get; set; } = new RelayCommand(() => Debug.WriteLine("Command Fired"));

		//private void Button_Click(object sender, RoutedEventArgs e)
		//{
		//	menu.Position = MenuPosition.Left;
		//}

		//private void Button_Click_1(object sender, RoutedEventArgs e)
		//{
		//	menu.Position = MenuPosition.Top;
		//}

		//private void Button_Click_2(object sender, RoutedEventArgs e)
		//{
		//	menu.Position = MenuPosition.Right;
		//}

		//private void Button_Click_3(object sender, RoutedEventArgs e)
		//{
		//	menu.Position = MenuPosition.Bottom;
		//}

		//private void Button_Click_4(object sender, RoutedEventArgs e)
		//{
		//	menu.IsOpen = !menu.IsOpen;
		//}

		//private async void Button_Click_5(object sender, RoutedEventArgs e)
		//{
		//	menu.Position = MenuPosition.Top;
		//	await Task.Delay(100);
		//	menu.Position = MenuPosition.Right;
		//	await Task.Delay(100);
		//	menu.Position = MenuPosition.Bottom;
		//	await Task.Delay(100);
		//	menu.Position = MenuPosition.Left;
		//}

		//private void Button_Click_6(object sender, RoutedEventArgs e)
		//{
		//	collection.Add("new test");
		//}

		//private void Button_Click_7(object sender, RoutedEventArgs e)
		//{
		//	var rd = new Random();
		//	collection.RemoveAt(rd.Next() % collection.Count);
		//}
	}
}




