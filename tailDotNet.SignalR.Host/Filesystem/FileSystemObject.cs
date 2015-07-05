namespace tailDotNet.SignalR.Host.Filesystem
{
	public class FileSystemObject
	{
		public bool IsDirectory
		{
			get { return Type == FsType.Directory; }
		}

		public bool IsDrive
		{
			get { return Type == FsType.Drive; }
		}

		public bool IsFile
		{
			get { return Type == FsType.File; }
		}

		public string Fullname { get; set; }
		
		public string Name { get; set; }
		
		public FsType Type { get; set; }
	}

	public enum FsType
	{
		Directory = 0,
		Drive = 1,
		File = 2,
	}
}