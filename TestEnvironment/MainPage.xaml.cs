using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using UWPEnhanced.Xaml;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
			test = new RelayCommand(() => Debugger.Break());
			this.InitializeComponent();
			task();
        }

		public int d { get; set; } = 0;

		private async Task task()
		{
			await Task.Delay(1000);
			d = 5;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("d"));
			(this.Resources["sb"] as Storyboard)?.Begin();
		}

		public ICommand test;

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
