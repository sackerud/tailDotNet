using System;
using System.IO;

namespace tailDotNet.Configuration
{
	public interface IWatchConfiguration
	{
		int PollIntervalInMs { get; set; }

		IObserver<TailPayload> Observer { get; set; }

		[Obsolete("Use Observer instead")]
		TextWriter OutPut { get; set; }
	}
}