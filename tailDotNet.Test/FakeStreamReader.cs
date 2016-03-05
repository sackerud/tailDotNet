using System;
using tailDotNet.Watchers;

namespace tailDotNet.Test
{
    public class FakeStreamReader : IStreamReader
    {
        private long _length = 0;
	    public string ReadToEnd()
	    {
		    throw new NotImplementedException();
	    }

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
            return $"Length is now {_length}";
        }
    }
}