using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace tailDotNet.Console
{
    /// <summary>
    /// TailOptions reflects the UNIX tail command options where feasable.
    /// </summary>
	public class TailOptions
	{
        //[Option('e', "excludefilter", HelpText = "don't write to the console if the specified string exists in the current tail", MutuallyExclusiveSet = "filtering")]
	    [Option('e', "excludefilter", HelpText = "don't write to the console if the specified string exists in the current tail")]
        public string ExclusionFilter {get;set;}

		[Option('f', "follow", Default = false, HelpText = "output appended data as the file grows")]
		public bool Follow { get; set; }

        //[Option('i', "includefilter", HelpText = "write to the console only if the specified string exists in the current tail", MutuallyExclusiveSet = "filtering")]
	    [Option('i', "includefilter", HelpText = "write to the console only if the specified string exists in the current tail")]
        public string InclusionFilter { get; set; }

		[Option('n', "lines", Default = 10, HelpText = "output the last N lines, instead of the last 10")]
		public int Lines { get; set; }

		[Option('s', "sleep-interval", Default = 1.0d, HelpText = "with -f, sleep for approximately s seconds (default 1.0) between iterations")]
		public double SleepIntervalInSeconds { get; set; }

		[Option('~', "version", Default = false, HelpText = "output version information and exit")]
		public bool Version { get; set; }

		[Value(0)]
		public IList<string> Filename { get; set; }

//		[HelpOption]
//		public string GetUsage()
//		{
//			return HelpText.AutoBuild(this,
//										current => HelpText.DefaultParsingErrorsHandler(this, current));
//		}
	}
}
