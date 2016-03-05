using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tailDotNet.Test
{
    [TestClass]
    public class CommandLineParserTest
    {
        [TestMethod]
        public void ParseInclusionFilter()
        {
            var options = new Console.TailOptions();
            CommandLine.Parser.Default.ParseArguments(new[] { "-i", "apa", "-e", "apa" }, options);

			Assert.AreEqual("apa", options.InclusionFilter);
			Assert.AreEqual("apa", options.ExclusionFilter);
        }

	    [TestMethod]
	    public void Inclusion_and_exclusion_filter_should_be_mutually_exclusive()
	    {
		    var options = new Console.TailOptions();
		    var args = new[]
		    {
			    "-e", "Exclude this",
			    "-i", "Include this"
		    };

			var actual = CommandLine.Parser.Default.ParseArgumentsStrict(args, options, OnFail);

			Assert.AreEqual(false, actual, "Command line arguments: {0}", string.Join(" ", args));
	    }

	    private void OnFail()
	    {
		    System.Console.WriteLine("Parsing arguments failed");
	    }
    }
}