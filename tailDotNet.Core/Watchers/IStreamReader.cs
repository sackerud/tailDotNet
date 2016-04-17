using System.IO;

namespace tailDotNet.Watchers
{
	public interface IStreamReader
	{
		string ReadLine();
		string ReadToEnd();
		long Length { get; }
		long Position { get; }
		Stream BaseStream { get; }
		void Dispose();
	}
}