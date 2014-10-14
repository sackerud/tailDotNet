using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tailDotNet.Watchers
{
    public class ThreadSleeper : ISleeper
    {
        public void Sleep(int milliSecondsTimeout)
        {
            System.Threading.Thread.Sleep(milliSecondsTimeout);
        }
    }
}
