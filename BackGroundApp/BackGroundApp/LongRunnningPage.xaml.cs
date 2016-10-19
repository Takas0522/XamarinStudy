using System;
using System.Collections.Generic;
using BackGroundApp.Messages;
using Xamarin.Forms;

namespace BackGroundApp
{
	public partial class LongRunnningPage : ContentPage
	{
		public LongRunnningPage()
		{
			InitializeComponent();

			longRunnningTask.Clicked += (s, e) =>
			{
				var message = new StartLongRunningTasgMessgae();
				MessagingCenter.Send(message, "StartLongRunningMessage");
			};

			stopLongRunnning.Clicked += (s, e) =>
			{
				var message = new StopLongRunningTaskMessage();
				MessagingCenter.Send(message, "StopLongRunnningMessage");
			};
		}

		void HandleReceivedMessage() {
			MessagingCenter.Subscribe<TickedMessage>(this, "TickedMessage", message =>
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					ticker.Text = message.Message;
				});
			});
			MessagingCenter.Subscribe<CancelledMessage>(this, "CancelledMessage", message =>
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					ticker.Text = "Canceled!";
				});
			});
		}
	}
}
