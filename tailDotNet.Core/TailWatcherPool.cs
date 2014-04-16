using System;
using System.Collections.Generic;
using System.Linq;
using tailDotNet.Configuration;

namespace tailDotNet
{
	internal class TailWatcherPool
	{
		private IList<WathcherPoolObject> _watcherPool = new List<WathcherPoolObject>();
		private IList<WathcherPoolObject> WatcherPool
		{
			get { return _watcherPool; }
			set { _watcherPool = value; }
		}

		public Guid Add(IWatcher watcher)
		{
			var wpo = new WathcherPoolObject(watcher);
			WatcherPool.Add(wpo);
			return wpo.Id;
		}

		public int Count()
		{
			return WatcherPool.Count;
		}

		public IWatcher GetWatcherById(Guid id)
		{
			var watcher = WatcherPool.SingleOrDefault(w => w.Id == id);

			return watcher == null ? null : watcher.Watcher;
		}

		/// <summary>
		/// Returns a list of file watchers by filename.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public IList<FileWatcher> GetFileWatcherByFileName(string filename)
		{
			var watcherList = new List<FileWatcher>();

			foreach (var watcherPoolObject in WatcherPool)
			{
				if (!(watcherPoolObject.Watcher is FileWatcher)) continue;

				var fileWatcher = (FileWatcher)watcherPoolObject.Watcher;
				if (((FileWatchConfiguration)fileWatcher.Configuration).FileName == filename)
					watcherList.Add(fileWatcher);
			}

			return watcherList;
		}

		public void Remove(IWatcher watcher)
		{
			WatcherPool.Remove(new WathcherPoolObject(watcher));
		}

		public void ResumeAll()
		{
			foreach (var wathcherPoolObject in WatcherPool)
				wathcherPoolObject.Watcher.Resume();
		}

		public void SuspendAll()
		{
			foreach (var wathcherPoolObject in WatcherPool)
				wathcherPoolObject.Watcher.Pause();
		}

		/// <summary>
		/// Calls <see cref="IDisposable.Dispose"/> on all watcher in the pool.
		/// </summary>
		public void DisposeAll()
		{
			foreach (var wathcherPoolObject in WatcherPool)
				wathcherPoolObject.Watcher.Dispose();
		}
	}

	internal class WathcherPoolObject
	{
		public Guid Id { get; private set; }
		public IWatcher Watcher { get; private set; }

		public WathcherPoolObject(IWatcher watcher)
		{
			Watcher = watcher;
			Id = Guid.NewGuid();
		}
	}
}