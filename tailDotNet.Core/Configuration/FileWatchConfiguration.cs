using System;
using System.IO;
using tailDotNet.Filtering;

namespace tailDotNet.Configuration
{
	public class FileWatchConfiguration : IWatchConfiguration
	{
		/// <summary>
		/// Poll interval in milliseconds. Default is 100ms.
		/// </summary>
		public int PollIntervalInMs { get; set; } = 100;

		public IObserver<TailPayload> Observer { get; set; }

		public string FileName { get; set; }
		public bool Follow { get; set; }
		public TextWriter OutPut { get; set; }
		public WatchFilter WatchFilter { get; set; }
		public int NumberOfLinesToOutputWhenWatchingStarts { get; set; }

		public bool HasWatchFilter => WatchFilter != null;
	}
}