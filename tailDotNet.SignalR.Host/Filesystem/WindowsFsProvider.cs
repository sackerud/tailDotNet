using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace tailDotNet.SignalR.Host.Filesystem
{
    class WindowsFsProvider : IFileSystemProvider
    {
		public IEnumerable<FileSystemObject> GetVolumes()
        {
            var drives = DriveInfo.GetDrives().Where(d => d.IsReady);

			var fsObjects = from d in drives
			                select new FileSystemObject
				                {
					                Fullname = d.VolumeLabel,
					                Name = d.Name,
					                Type = FsType.Drive
				                };
            return fsObjects;
        }

		public IEnumerable<FileSystemObject> GetDirectoryListing(FileSystemObject fso)
        {
			if (fso == null) throw new ArgumentNullException("fso");
			if (fso.Type == FsType.File) throw new ArgumentException("Cannot get directory listing from a file");

			var driveOrDirName = GetFullname(fso);

			if (string.IsNullOrWhiteSpace(driveOrDirName)) throw new ArgumentException("Cannot get directory listing from an empty path");

			// First, get the directories
			var dirs = from d in Directory.GetDirectories(driveOrDirName)
						select new FileSystemObject
							{
								Fullname = d,
								Name = Path.GetFileName(d),
								Type = FsType.Directory
							};

			// Then get the files
			dirs = dirs.Concat(from f in Directory.GetFiles(driveOrDirName)
								select new FileSystemObject
									{
										Fullname = f,
										Name = Path.GetFileName(f),
										Type = FsType.File
									});

            return dirs;
        }

		private string GetFullname(FileSystemObject fso)
		{
			switch (fso.Type)
			{
				case FsType.Directory:
					return fso.Fullname;
				case FsType.Drive:
					return fso.Name;
				default:
					throw new NotImplementedException(string.Format("Getting directory listing for FileSystemObject type {0} is not implemented", fso.Type));
			}
		}
    }
}
