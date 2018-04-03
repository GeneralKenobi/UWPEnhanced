using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using UWPEnhanced.Xaml;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
		
		
			//MenuLeft = new RelayCommand(() => menu.Position = MenuPosition.Left);
			//MenuTop = new RelayCommand(() => menu.Position = MenuPosition.Top);
			//MenuRight = new RelayCommand(() => menu.Position = MenuPosition.Right);
			//MenuBottom = new RelayCommand(() => menu.Position = MenuPosition.Bottom);
		}
		private async void t()
		{
			await Task.Delay(1000);
			VisualStateManager.GoToState(this, "State2", true);
			await Task.Delay(1000);
			VisualStateManager.GoToState(this, "State1", true);
			await Task.Delay(1000);
			VisualStateManager.GoToState(this, "State2", true);
			await Task.Delay(1000);
			VisualStateManager.GoToState(this, "State4", true);
			var a = VisualStateManager.GetVisualStateGroups(this);
			var b = a.Count;
			a.Add(new VisualStateGroup());
			a[0].States.Add(new VisualState());
		}

		TaskCompletionSource<bool> StoryboardCompleted = null;
		private async void t2()
		{
			await Task.Delay(1000);

			

			
		}

		AutoResetEvent testReset = new AutoResetEvent(false);
		SemaphoreSlim testSemaphore = new SemaphoreSlim(1,1);

		private EventHandler testevent;
		public ICommand MenuLeft { get; set; }
		public ICommand MenuTop { get; set; }
		public ICommand MenuRight { get; set; }
		public ICommand MenuBottom { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var task = VisualManager.GoToSetup(RootGrid, "test");
			Task.Run(() =>
			{
				int a = task.Result;
			});
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			var task = VisualManager.GoToSetup(RootGrid, "Normal");
			Task.Run(() =>
			{
				int a = task.Result;
			});
		}
	}
}
