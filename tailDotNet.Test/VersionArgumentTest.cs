using Microsoft.VisualStudio.TestTools.UnitTesting;
using tailDotNet.Console;

namespace tailDotNet.Test
{
    [TestClass]
    public class VersionArgumentTest
    {
        [TestMethod]
        public void Sending_version_argument_should_exit_cleanly_with_exitCode_0()
        {
            // Arrange
            var fakeEnvironment = new FakeEnvironment();
            Program.CurrentEnvironment = fakeEnvironment;

            // Act
            Program.Main(new[] { "--version" });

            // Assert
            Assert.AreEqual(0, fakeEnvironment.LastExitCode);
        }
    }
}