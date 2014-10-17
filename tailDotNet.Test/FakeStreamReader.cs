using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tailDotNet.Watchers;

namespace tailDotNet.Test
{
    public class FakeStreamReader : IStreamReader
    {
        private long _length = 0;
        public long Length
        {
            get
            {
                Position = _length;
                _length++;
                return _length;
            }
        }
        public long Position { get; set; }

        public string ReadLine()
        {
            return string.Format("Length is now {0}", _length);
        }
    }
}
