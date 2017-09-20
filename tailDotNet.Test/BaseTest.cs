using Microsoft.VisualStudio.TestTools.UnitTesting;
using tailDotNet.Console;

namespace tailDotNet.Test
{
    [TestClass]
    public class BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Program.Options = new TailOptions();
        }
    }
}