using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Media;
using System.Diagnostics;
using Android.Webkit;
using AudioRecorder.Droid;
using AudioRecorder.Interface;
using Java.IO;
using Xamarin.Forms;
using Android.Content;
using System.IO;

[assembly: Dependency(typeof(Recorder_Droid))]

namespace AudioRecorder.Droid
{
	public class Recorder_Droid : IRecorder
	{
		int RECORDER_BPP = 16;
		int RECORDER_SAMPLERATE;
		ChannelIn RECORDER_CHANNELS = ChannelIn.Stereo;
		Encoding RECORDER_AUDIO_ENCODING = Encoding.Pcm16bit;

		AudioRecord recorder;
		int bufferSize;
		bool isRecording;
		CancellationTokenSource token;
		public void RecEnd()
		{
			if (recorder != null)
			{
				//RecorderのStopからリリースまで。Androidでおなじみの一連の流れ
				recorder.Stop();
				isRecording = false;
				token.Cancel();

				recorder.Release();
				recorder = null;
			}
			//蓄積していったデータをちゃんとした場所に格納する
			CopyWaveFile(GetTempFilename(), GetFilename());
		}
		public void RecStart()
		{
			//CrossPlatoform嬢上から画面上のサービスを呼び出すためのPlugin？？
			var context = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
			//AndroidでのAudioServiceを司るAudioMangerをcontext経由で呼び出し？
			var audioManager = (AudioManager)context.GetSystemService(Context.AudioService);
			//vaudioManager.GetProperty(AudioManager.PropertyOutputSampleRate)
			//現在使用しているデバイスでOutputするための基本ビットレートを取得する？
			RECORDER_SAMPLERATE = Int32.Parse(audioManager.GetProperty(AudioManager.PropertyOutputSampleRate));
			//RecorderObjectがすでに生成されている場合は破棄を行う
			if (recorder != null)
			{
				recorder.Release();
			}
			//SampleRateと録音時のモノラル等の設定、Encodingのビット数から最小BufferrSizeを算出する。
			bufferSize = AudioRecord.GetMinBufferSize(RECORDER_SAMPLERATE, ChannelIn.Mono, Encoding.Pcm16bit);
			//BufferSizeを確保した上でAudioRecordInstanceを生成する。
			//BufferSize指定時にStereoだったのにRECORDER_CHANNELSがStereoなのは録音側の設定だからかな？
			recorder = new AudioRecord(AudioSource.Mic, RECORDER_SAMPLERATE, RECORDER_CHANNELS, RECORDER_AUDIO_ENCODING, bufferSize);
			recorder.StartRecording();
			isRecording = true;

			token = new CancellationTokenSource();
			//RecordAudioはリアルタイムに音声処理を行うものなのでTaskで都度ファイルの書き込みを実行する。
			//BufferSize単位に実行される感じだと思う
			Task.Run(() => WriteAudioDataToFile(), token.Token);
		}
		/// <summary>
		/// Writes the audio data to file.
		/// </summary>
		void WriteAudioDataToFile()
		{
			//BufferSize分のByte配列を確保
			byte[] data = new byte[bufferSize];
			var filename = GetTempFilename();
			FileOutputStream outputStream = null;
			Debug.WriteLine(filename);

			try
			{
				//Streamを開く
				outputStream = new FileOutputStream(filename);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}

			if (outputStream != null)
			{
				while (isRecording)
				{
					//Recorderに蓄積されたデータを」BufferSizeで取り出し、Dataに格納する
					recorder.Read(data, 0, bufferSize);
					try
					{
						//Streamにデータ書き出し
						outputStream.Write(data);
					}
					catch (Exception ex)
					{
						Debug.WriteLine(ex.Message);
					}
				}

				try
				{
					outputStream.Close();
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
				}
			}
		}

		void CopyWaveFile(string tempFile, string permanentFile)
		{
			FileInputStream inputStream = null;
			FileOutputStream outputStream = null;
			long totalAudioLength = 0;
			long totalDataLength = totalAudioLength + 36;
			long sampleRate = RECORDER_SAMPLERATE;
			int channels = 2;
			long byteRate = RECORDER_BPP * RECORDER_SAMPLERATE * channels / 8;

			byte[] data = new byte[bufferSize];

			try
			{
				inputStream = new FileInputStream(tempFile);
				outputStream = new FileOutputStream(permanentFile);
				totalAudioLength = inputStream.Channel.Size();
				totalDataLength = totalAudioLength + 36;

				Debug.WriteLine("File size: " + totalDataLength);
				WriteWaveFileHeader(outputStream, totalAudioLength, totalDataLength, sampleRate, channels, byteRate);

				while (inputStream.Read(data) != -1)
				{
					outputStream.Write(data);
				}
				inputStream.Close();
				outputStream.Close();
				DeleteTempFile();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		void WriteWaveFileHeader(FileOutputStream outputStream, long audioLength, long dataLength, long sampleRate, int channels, long byteRate)
		{
			byte[] header = new byte[44];

			header[0] = Convert.ToByte('R'); // RIFF/WAVE header
			header[1] = Convert.ToByte('I'); // (byte)'I';
			header[2] = Convert.ToByte('F');
			header[3] = Convert.ToByte('F');
			header[4] = (byte)(dataLength & 0xff);
			header[5] = (byte)((dataLength >> 8) & 0xff);
			header[6] = (byte)((dataLength >> 16) & 0xff);
			header[7] = (byte)((dataLength >> 24) & 0xff);
			header[8] = Convert.ToByte('W');
			header[9] = Convert.ToByte('A');
			header[10] = Convert.ToByte('V');
			header[11] = Convert.ToByte('E');
			header[12] = Convert.ToByte('f');// 'fmt ' chunk
			header[13] = Convert.ToByte('m');
			header[14] = Convert.ToByte('t');
			header[15] = (byte)' ';
			header[16] = 16; // 4 bytes: size of 'fmt ' chunk
			header[17] = 0;
			header[18] = 0;
			header[19] = 0;
			header[20] = 1; // format = 1
			header[21] = 0;
			header[22] = Convert.ToByte(channels);
			header[23] = 0;
			header[24] = (byte)(sampleRate & 0xff);
			header[25] = (byte)((sampleRate >> 8) & 0xff);
			header[26] = (byte)((sampleRate >> 16) & 0xff);
			header[27] = (byte)((sampleRate >> 24) & 0xff);
			header[28] = (byte)(byteRate & 0xff);
			header[29] = (byte)((byteRate >> 8) & 0xff);
			header[30] = (byte)((byteRate >> 16) & 0xff);
			header[31] = (byte)((byteRate >> 24) & 0xff);
			header[32] = (byte)(2 * 16 / 8); // block align
			header[33] = 0;
			header[34] = Convert.ToByte(RECORDER_BPP); // bits per sample
			header[35] = 0;
			header[36] = Convert.ToByte('d');
			header[37] = Convert.ToByte('a');
			header[38] = Convert.ToByte('t');
			header[39] = Convert.ToByte('a');
			header[40] = (byte)(audioLength & 0xff);
			header[41] = (byte)((audioLength >> 8) & 0xff);
			header[42] = (byte)((audioLength >> 16) & 0xff);
			header[43] = (byte)((audioLength >> 24) & 0xff);

			outputStream.Write(header, 0, 44);
		}

		string GetTempFilename()
		{
			var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			return Path.Combine(path, "temp.wav");
		}

		string GetFilename()
		{
			//var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			var path = "/sdcard/";
			return Path.Combine(path, "Todo.wav");
		}

		void DeleteTempFile()
		{
			var file = new Java.IO.File(GetTempFilename());
			file.Delete();
		}
	}
}
