using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace tailDotNet.UWP.Views
{
	public sealed partial class Shell : Page
	{
		public static Shell Instance { get; set; }
		public static HamburgerMenu HamburgerMenu { get { return Instance.MyHamburgerMenu; } }

		public Shell()
		{
			Instance = this;
			InitializeComponent();
			
		}

		public Shell(INavigationService navigationService) : this()
		{
			SetNavigationService(navigationService);
		}

		public void SetNavigationService(INavigationService navigationService)
		{
			MyHamburgerMenu.NavigationService = navigationService;
		}

		private async void SymbolIcon_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			await PickFileAsync();
		}

		private async void SymbolIcon_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			await PickFileAsync();
		}

		private async Task<IReadOnlyList<StorageFile>> PickFileAsync()
		{
			var picker = new FileOpenPicker();
			var files = await picker.PickMultipleFilesAsync();

			if (files == null) return null;
			
			return files;
		}

		private async void HamburgerButtonInfo_Tapped(object sender, RoutedEventArgs e)
		{
			await PickFileAsync();
		}
	}
}