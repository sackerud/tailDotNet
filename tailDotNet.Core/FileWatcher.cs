using System.IO;

namespace tailDotNet
{
	public class FileWatcher : IWatcher
	{
		private readonly FileWatchConfiguration _conf;
		private bool _firstReadMade;

		public FileWatcher(FileWatchConfiguration fileWatchConfiguration)
		{
			_conf = fileWatchConfiguration;
		}

		public WatchFilter Filter { get; set; }
		
		public bool Paused { get; private set; }

		public bool IsPaused { get { return Paused; } }

		public void Pause() { Paused = true; }

		public void Resume() { Paused = false; }

		public void Start()
		{
			Paused = false;

			using (var reader = new StreamReader(new FileStream(_conf.FileName,
					 FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
			{
				//start at the end of the file
				long lastMaxOffset = _firstReadMade ? reader.BaseStream.Length : 0;

				while (!Paused)
				{
					System.Threading.Thread.Sleep(_conf.PollIntervalInMs);

					//if the file size has not changed, idle
					if (reader.BaseStream.Length == lastMaxOffset)
						continue;

					//seek to the last max offset
					reader.BaseStream.Seek(lastMaxOffset, SeekOrigin.Begin);

					//read out of the file until the EOF
					string line;
					while ((line = reader.ReadLine()) != null)
					{
						_firstReadMade = true;
						_conf.OutPut.WriteLine(line);
					}

					//update the last max offset
					lastMaxOffset = reader.BaseStream.Position;
				}
			}
		}
	}
}
