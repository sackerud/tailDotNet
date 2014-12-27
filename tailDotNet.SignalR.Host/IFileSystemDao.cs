using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tailDotNet.SignalR.Host
{
    interface IFileSystemDao
    {
        IEnumerable<System.IO.DirectoryInfo> GetDirectories(System.IO.DriveInfo drive);
        IEnumerable<System.IO.DriveInfo> GetVolumes();
    }
}
