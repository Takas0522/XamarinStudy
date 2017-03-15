using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.ProjectOxford.SpeakerRecognition;
using PCLStorage;

namespace AudioRecorder.Services
{
	public class VerificationService
	{
		private SpeakerVerificationServiceClient _serviceClient;
		private string _yourVerificationPhrase;
		private static readonly string SPEAKER_FILENAME = "SpeakerId";

		string YourVerificationPhrase { 
			get { return _yourVerificationPhrase; }
			set { _yourVerificationPhrase = value; }
		}

		public VerificationService()
		{
			_yourVerificationPhrase = "EMPTY PHRASE";
		}

		public async Task InitService() 
		{ 
			var rootFolder = FileSystem.Current.LocalStorage;
			var res = await rootFolder.CheckExistsAsync("keyvalue.txt");
			if (res == ExistenceCheckResult.FileExists)
			{
				var file = await rootFolder.GetFileAsync("keyvalue.txt");
				var keyText = await file.ReadAllTextAsync();
				_serviceClient = new SpeakerVerificationServiceClient(keyText);
			}
		}

		public async Task<string> SendEnrollmentData(string dirPath, string fileName) 
		{
			var rootFolder = FileSystem.Current.LocalStorage;
			var file = await rootFolder.GetFileAsync(Path.Combine(dirPath, fileName));
			var fileStream = await file.OpenAsync(FileAccess.ReadAndWrite);

			var guid = await GetGuid();
			if (guid == Guid.Empty)
			{
				var rec = await CreateProfile();
				if (rec)
				{
					guid = await GetGuid();
				}
			}
			var response = await _serviceClient.EnrollAsync(fileStream, guid);
			return response.Phrase;
		}

		public async Task<Guid> GetGuid()
		{ 
			var rootFolder = FileSystem.Current.LocalStorage;
			var ret = await rootFolder.CheckExistsAsync(SPEAKER_FILENAME);
			var retGuid = Guid.Empty;
			if (ret == ExistenceCheckResult.FileExists)
			{
				var file = await rootFolder.GetFileAsync(SPEAKER_FILENAME);
				var fileStream = await file.OpenAsync(FileAccess.Read);
				var readStrem = new StreamReader(fileStream);
				var st = readStrem.ReadLine();
				retGuid = new Guid(st);
				readStrem.Dispose();
				fileStream.Dispose();
			}
			return retGuid;
		}

		public async Task<bool> CreateProfile()
		{
			try
			{
				var response = await _serviceClient.CreateProfileAsync("en-us");
				var speakerId = response.ProfileId;
				var rootFolder = FileSystem.Current.LocalStorage;
				var file = await rootFolder.CreateFileAsync(SPEAKER_FILENAME, CreationCollisionOption.ReplaceExisting);
				await file.WriteAllTextAsync(speakerId.ToString());
				return true;
			}
			catch
			{
				return false;
			}

		}

	}
}
