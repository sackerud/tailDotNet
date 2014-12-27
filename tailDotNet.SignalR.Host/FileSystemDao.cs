using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tailDotNet.SignalR.Host
{
    class FileSystemDao : IFileSystemDao
    {
        public IEnumerable<DriveInfo> GetVolumes()
        {
            var drives = DriveInfo.GetDrives().Where(d => d.IsReady);
            return drives;
        }

        public IEnumerable<DirectoryInfo> GetDirectories(DriveInfo drive)
        {
            var dirs = drive.RootDirectory.EnumerateDirectories();
            return dirs;
        }
    }
}
