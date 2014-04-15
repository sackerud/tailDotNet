using System;

namespace tailDotNet
{
	public class TailWatcherProxy
	{
		private static TailWatcherPool _tailWatcherPool = new TailWatcherPool();

		public enum WatcherType
		{
			File
		}

		public static void StartWatcher(WatcherType watcherType, FileWatchConfiguration fileWatchConfiguration)
		{
			var watcher = CreateWatcherInternal(watcherType);
			watcher.Configuration = fileWatchConfiguration;
			watcher.Start();
			AddWatcherToPool(watcher);
		}

		public static void StartWatcher(WatcherType watcherType, string fileName)
		{
			var watcher = CreateWatcherInternal(watcherType);
			watcher.Configuration = GetDefaultFileWatcherConfiguration(fileName);
			watcher.Start();
			AddWatcherToPool(watcher);
		}

		public static int GetWatchCount()
		{
			return _tailWatcherPool.Count();
		}

		public static void ResumeAll()
		{
			_tailWatcherPool.ResumeAll();
		}

		public static void SuspendAll()
		{
			_tailWatcherPool.SuspendAll();
		}

		private static IWatcher CreateWatcherInternal(WatcherType watcherType)
		{
			switch (watcherType)
			{
				case WatcherType.File:
					return new FileWatcher();
				default:
					throw new NotImplementedException(string.Format("Factory method for {0} not implemented", watcherType.ToString()));
			}
		}

		private static FileWatchConfiguration GetDefaultFileWatcherConfiguration(string fileName)
		{
			return new FileWatchConfiguration
			{
				PollIntervalInMs = 1000,
				OutPut = Console.Out,
				FileName = fileName
			};
		}

		private static void AddWatcherToPool(IWatcher watcher)
		{
			_tailWatcherPool.Add(watcher);
		}

		private static void RemoveWatcherFromPool(IWatcher watcher)
		{
			_tailWatcherPool.Remove(watcher);
		}
	}
}