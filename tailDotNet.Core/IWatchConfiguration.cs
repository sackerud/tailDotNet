using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tailDotNet
{
	interface IWatchConfiguration
	{
		int PollIntervalInMs { get; set; }
		TextWriter OutPut { get; set; }
	}
}