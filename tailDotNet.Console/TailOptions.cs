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
        [Option('e', "excludefilter", HelpText = "exclude the specified string from the output", MutuallyExclusiveSet = "filtering")]
        public string ExclusionFilter {get;set;}

		[Option('f', "follow", DefaultValue = false, HelpText = "output appended data as the file grows")]
		public bool Follow { get; set; }

        [Option('i', "includefilter", HelpText = "only include the specified string in the output", MutuallyExclusiveSet = "filtering")]
        public string InclusionFilter { get; set; }

		[Option('n', "lines", DefaultValue = 10, HelpText = "output the last N lines, instead of the last 10")]
		public int Lines { get; set; }

		[Option('s', "sleep-interval", DefaultValue = 1.0d, HelpText = "with -f, sleep for approximately s seconds (default 1.0) between iterations")]
		public double SleepIntervalInSeconds { get; set; }

		[Option('~', "version", DefaultValue = false, HelpText = "output version information and exit")]
		public bool Version { get; set; }

		[ValueList(typeof(List<string>), MaximumElements = 1)]
		public IList<string> Filename { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this,
										current => HelpText.DefaultParsingErrorsHandler(this, current));
		}
	}
}
