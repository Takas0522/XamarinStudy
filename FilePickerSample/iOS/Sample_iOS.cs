using System;
using Xamarin.Forms;
using FilePickerSample.Interface;
using FilePickerSample.iOS;

using CloudKit;

//[assembly: Dependency(typeof(Sample_iOS))]

namespace FilePickerSample.iOS
{
	//public class Sample_iOS : ISample
	//{
	//	public CKDatabase PublicDatabase { get; set; }
	//	public CKDatabase PrivateDatabase { get; set; }

	//	private string _PublicDatabaseName;
	//	public string PublicDatabaseName
	//	{
	//		get
	//		{
	//			return _PublicDatabaseName;
	//		}
	//	}

	//	private string _PrivateDatabaseName; 
	//	public string PrivateDatabaseName
	//	{
	//		get
	//		{
	//			return _PrivateDatabaseName;
	//		}
	//	}

	//	public void runFunction()
	//	{
	//		PublicDatabase = CKContainer.DefaultContainer.PublicCloudDatabase;
	//		PrivateDatabase = CKContainer.DefaultContainer.PrivateCloudDatabase;
	//		_PublicDatabaseName = PublicDatabase.ToString();
	//		_PrivateDatabaseName = PrivateDatabase.ToString();
	//	}
	//}
}
