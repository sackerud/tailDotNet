using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tailDotNet.SignalR.Host
{
    class FileSystemManager
    {
        private readonly IFileSystemDao _filesystemDao;
        
        public FileSystemManager()
        {

        }

        public FileSystemManager(IFileSystemDao filesystemDao)
        {
            _filesystemDao = filesystemDao;
        }

        public IEnumerable<DriveInfo> GetVolumes()
        {
            return _filesystemDao.GetVolumes();
        }

        public IEnumerable<DirectoryInfo> GetDirectories(DriveInfo drive)
        {
            return _filesystemDao.GetDirectories(drive);
        }
    }
}
