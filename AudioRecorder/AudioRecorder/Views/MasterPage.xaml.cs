using System;
using System.Collections.Generic;
using AudioRecorder.Model;
using Xamarin.Forms;

namespace AudioRecorder.Views
{
	public partial class MasterPage : ContentPage
	{
		public ListView ListView { get { return listView; } }

		public MasterPage()
		{
			InitializeComponent();

			var masterPageItems = new List<MasterPageItem>();
			masterPageItems.Add(new MasterPageItem
			{
				Title = "SaveSubscription",
				TargetType = typeof(SaveSubscriptionPage)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Scenario1",
				TargetType = typeof(ScenarionOnePage)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Reminders",
				TargetType = typeof(BlankPage)
			});

			listView.ItemsSource = masterPageItems;
		}
	}
}
