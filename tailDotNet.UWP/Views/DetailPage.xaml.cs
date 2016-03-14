using tailDotNet.UWP.ViewModels;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;

namespace tailDotNet.UWP.Views
{
	public sealed partial class DetailPage : Page
	{
		public DetailPage()
		{
			InitializeComponent();
			NavigationCacheMode = NavigationCacheMode.Disabled;
		}
	}
}

