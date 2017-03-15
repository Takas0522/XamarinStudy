using System;
using CloudKitSample.iOS;
using Xamarin.Forms;
using CloudKitSample.Interface;
using CloudKit;
using UIKit;
[assembly: Dependency(typeof(Action_iOS))]
namespace CloudKitSample.iOS
{
	public class Action_iOS : IAction
	{
		public CKDatabase PublicDatabase { get; set; }
		public CKDatabase PrivateDatabase { get; set; }

		public void RunAction()
		{
			PublicDatabase = CKContainer.DefaultContainer.PublicCloudDatabase;
			PrivateDatabase = CKContainer.DefaultContainer.PrivateCloudDatabase;
		}
	}
}
