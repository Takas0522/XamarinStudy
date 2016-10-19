﻿using Foundation;
using UIKit;
using System;
using System.IO;
using System.Threading.Tasks;
namespace BackGroundApp.iOS.Services
{
	public class Downloader
	{
		private string targetFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "huge_mobkey.png");
		private const string sessionId = "com.xamarin.transfersession";
		private NSUrlSession session;
		readonly string _downloadFileUrl;

		public Downloader(string downloadFileUrl) {
			this._downloadFileUrl = downloadFileUrl;
		}

		public async Task DownloadFile() {
			this.InitializeSession();
			var pendingTasks = await this.session.GetTasksAsync();
			if (pendingTasks != null && pendingTasks.DownloadTasks != null) {
				foreach (var task in pendingTasks.DownloadTasks) { 
					task.Cancel();				
				}
				if (File.Exists(targetFileName)) {
					File.Delete(targetFileName);
				}
				this.EnqueDownload();
			}
		}
		void InitializeSession() {
			using (var sessionConfig = UIDevice.CurrentDevice.CheckSystemVersion(8, 0)
				   ? NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration(sessionId)
				   : NSUrlSessionConfiguration.BackgroundSessionConfiguration(sessionId)) {
				sessionConfig.NetworkServiceType = NSUrlRequestNetworkServiceType.Default;
				sessionConfig.HttpMaximumConnectionsPerHost = 2;
				var sessionDelegate = new CustomSessionDownloadDelegate(targetFileName);
				this.session = NSUrlSession.FromConfiguration(sessionConfig, sessionDelegate, null);
			}
		}
		void EnqueDownload() {
			var downloadTask = this.session.CreateDownloadTask(NSUrl.FromString(_downloadFileUrl));
			if (downloadTask == null) {
				new UIAlertView(string.Empty, "Failed to create download task", null, "OK").Show();
				return;
			}
			downloadTask.Resume();
			Console.WriteLine("STARTING DOWNLOAD. STATE OF TASK: '{0}' . ID: '{1}'", downloadTask.State, downloadTask.TaskIdentifier);
		}
	}
}
