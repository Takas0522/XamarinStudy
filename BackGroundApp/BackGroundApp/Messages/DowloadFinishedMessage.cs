using System;
namespace BackGroundApp.Messages
{
	public class DowloadFinishedMessage
	{
		public string Url { get; set;}
		public string FilePath { get; set; }
	}
}