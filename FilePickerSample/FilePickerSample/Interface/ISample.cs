using System;
namespace FilePickerSample.Interface
{
	public interface ISample
	{
		void runFunction();
		string PublicDatabaseName { get; }
		string PrivateDatabaseName { get; }
	}
}
