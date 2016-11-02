using System.Threading;
using System.Threading.Tasks;
using BackGroundStudy.Messages;
using Xamarin.Forms;
using PCLStorage;

namespace BackGroundStudy
{
	public class TaskCounter
	{
		private static readonly string FilePath = "/sdcard/sample";

		public async Task RunCounter(CancellationToken token)
		{
			await Task.Run(async () =>
			{
				//2500msごとにメッセージセンターにメッセージをバックグラウンドでsendする。
				for (long i = 0; i < long.MaxValue; i++)
				{
					token.ThrowIfCancellationRequested();
					await Task.Delay(2500);
					var message = new TickedMessage { Message = i.ToString() };
					Device.BeginInvokeOnMainThread(() =>
					{
						MessagingCenter.Send<TickedMessage>(message, "TickedMessage");
					});

					IFolder rootFolder = await FileSystem.Current.GetFolderFromPathAsync(FilePath);
					IFile file = await rootFolder.CreateFileAsync("data.txt", CreationCollisionOption.GenerateUniqueName);
					await file.WriteAllTextAsync(i.ToString());
				}
			}, token);
		}
	}
}
