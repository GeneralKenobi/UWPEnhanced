using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using UWPEnhanced.Controls;
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
			this.InitializeComponent();
			t();
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
		public ICommand MenuLeft { get; set; }
		public ICommand MenuTop { get; set; }
		public ICommand MenuRight { get; set; }
		public ICommand MenuBottom { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
