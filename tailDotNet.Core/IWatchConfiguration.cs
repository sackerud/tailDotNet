using System.IO;

namespace tailDotNet
{
	interface IWatchConfiguration
	{
		int PollIntervalInMs { get; set; }
		TextWriter OutPut { get; set; }
	}
}