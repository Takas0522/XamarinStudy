using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using BackGroundApp.iOS.Services;
using BackGroundApp.Messages;
using Xamarin.Forms;

namespace BackGroundApp.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		void WireUpDownLoadTask() {
			MessagingCenter.Subscribe<DownloadMessage>(this, "Download", async message =>
			{
				var downloader = new Downloader (message.Url);
				await downloader.DownloadFile();
			});
		}

		public static Action BackgroundSessionCompletionHandler;

		public override void HandleEventsForBackgroundUrl(UIApplication application, string sessionIdentifier, Action completionHandler)
		{
			Console.WriteLine("HandleEventsForBackgroundUrl(): " + sessionIdentifier);
			// We get a completion handler which we are supposed to call if our transfer is done.
			BackgroundSessionCompletionHandler = completionHandler;
		}

		iOSLongRunningTaskExample longRunningTaskExample;

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}

		void WireUpLongRunningTask()
		{
			MessagingCenter.Subscribe<StartLongRunningTasgMessgae>(this, "StartLongRunningTaskMessage", async message =>
			{
				longRunningTaskExample = new iOSLongRunningTaskExample();
				await longRunningTaskExample.Start();
			});

			MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunningTaskMessage", message =>
			{
				longRunningTaskExample.Stop();
			});
		}
	}
}
