using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace tailDotNet.SignalR.Host.Filesystem
{
    public class FileSystemManager
    {
        private readonly IFileSystemProvider _filesystemDao;
        
        public FileSystemManager()
        {
			_filesystemDao = new WindowsFsProvider();
        }

        public FileSystemManager(IFileSystemProvider filesystemDao)
        {
            _filesystemDao = filesystemDao;
        }

        public IEnumerable<FileSystemObject> GetVolumes()
        {
            var volumes = _filesystemDao.GetVolumes();

	        var fsObjects = from d in volumes
	                        select new FileSystemObject
		                        {
			                        Name = d.Name,
			                        Type = FsType.Drive
		                        };

	        return fsObjects;
        }

        public IEnumerable<FileSystemObject> GetDirectories(FileSystemObject drive)
        {
            var dirs =  _filesystemDao.GetDirectoryListing(drive);

	        var fsObjects = from d in dirs
	                        select new FileSystemObject
		                        {
									Fullname = d.Fullname,
			                        Name = d.Name,
			                        Type = FsType.Directory
		                        };
	        return fsObjects;
        }
    }
}
