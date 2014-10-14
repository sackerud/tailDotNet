using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tailDotNet.Watchers;

namespace tailDotNet.Test
{
    public class FakeSleeper : ISleeper
    {
        public void Sleep(int ms) { }
    }
}
