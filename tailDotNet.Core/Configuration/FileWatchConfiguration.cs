using System.IO;

namespace tailDotNet.Configuration
{
	public class FileWatchConfiguration : IWatchConfiguration
	{
		private int _pollIntervalInMs = 100;
		public int PollIntervalInMs
		{
			get { return _pollIntervalInMs; }
			set { _pollIntervalInMs = value; }
		}

		public string FileName { get; set; }
		public TextWriter OutPut { get; set; }
	}
}
