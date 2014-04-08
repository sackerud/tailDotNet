using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tailDotNet
{
	public class FileWatchConfiguration : IWatchConfiguration
	{
		private int _pollIntervalInMs = 100;
		public int PollIntervalInMs
		{
			get { return _pollIntervalInMs; }
			set { _pollIntervalInMs = value; }
		}

		public string FileName { get; set; }
		public TextWriter OutPut { get; set; }
	}
}
