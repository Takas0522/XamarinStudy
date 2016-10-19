using Xamarin.Forms;
using System.Threading.Tasks;
using System.Threading;
using BackGroundApp.Messages;

using System;
namespace BackGroundApp
{
	public class TaskCounter
	{
		public async Task RunCounter(CancellationToken token) {
			await Task.Run(async () =>
			{
				for (long i = 0; i < long.MaxValue; i++)
				{
					token.ThrowIfCancellationRequested();
					await Task.Delay(250);
					var message = new TickedMessage
					{
						Message = i.ToString()
					};
					Device.BeginInvokeOnMainThread(() =>
					{
						MessagingCenter.Send<TickedMessage>(message, "ThickedMessage");
					});
				}
			}, token);
		}
	}
}
