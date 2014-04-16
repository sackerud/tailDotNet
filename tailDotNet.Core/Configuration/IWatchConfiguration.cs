using System.IO;

namespace tailDotNet.Configuration
{
	public interface IWatchConfiguration
	{
		int PollIntervalInMs { get; set; }

		TextWriter OutPut { get; set; }
	}
}