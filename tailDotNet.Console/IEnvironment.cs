using System;

namespace tailDotNet.Console
{
    public interface IEnvironment
    {
        void Exit(int exitCode);
    }

    public class RealEnvironment : IEnvironment
    {
        public void Exit(int exitCode)
        {
            Environment.Exit(exitCode);
        }
    }
}