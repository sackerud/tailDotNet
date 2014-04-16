using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using tailDotNet.Configuration;

namespace tailDotNet.Test
{
	[TestFixture]
    public class FileWatcherTest
    {
		[Test]
		public void FileWatchTest()
		{
			var fileToWatch = GetTempFileWithContents();
			var conf = new FileWatchConfiguration
				{
					OutPut = Console.Out,
					FileName = fileToWatch,
				};
			var fileWatcher = new FileWatcher(conf);
			Console.WriteLine("About to start watching file {0}", fileToWatch);
			fileWatcher.Start();
			var expected = "Hello world!";
			fileWatcher.Pause();
			Console.WriteLine("Is paused: {0}", fileWatcher.IsPaused);
			//Assert.AreEqual(expected, actual.First());
		}

		private string GetTempFileWithContents()
		{
			var tempFileName = Path.GetTempFileName();
			File.WriteAllText(tempFileName, "Hello world!");
			return tempFileName;
		}
    }
}