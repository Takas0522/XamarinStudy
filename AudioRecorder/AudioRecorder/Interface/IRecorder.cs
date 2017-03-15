using System;
using System.IO;

namespace AudioRecorder.Interface
{
	public interface IRecorder
	{
		void RecStart();
		void RecEnd();
		string FileName { get; set; }
		string DirectryPath { get; set; }
	}
}
