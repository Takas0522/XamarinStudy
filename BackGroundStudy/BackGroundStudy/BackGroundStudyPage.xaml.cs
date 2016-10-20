using Xamarin.Forms;
using System;
using BackGroundStudy.Messages;
namespace BackGroundStudy
{
	public partial class BackGroundStudyPage : ContentPage
	{
		public BackGroundStudyPage()
		{
			InitializeComponent();
			StartButton.Clicked += (s, e) =>
			{
				var message = new StartLongRunningTaskMessage();
				MessagingCenter.Send(message, "StartLongRunningTaskMessage");
			};
			StopButton.Clicked += (s, e) =>
			{
				var message = new StopLongRunningTaskMessage();
				MessagingCenter.Send(message, "StopLongRunnningTaskMessage");
			};
		}
		public DateTime SleepDate 
		{ 
			set { this.sleepDate.Text = value.ToString("t");}
		}
	}
}
