using System;
using Xamarin.Forms;

namespace BackGroundStudy
{
	public partial class App : Application
	{
		private BackGroundStudyPage _backgroundPage;
		public App()
		{
			InitializeComponent();
			_backgroundPage = new BackGroundStudyPage();
			MainPage = _backgroundPage;
		}

		protected override void OnStart()
		{
			LoadPersistedValues();
		}

		protected override void OnSleep()
		{
			Application.Current.Properties["SleepDate"] = DateTime.Now.ToString("O");
		}

		protected override void OnResume()
		{
			LoadPersistedValues();
		}

		private void LoadPersistedValues() 
		{
			if (Application.Current.Properties.ContainsKey("SleepDate")) {
				var value = (string)Application.Current.Properties["SleepDate"];
				DateTime sleepDate;
				if (DateTime.TryParse(value, out sleepDate)) {
					_backgroundPage.SleepDate = sleepDate;
				}
			}	
		}
	}
}
