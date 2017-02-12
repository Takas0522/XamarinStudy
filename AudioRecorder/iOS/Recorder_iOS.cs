using System;
using Xamarin.Forms;
using AudioRecorder.iOS;
using AudioRecorder.Interface;
using AVFoundation;
using Foundation;
using System.Runtime.InteropServices;
using System.IO;

[assembly: Dependency(typeof(Recorder_iOS))]

namespace AudioRecorder.iOS
{
	public class Recorder_iOS: IRecorder
	{
		AVAudioRecorder recorder;
		NSError error;
		NSUrl url; 
		NSDictionary settings;
		public void RecStart()
		{
			var audioSession = AVAudioSession.SharedInstance(); 
			var err = audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord); 
			if (err != null) 
			{ 
				Console.WriteLine("audioSession: {0}", err); 
			}
			err = audioSession.SetActive(true); 
			if (err != null) 
			{ 
				Console.WriteLine("audioSession: {0}", err); 
			}
			string fileName = string.Format ("Myfile{0}.wav", DateTime.Now.ToString ("yyyyMMddHHmmss")); 
			string audioFilePath = Path.Combine (Path.GetTempPath (), fileName);
			Console.WriteLine("Audio File Path: " + audioFilePath);
			url = NSUrl.FromFilename(audioFilePath); 
			//set up the NSObject Array of values that will be combined with the keys to make the NSDictionary 
			NSObject[] values = new NSObject[] 
			{ 
				NSNumber.FromFloat (44100.0f), //Sample Rate 
				NSNumber.FromInt32 ((int)AudioToolbox.AudioFormatType.LinearPCM), //AVFormat 
				NSNumber.FromInt32 (2), //Channels 
				NSNumber.FromInt32 (16), //PCMBitDepth 
				NSNumber.FromBoolean (false), //IsBigEndianKey
				NSNumber.FromBoolean (false) //IsFloatKey 
			};
			//Set up the NSObject Array of keys that will be combined with the values to make the NSDictionary 
			NSObject[] keys = new NSObject[] 
			{ 
				AVAudioSettings.AVSampleRateKey, 
				AVAudioSettings.AVFormatIDKey, 
				AVAudioSettings.AVNumberOfChannelsKey, 
				AVAudioSettings.AVLinearPCMBitDepthKey, 
				AVAudioSettings.AVLinearPCMIsBigEndianKey, 
				AVAudioSettings.AVLinearPCMIsFloatKey 
			};
			//Set Settings with the Values and Keys to create the NSDictionary 
			settings = NSDictionary.FromObjectsAndKeys (values, keys);
			//Set recorder parameters 
			recorder = AVAudioRecorder.Create(url, new AudioSettings(settings), out error);
			recorder.PrepareToRecord();
			recorder.Record();
		}

		public void RecEnd()
		{
			recorder.Stop();
		}
	}
}
