using System;
using System.IO;
using tailDotNet.Filtering;

namespace tailDotNet.Configuration
{
	public interface IWatchConfiguration
	{
		int PollIntervalInMs { get; set; }

		IObserver<TailPayload> Observer { get; set; }

		WatchFilter WatchFilter { get; set; }
		bool HasWatchFilter { get; }

		[Obsolete("Use Observer instead")]
		TextWriter OutPut { get; set; }
	}
}