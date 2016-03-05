using System.IO;

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
			return _streamReader.ReadLine();
			//            return string.Format("{0}{1}", _streamReader.ReadLine(), Environment.NewLine);
		}

		public string ReadToEnd()
		{
			return _streamReader.ReadToEnd();
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