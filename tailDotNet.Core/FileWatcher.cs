using System.IO;
using System.Text;

namespace tailDotNet
{
	public class FileWatcher : IWatcher
	{
		private IWatchConfiguration _conf;
		public IWatchConfiguration Configuration
		{
			get { return _conf; }
			set { _conf = value; }
		}

		private long _lastMaxOffset;

		public FileWatcher(FileWatchConfiguration fileWatchConfiguration)
		{
			_conf = fileWatchConfiguration;
		}

		/// <summary>
		/// Internal constructor to enable adding the configuration after the instance has been created
		/// </summary>
		internal FileWatcher() {}

		public WatchFilter Filter { get; set; }
		public bool Paused { get; private set; }
		public bool IsPaused { get { return Paused; } }
		public void Pause() { Paused = true; }
		public void Resume() { Paused = false; }
		
		public void Start()
		{
			Paused = false;

			while (!Paused)
			{
				var tailString = GetTail();

				if (tailString != string.Empty)
					_conf.OutPut.WriteLine(tailString);

				System.Threading.Thread.Sleep(_conf.PollIntervalInMs);
			}
		}

		private string GetTail()
		{
			//if the file size has not changed, idle
			if (FileReader.BaseStream.Length == _lastMaxOffset)
			{
				return string.Empty;
			}

			var stringBuffer = new StringBuilder();

			//seek to the last max offset
			FileReader.BaseStream.Seek(_lastMaxOffset, SeekOrigin.Begin);

			//read out of the file until the EOF
			string line;
			while ((line = FileReader.ReadLine()) != null)
			{
				stringBuffer.AppendLine(line);
			}

			//update the last max offset
			_lastMaxOffset = FileReader.BaseStream.Position;

			return stringBuffer.ToString();
		}

		private StreamReader _reader;
		private StreamReader FileReader
		{
			get
			{
				if (_reader == null)
				{
					_reader = new StreamReader(new FileStream(((FileWatchConfiguration) _conf).FileName,
														  FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
				}

				return _reader;
			}
		}

		public void Dispose()
		{
			Pause();

			if (_reader != null)
			{
				_reader.Dispose();
			}
		}
	}
}
