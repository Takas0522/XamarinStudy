using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using BackGroundApp.Messages;
using BackGroundApp.Droid.Services;

namespace BackGroundApp.Droid
{
	[Activity(Label = "BackGroundApp.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication(new App());

			WireUpLongRunningTask();
			WireUpLongDownloadTask();
		}
		void WireUpLongRunningTask()
		{
			MessagingCenter.Subscribe<StartLongRunningTasgMessgae>(this, "StartLongRunningTaskMessage", message =>
			{
				var intent = new Intent(this, typeof(LongRunningTaskService));
				StartService(intent);
			});

			MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunningTaskMessage", message =>
			{
				var intent = new Intent(this, typeof(LongRunningTaskService));
				StopService(intent);
			});
		}

		void WireUpLongDownloadTask()
		{
			MessagingCenter.Subscribe<DownloadMessage>(this, "Download", message =>
			{
				var intent = new Intent(this, typeof(DownloaderService));
				intent.PutExtra("url", message.Url);
				StartService(intent);
			});
		}
	}
	}
}
