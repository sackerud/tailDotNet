using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tailDotNet.Watchers
{
    public class TailStreamReader : IStreamReader
    {
        private readonly StreamReader _streamReader;

        public TailStreamReader(string filename)
        {
            _streamReader = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        }

        public string ReadLine()
        {
//            char[] buffer = new char[Length - Position];
//            return _streamReader.Read(buffer, Position, Length);
            return string.Format("{0}{1}", _streamReader.ReadLine(), Environment.NewLine);
        }

        public long Length
        {
            get { return _streamReader.BaseStream.Length; }
        }

        public long Position
        {
            get { return _streamReader.BaseStream.Position; }
        }
    }
}
