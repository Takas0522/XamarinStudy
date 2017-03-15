using System;
using AudioRecorder.Interface;
using Xamarin.Forms;
using PCLStorage;
using System.IO;

namespace AudioRecorder
{
	public partial class AudioRecorderPage : ContentPage
	{

		private void changeButtonsEnable(Boolean isRecording) { 
			StartButton.IsEnabled = !isRecording;
			EndButton.IsEnabled = isRecording;			
		}

		public AudioRecorderPage()
		{
			InitializeComponent();

			changeButtonsEnable(false);
			SendButton.IsEnabled = false;

			StartButton.Clicked += new EventHandler(StartButtonClicked);
			EndButton.Clicked += new EventHandler(EndButtonClicked);
			SendButton.Clicked += new EventHandler(SendButtonClicked);
		}

		private void StartButtonClicked(object sender, EventArgs e) {

			changeButtonsEnable(true);

			var recService = DependencyService.Get<IRecorder>();

			SendButton.IsEnabled = false;
			recService.RecStart();
		}

		private void EndButtonClicked(object sender, EventArgs e) {

			changeButtonsEnable(false);

			var recService = DependencyService.Get<IRecorder>();
			recService.RecEnd();

			SendButton.IsEnabled = true;
			fileName = recService.FileName;
		}

		private string fileName;
		private string directoryPath;

		private async void SendButtonClicked(object sender, EventArgs e)
		{
			SendButton.IsEnabled = false;

			var rootFolder = FileSystem.Current.LocalStorage;
			var file = await rootFolder.GetFileAsync(directoryPath + fileName);
			var fileStream = await file.OpenAsync(FileAccess.Read);

			SendButton.IsEnabled = true;
		}
	}
}
