using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.ProjectOxford.SpeakerRecognition;
using PCLStorage;
using System.Net.Http;
using System.Net.Http.Headers;

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



        public async Task<string> GetPhrase(string dirPath, string fileName)
        {
            byte[] byteData;
            try
            {
                var client = new HttpClient();
                var uri = "https://westus.api.cognitive.microsoft.com/spid/v1.0/verificationProfiles/{verificationProfileId}/enroll";
                var guid = await GetGuid();
                var sendUrl = uri.Replace("{verificationProfileId}", guid.ToString());

                var tokenAPI = "";
                tokenAPI = await GetSubscriptionKey();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", tokenAPI);

                var fileStream = await GetFileStream(dirPath, fileName);
                byteData = new byte[fileStream.Length];
                fileStream.Read(byteData, 0, byteData.Length);

                HttpResponseMessage response;
                var content = new ByteArrayContent(byteData);
                content.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");
                response = await client.PostAsync(sendUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    var retString = await response.Content.ReadAsStringAsync();
                    return retString;
                }
                else 
                {
                    return response.ReasonPhrase;
                }


                //using (var content = new ByteArrayContent(byteData))
                //{
                //    content.Headers.ContentType = new MediaTypeHeaderValue("audio/wav; samplerate=16000");
                //    //response = await client.PostAsync(sendUrl, content);
                //    //if (response.IsSuccessStatusCode)
                //    //{
                //    //    var retString = await response.Content.ReadAsStringAsync();
                //    //    return retString;
                //    //}
                //    //else
                //    //{
                //    //    return response.ReasonPhrase;
                //    //}
                //}
                return "aa";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private async Task<string> GetSubscriptionKey() 
        {
            var retText = "";
            var rootFolder = FileSystem.Current.LocalStorage;
            var res = await rootFolder.CheckExistsAsync("keyvalue.txt");
            if (res == ExistenceCheckResult.FileExists)
            { 
                var file = await rootFolder.GetFileAsync("keyvalue.txt");
                retText = await file.ReadAllTextAsync();
            }
            return retText;
        }


        public async Task InitService() 
        {
            var keyText = await GetSubscriptionKey();
            if (keyText != "")
            { 
                _serviceClient = new SpeakerVerificationServiceClient(keyText);
            }
        }

        private async Task<Stream> GetFileStream(string dirPath, string fileName)
        { 
            var rootFolder = FileSystem.Current.LocalStorage;
            var file = await rootFolder.GetFileAsync(Path.Combine(dirPath, fileName));
            return await file.OpenAsync(FileAccess.ReadAndWrite);            
        }

        public async Task<string> SendEnrollmentData(string dirPath, string fileName) 
        {

            var fileStream = await GetFileStream(dirPath, fileName);

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
