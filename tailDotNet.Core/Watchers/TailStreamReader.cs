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

		public Stream BaseStream => _streamReader.BaseStream;

		public void Dispose()
		{
			_streamReader.Dispose();
		}

		public string ReadLine()
		{
			return _streamReader.ReadLine();
		}

		public string ReadToEnd()
		{
			return _streamReader.ReadToEnd();
		}

		public long Length => _streamReader.BaseStream.Length;

		public long Position => _streamReader.BaseStream.Position;
	}
}