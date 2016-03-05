namespace tailDotNet.Watchers
{
	public interface IStreamReader
	{
		string ReadLine();
		string ReadToEnd();
		long Length { get; }
		long Position { get; }
	}
}