﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using tailDotNet.Configuration;
using tailDotNet.Observers;
using tailDotNet.Watchers;

namespace tailDotNet.Console
{
	class Program : IObserver<TailPayload>
	{
		static readonly TailOptions Options = new TailOptions();

		static void Main(string[] args)
		{
			
			if (CommandLine.Parser.Default.ParseArguments(args, Options))
			{
				StartFileWatch(Options);
			}
		}

		private static void StartFileWatch(TailOptions options)
		{
			if (options.Version)
				SpitVersionInfoAndExit();

			var conf = TailOptionsToFileWatchConfiguration(options);
            ISleeper sleeper = new ThreadSleeper();
            IStreamReader streamReader = new TailStreamReader(conf.FileName);
			TailWatcherProxy.StartWatcher(TailWatcherProxy.WatcherType.File, conf, streamReader, sleeper);
			//var fw = new FileWatcher(conf, sleeper);
			//fw.Start();
		}

		private static void SpitVersionInfoAndExit()
		{
			var version = GetAssemblyVersion();
			System.Console.WriteLine(version.ToString());
			Environment.Exit(0);
		}

		private static Version GetAssemblyVersion()
		{
			return Assembly.GetExecutingAssembly().GetName().Version;
		}

		private static FileWatchConfiguration TailOptionsToFileWatchConfiguration(TailOptions options)
		{
			var assertionResult = AssertFileExists(options);
			if (!assertionResult.FileExists)
			{
				System.Console.WriteLine(assertionResult.AssertionFailedReason);
				Environment.Exit(0);
			}

			var conf = new FileWatchConfiguration
				{
					PollIntervalInMs = (int) options.SleepIntervalInSeconds*1000,
					Observer = new ConsoleObserver(),
					FileName = options.Filename.First()
				};

			return conf;
		}

		private static FileExistsAssertionResult AssertFileExists(TailOptions options)
		{
			if (!options.Filename.Any())
				return new FileExistsAssertionResult {AssertionFailedReason = "You must specify a file to tail"};

			var fileName = options.Filename.First();

			if (!File.Exists(fileName))
				return new FileExistsAssertionResult {AssertionFailedReason = string.Format("{0} does not exist", fileName)};

			return new FileExistsAssertionResult {FileExists = true};
		}

		public void OnNext(TailPayload value)
		{
			if(value.TailEvent == FileEvent.TailGrown)
				System.Console.Write(value.TailString);
		}

		public void OnError(Exception error)
		{
			throw error;
		}

		public void OnCompleted()
		{
			throw new NotImplementedException();
		}
	}

	internal class FileExistsAssertionResult
	{
		public bool FileExists { get; set; }
		public string AssertionFailedReason { get; set; }
	}
}
