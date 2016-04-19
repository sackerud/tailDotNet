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
			_streamReader = new StreamReader(new FileStream(conf.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
			SetStreamPosition(conf);
			return _streamReader.ReadToEnd();
		}

		private void SetStreamPosition(FileWatchConfiguration conf)
		{
			if (conf.NumberOfLinesToOutputWhenWatchingStarts < 1) return;

			var lineNumberToStartTailingFrom = File.ReadLines(conf.FileName).Count() -
											   conf.NumberOfLinesToOutputWhenWatchingStarts;

			if (lineNumberToStartTailingFrom <= 0) return;

			var index = 1;

			while (_streamReader.ReadLine() != null)
			{
				if (lineNumberToStartTailingFrom == index) break;

				index++;
			}
		}
	}
}