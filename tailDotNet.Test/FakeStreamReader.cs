using System;
using System.IO;
using tailDotNet.Watchers;

namespace tailDotNet.Test
{
    public class FakeStreamReader : IStreamReader
    {
        private long _length;
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
	    public Stream BaseStream { get; }
	    public void Dispose()
	    {
		    System.Console.WriteLine("FakeStreamReader was disposed");
	    }

	    public string ReadLine()
        {
            return $"Length is now {_length}";
        }
    }
}