using System;
using AudioRecorder.Interface;
using Xamarin.Forms;
using PCLStorage;
using System.IO;

namespace AudioRecorder
{
	public partial class AudioRecorderPage : ContentPage
	{
		public AudioRecorderPage()
		{
			InitializeComponent();

			StartButton.IsEnabled = true;
			EndButton.IsEnabled = false;

			StartButton.Clicked += new EventHandler(StartButtonClicked);
			EndButton.Clicked += new EventHandler(EndButtonClicked);
		}
		private void StartButtonClicked(object sender, EventArgs e) {
			StartButton.IsEnabled = false;
			EndButton.IsEnabled = true;

			var recService = DependencyService.Get<IRecorder>();
			recService.RecStart();
		}
		private void EndButtonClicked(object sender, EventArgs e) {
			StartButton.IsEnabled = true;
			EndButton.IsEnabled = false;

			var recService = DependencyService.Get<IRecorder>();
			recService.RecEnd();
		}
	}
}
