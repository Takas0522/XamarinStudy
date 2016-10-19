using System;
using Xamarin.Forms;

namespace BackGroundApp
{
	public partial class App : Application
	{
		private readonly BackGroudPage _backGroundPage; 
		public App()
		{
			InitializeComponent();

			var tabbedPage = new TabbedPage();
			tabbedPage.Children.Add(_backGroundPage);
			tabbedPage.Children.Add(new LongRunnningPage());
			tabbedPage.Children.Add(new DownloadPage());

			MainPage = tabbedPage;
		}

		protected override void OnStart()
		{
			LoadPersistedValues();
		}

		protected override void OnSleep()
		{
			Application.Current.Properties["SleepDate"] = DateTime.Now.ToString("O");
			Application.Current.Properties["FirstName"] = _backGroundPage.FirstName;
		}

		protected override void OnResume()
		{
			LoadPersistedValues();
		}

		private void LoadPersistedValues() {
			if (Application.Current.Properties.ContainsKey("FirstName")) {
				var firstName = (string)Application.Current.Properties["FirstName"];
				_backGroundPage.FirstName = firstName;
			}
		}
	}
}
