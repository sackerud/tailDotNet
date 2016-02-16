using System.IO;

namespace tailDotNet.Ftp
{
	public interface IFtpWatcher
	{
		FileInfo Get(string filename);
	}

	class FtpWatcher : IFtpWatcher
	{
		public FileInfo Get(string filename)
		{
			return new FileInfo(filename);
		}
	}
}
