using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tailDotNet.Test
{
	[TestClass]
	public class CommandLineParserTest
	{
		[TestMethod]
		public void ParseInclusionFilterFromArguments()
		{
			var options = new Console.TailOptions();
            //CommandLine.Parser.Default.ParseArguments(new[] { "-i", "boing" }, options);
		    CommandLine.Parser.Default.ParseArguments(new[] { "-i", "boing" });

            Assert.AreEqual("boing", options.InclusionFilter);
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void Inclusion_and_exclusion_filter_should_be_mutually_exclusive()
		{
			var args = new[]
			{
				"-e", "Exclude this",
				"-i", "Include this"
			};

			Console.Program.Main(args);
		}
	}
}