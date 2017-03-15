using System;
namespace AudioRecorder.Model
{
	public class ConstValue
	{
		private static ConstValue _singleInstance = new ConstValue();
		public static ConstValue GetInstance()
		{
			return _singleInstance;
		}
		public readonly string EnrollFileName = "EnrollFile.wav";
		public readonly string IdentifyFileName = "IdentifyFile.wav";
	}
}
