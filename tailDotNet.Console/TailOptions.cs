using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace tailDotNet.Console
{
	public class TailOptions
	{
		[Option('f', "follow", DefaultValue = false)]
		public bool Follow { get; set; }

		[Option('n', "lines", DefaultValue = 10)]
		public int Lines { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this,
			  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
		}
	}
}
