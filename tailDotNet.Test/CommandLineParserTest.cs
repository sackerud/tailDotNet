using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tailDotNet.Test
{
    [TestClass]
    public class CommandLineParserTest
    {
        [TestMethod]
        public void ParseInclusionFilter()
        {
            var options = new tailDotNet.Console.TailOptions();
            bool actual = CommandLine.Parser.Default.ParseArguments(new string[] { "-i", "apa" }, options);

			Assert.AreEqual("apa", options.InclusionFilter);
        }
    }
}