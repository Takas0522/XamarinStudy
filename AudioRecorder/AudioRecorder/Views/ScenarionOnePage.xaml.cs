using System;
using System.Collections.Generic;
using AudioRecorder.Interface;
using AudioRecorder.Model;
using AudioRecorder.Services;
using Xamarin.Forms;
using System.IO;
using PCLStorage;
using System.Threading.Tasks;

namespace AudioRecorder.Views
{
    public partial class ScenarionOnePage : ContentPage
    {
        private Guid _audioGuid = Guid.Empty;
        private VerificationService _service = new VerificationService();

        public ScenarionOnePage()
        {
            InitializeComponent();
            ChangeIsEnableProperty(true);
        }

        private void ChangeIsEnableProperty(Boolean isEnabled)
        { 
            StartButton.IsEnabled = isEnabled;
            EndButton.IsEnabled = !isEnabled;
            StartButton.Clicked += onClickStartButton;
            EndButton.Clicked += onClickEndButton;
        }

        private void onClickStartButton(object sender, EventArgs e) 
        {
            ChangeIsEnableProperty(false);

            var recService = DependencyService.Get<IRecorder>();
            var constValue = ConstValue.GetInstance();
            recService.FileName = constValue.EnrollFileName;
            recService.RecStart();
        }

        private bool IsRecEnding = false;
        private async void onClickEndButton(object sender, EventArgs e)
        {
            if (IsRecEnding) return;
            IsRecEnding = true;
            var recService = DependencyService.Get<IRecorder>();
            var constValue = ConstValue.GetInstance();
            recService.RecEnd();
            var dirPath = recService.DirectryPath;
            var fileName = recService.FileName;

            var phrase = await _service.GetPhrase(dirPath, fileName);
            YourPhrase.Text = phrase;



            //await _service.InitService();
            //var phrase = await _service.SendEnrollmentData(dirPath, fileName);

            //var file = Path.Combine(dirPath, fileName);

            //var rootFolder = FileSystem.Current.LocalStorage;
            //var fileData = await rootFolder.GetFileAsync(file);
            //var fileStream = await fileData.OpenAsync(FileAccess.Read);
            //var byteData = ReadBinaryData(fileStream);
            //_audioGuid = new Guid(byteData);
            //var readStream = new StreamReader(fileStream);
            //var readString = readStream.ReadLine();
            //_audioGuid = new Guid(readString);

            ChangeIsEnableProperty(true);
            IsRecEnding = false;
        }
    }
}
