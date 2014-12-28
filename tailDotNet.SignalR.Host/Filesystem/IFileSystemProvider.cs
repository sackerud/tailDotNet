using System.Collections.Generic;

namespace tailDotNet.SignalR.Host.Filesystem
{
    public interface IFileSystemProvider
    {
		IEnumerable<FileSystemObject> GetDirectoryListing(FileSystemObject drive);
        IEnumerable<FileSystemObject> GetVolumes();
    }
}