using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tailDotNet
{
	public class FileWatcher : IWatcher
	{
		private FileWatchConfiguration conf;
		private bool firstReadMade;

		public FileWatcher(FileWatchConfiguration fileWatchConfiguration)
		{
			conf = fileWatchConfiguration;
		}

		public bool Paused { get; private set; }

		public bool IsPaused
		{
			get { return Paused; }
		}

		public void Pause()
		{
			Paused = true;
		}

		public void Start()
		{
			Paused = false;

			using (var reader = new StreamReader(new FileStream(conf.FileName,
					 FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
			{
				//start at the end of the file
				long lastMaxOffset = firstReadMade ? reader.BaseStream.Length : 0;

				while (!Paused)
				{
					System.Threading.Thread.Sleep(conf.PollIntervalInMs);

					//if the file size has not changed, idle
					if (reader.BaseStream.Length == lastMaxOffset)
						continue;

					//seek to the last max offset
					reader.BaseStream.Seek(lastMaxOffset, SeekOrigin.Begin);

					//read out of the file until the EOF
					string line = "";
					while ((line = reader.ReadLine()) != null)
					{
						firstReadMade = true;
						conf.OutPut.WriteLine(line);
					}

					//update the last max offset
					lastMaxOffset = reader.BaseStream.Position;
				}
			}
		}
	}
}
