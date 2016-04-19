using System;
using tailDotNet.Configuration;
using tailDotNet.Watchers;

namespace tailDotNet
{
	public class TailWatcherProxy
	{
		private static readonly TailWatcherPool TailWatcherPool = new TailWatcherPool();

		public enum WatcherType
		{
			File
		}

		public static void StartWatcher(WatcherType watcherType, FileWatchConfiguration fileWatchConfiguration,
                                        IStreamReader streamReader, ISleeper sleeper)
		{
			var watcher = CreateWatcherInternal(watcherType, streamReader, sleeper);
			watcher.Configuration = fileWatchConfiguration;
			
			AddWatcherToPool(watcher);
			watcher.Start();
		}

		public static void StartWatcher(WatcherType watcherType, string fileName, IStreamReader streamReader, ISleeper sleeper)
		{
			var watcher = CreateWatcherInternal(watcherType, streamReader, sleeper);
			watcher.Configuration = GetDefaultFileWatcherConfiguration(fileName);
			AddWatcherToPool(watcher);
			watcher.Start();
		}

		public static int GetWatchCount()
		{
			return TailWatcherPool.Count();
		}

		public static void DisposeAll()
		{
			TailWatcherPool.DisposeAll();
		}

		public static void ResumeAll()
		{
			TailWatcherPool.ResumeAll();
		}

		public static void SuspendAll()
		{
			TailWatcherPool.SuspendAll();
		}

		private static IWatcher CreateWatcherInternal(WatcherType watcherType, IStreamReader streamReader, ISleeper sleeper)
		{
			switch (watcherType)
			{
				case WatcherType.File:
					return new FileWatcher(streamReader, sleeper);
				default:
					throw new NotImplementedException($"Factory method for {watcherType} not implemented");
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
			TailWatcherPool.Add(watcher);
		}
	}
}