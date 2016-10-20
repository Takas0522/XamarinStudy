using System;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using BackGroundStudy.Messages;
using Xamarin.Forms;
namespace BackGroundStudy.iOS
{
	public class iOSLongRunningTaskExample
	{
		nint _taskId;
		CancellationTokenSource _cts;

		public async Task Start() {
			_cts = new CancellationTokenSource();
			_taskId = UIApplication.SharedApplication.BeginBackgroundTask("LongRunnningTask", OnExpiration);

			try
			{
				var counter = new TaskCounter();
				await counter.RunCounter(_cts.Token);
			}
			catch (OperationCanceledException)
			{

			}
			finally {
				if (_cts.IsCancellationRequested) {
					var message = new CancelledMessage();
					Device.BeginInvokeOnMainThread(() => MessagingCenter.Send(message, "CancelledMessage"));
				}
			}
			UIApplication.SharedApplication.EndBackgroundTask(_taskId);
		}

		public void Stop() {
			_cts.Cancel();
		}

		public void OnExpiration() {
			_cts.Cancel();
		}
	}
}
