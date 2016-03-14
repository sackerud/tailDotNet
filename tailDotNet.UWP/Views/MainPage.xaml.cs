using System;
using tailDotNet.UWP.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;

namespace tailDotNet.UWP.Views
{
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			InitializeComponent();
			NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
		}
	}
}
