using tailDotNet.Console;

namespace tailDotNet.Test
{
    public class FakeEnvironment : IEnvironment
    {
        public int LastExitCode { get; private set; }

        public void Exit(int exitCode)
        {
            LastExitCode = exitCode;
        }
    }
}