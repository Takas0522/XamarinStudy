using System;
using System.Collections.Generic;
using AudioRecorder.Model;
using Xamarin.Forms;
using PCLStorage;
using System.Threading.Tasks;

namespace AudioRecorder.Views
{
	public partial class SaveSubscriptionPage : ContentPage
	{
		private readonly string FILE_NAME = "keyvalue.txt";
		public SaveSubscriptionPage()
		{
			InitializeComponent();
			LoadSubscriptionText();
			SaveButton.Clicked += onClickSaveButton;
		}

		private async Task LoadSubscriptionText()
		{ 
			var rootFolder = FileSystem.Current.LocalStorage;
			var res = await rootFolder.CheckExistsAsync(FILE_NAME);
			if (res == ExistenceCheckResult.FileExists)
			{ 
				var file = await rootFolder.GetFileAsync(FILE_NAME);
				KeyText.Text = await file.ReadAllTextAsync();
			}
		}

		private async void onClickSaveButton(object sender, EventArgs e)
		{
			var rootFolder = FileSystem.Current.LocalStorage;
			var file = await rootFolder.CreateFileAsync(FILE_NAME,CreationCollisionOption.ReplaceExisting);
			await file.WriteAllTextAsync(KeyText.Text);
		}
	}
}
