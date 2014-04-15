using System;
using System.Collections.Generic;

namespace tailDotNet
{
	internal class TailWatcherPool
	{
		private IList<IWatcher> _watcherPool = new List<IWatcher>();
		private IList<IWatcher> WatcherPool
		{
			get { return _watcherPool; }
			set { _watcherPool = value; }
		}

		public void Add(IWatcher watcher)
		{
			WatcherPool.Add(watcher);
		}

		public int Count()
		{
			return WatcherPool.Count;
		}

		/// <summary>
		/// Returns a list of file watchers by filename.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public IList<FileWatcher> GetFileWatcherByFileName(string filename)
		{
			var watcherList = new List<FileWatcher>();

			foreach (var watcher in WatcherPool)
			{
				if (!(watcher is FileWatcher)) continue;
				
				var fileWatcher = (FileWatcher) watcher;
				if (((FileWatchConfiguration)fileWatcher.Configuration).FileName == filename)
					watcherList.Add(fileWatcher);
			}

			return watcherList;
		}

		public void Remove(IWatcher watcher)
		{
			WatcherPool.Remove(watcher);
		}

		public void ResumeAll()
		{
			foreach (var watcher in WatcherPool)
				watcher.Resume();
		}

		public void SuspendAll()
		{
			foreach (var watcher in WatcherPool)
				watcher.Pause();
		}

		/// <summary>
		/// Calls <see cref="IDisposable.Dispose"/> on all watcher in the pool.
		/// </summary>
		public void TerminateAll()
		{
			foreach (var watcher in WatcherPool)
				watcher.Dispose();
		}
	}
}