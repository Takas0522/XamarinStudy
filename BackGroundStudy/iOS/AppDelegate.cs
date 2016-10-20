using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin.Forms;
using BackGroundStudy.Messages;

namespace BackGroundStudy.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			MessagingCenter.Subscribe<StartLongRunningTaskMessage>(this, "StartLongRunnningTaskMessage", async messages =>
			{
				longRunningTaskExample = new iOSLongRunningTaskExample();
				await longRunningTaskExample.start();

			});

			MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunnningTaskMessage", message =>
			{
				lonRunningTaskExample.Stop();
			});

			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
