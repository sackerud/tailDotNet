using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tailDotNet.Watchers
{
    public interface ISleeper
    {
        void Sleep(int milliSecondsTimeout);
    }
}
