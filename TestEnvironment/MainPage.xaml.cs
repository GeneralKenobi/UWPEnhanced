using System.Threading.Tasks;
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
	public sealed partial class MainPage : Page
    {
        public MainPage()
        {
			this.InitializeComponent();
			task();
        }


		private async Task task()
		{
			await Task.Delay(1000);
			(this.Resources["sb"] as Storyboard)?.Begin();
		}
	}
}
