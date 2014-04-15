using System.IO;
using tailDotNet.Configuration;

namespace tailDotNet
{
	public interface IWatchConfiguration
	{
		int PollIntervalInMs { get; set; }

		TextWriter OutPut { get; set; }
	}
}