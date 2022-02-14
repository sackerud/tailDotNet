using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using tailDotNet.Configuration;

namespace tailDotNet
{
	public class FileTailSpitter
	{
		private StreamReader _streamReader;

		public string GetLastLinesFromFile(FileWatchConfiguration conf)
		{
			var q = new Queue<string>(conf.NumberOfLinesToOutputWhenWatchingStarts + 1);
			_streamReader = new StreamReader(new FileStream(conf.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
			var lineEnumerator = new LineEnumerator(_streamReader);
			foreach (var line in lineEnumerator.Take(conf.NumberOfLinesToOutputWhenWatchingStarts))
				q.Enqueue(line);
			foreach (var line in lineEnumerator)
			{
				q.Dequeue();
				q.Enqueue(line);
			}
			return string.Join("\n", q);
		}

		internal class LineEnumerator : IEnumerable<string>
		{
			private readonly StreamReader _stream;
			internal LineEnumerator(StreamReader stream)
			{
				_stream = stream;
			}

			public IEnumerator<string> GetEnumerator()
			{
				while (!_stream.EndOfStream)
				{
					yield return _stream.ReadLine();
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				while (!_stream.EndOfStream)
				{
					yield return _stream.ReadLine();
				}
			}
		}
	}
}