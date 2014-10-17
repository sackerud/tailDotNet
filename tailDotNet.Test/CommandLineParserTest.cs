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
        public void ParseInclusionAndExclusionFilter()
        {
            var options = new tailDotNet.Console.TailOptions();
            bool actual = CommandLine.Parser.Default.ParseArguments(new string[] { "" }, options);

            Assert.IsFalse(actual);
        }
    }

}