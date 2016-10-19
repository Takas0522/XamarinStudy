using System;
namespace BackGroundApp.Messages
{
	public class DownloadProgressMessage
	{
		public long BytesWritten { get; set; }
		public long TotalBytesWritten { get; set; }
		public long TotalBytesExpectedToWrite { get; set; }
		public float Parcentage { get; set; }
	}
}
