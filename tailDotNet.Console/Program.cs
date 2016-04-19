using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLine;
using tailDotNet.Configuration;
using tailDotNet.Filtering;
using tailDotNet.Observers;
using tailDotNet.Watchers;

namespace tailDotNet.Console
{
	public class Program
	{
		static readonly TailOptions Options = new TailOptions();

		public static void Main(string[] args)
		{
			System.Console.CancelKeyPress += ConsoleOnCancelKeyPress;

			try
			{
				if (PopulateOptionsFromCommandArgs(args))
				{
					System.Console.WriteLine(new FileTailSpitter().GetLastLinesFromFile(TailOptionsToFileWatchConfiguration(Options)));

					if (Options.Follow) StartFileWatch(Options);
				}
			}
			finally
			{
				ResetColorInConsole();
			}
		}

		private static bool PopulateOptionsFromCommandArgs(string[] args)
		{
			var argsParsedSuccessfully = Parser.Default.ParseArguments(args, Options);

			if (!argsParsedSuccessfully) return false;

			if (!string.IsNullOrWhiteSpace(Options.ExclusionFilter)
			    && !string.IsNullOrWhiteSpace(Options.InclusionFilter))
			{
				throw new NotSupportedException("Exclusion filter and inclusion filter are mutually exclusive");
			}

			return true;
		}

		private static void StartFileWatch(TailOptions options)
		{
			if (options.Version) SpitVersionInfoAndExit();

			var conf = TailOptionsToFileWatchConfiguration(options);
            ISleeper sleeper = new ThreadSleeper();
            IStreamReader streamReader = new TailStreamReader(conf.FileName);
			TailWatcherProxy.StartWatcher(TailWatcherProxy.WatcherType.File, conf, streamReader, sleeper);
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
				FileName = options.Filename.First(),
				NumberOfLinesToOutputWhenWatchingStarts = options.Lines
			};

			if (!string.IsNullOrWhiteSpace(options.InclusionFilter))
			{
				conf.WatchFilter = new WatchFilter
				{
					InclusionFilter = new Filter
					{
						SimpleFilter = options.InclusionFilter
					}
				};
			}

			if (!string.IsNullOrWhiteSpace(options.ExclusionFilter))
			{
				conf.WatchFilter = new WatchFilter
				{
					ExclusionFilter = new Filter
					{
						SimpleFilter = options.ExclusionFilter
					}
				};
			}

			return conf;
		}

		private static FileExistsAssertionResult AssertFileExists(TailOptions options)
		{
			if (!options.Filename.Any()) return new FileExistsAssertionResult {AssertionFailedReason = "You must specify a file to tail"};

			var fileName = options.Filename.First();

			if (!File.Exists(fileName)) return new FileExistsAssertionResult
													{ AssertionFailedReason =
														$"{fileName} does not exist"
													};

			return new FileExistsAssertionResult { FileExists = true };
		}

		private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs consoleCancelEventArgs)
		{
			ResetColorInConsole();
		}

		private static void ResetColorInConsole()
		{
			System.Console.ResetColor();
		}
	}

	internal class FileExistsAssertionResult
	{
		public bool FileExists { get; set; }
		public string AssertionFailedReason { get; set; }
	}
}