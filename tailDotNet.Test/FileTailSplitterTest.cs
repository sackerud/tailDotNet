using System;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using tailDotNet.Configuration;

namespace tailDotNet.Test
{
	[TestClass]
	public class FileTailSplitterTest
	{
		[TestInitialize]
		public void Init()
		{
			try
			{
				File.OpenWrite("test.txt").Close();
			}
			catch
			{
				Environment.CurrentDirectory = Path.GetTempPath();
			}
			File.WriteAllText("shorttest.txt", "one\ntwo\nthree");
			File.WriteAllText("longtest.txt", "one\ntwo\nthree\nfour\nfive\nsix\nseven");
		}

		[TestMethod]
		public void Should_Handle_Fewer_Lines_Than_Conf_NumberOfLines()
		{
			//Arrange
			var CUT = new FileTailSpitter();
			var conf = new FileWatchConfiguration
			{
				FileName = "shorttest.txt",
				NumberOfLinesToOutputWhenWatchingStarts = 10
			};

			//Act
			Assert.AreEqual(3, CUT.GetLastLinesFromFile(conf).Split('\n').Count());
		}

		[TestMethod]
		public void Should_Only_Output_Conf_NumberOfLines_From_Longer_File()
		{
			//Arrange
			var CUT = new FileTailSpitter();
			var conf = new FileWatchConfiguration
			{
				FileName = "longtest.txt",
				NumberOfLinesToOutputWhenWatchingStarts = 3
			};

			//Act
			CollectionAssert.AreEquivalent(new[] { "five", "six", "seven" }, CUT.GetLastLinesFromFile(conf).Split('\n'));
		}

		[TestMethod]
		public void Should_Open_Non_Exclusive()
		{
			//Arrange
			const string filename = "writetest.txt";
			using (var testfile = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
			{
				StreamWriter writer = new StreamWriter(testfile);
				writer.WriteLine("one\ntwo\nthree\nfour\nfive\nsix");
				writer.Flush();
				testfile.Flush(true);

				var CUT = new FileTailSpitter();
				var conf = new FileWatchConfiguration
				{
					FileName = filename,
					NumberOfLinesToOutputWhenWatchingStarts = 3
				};

				//Act
				string[] actual = CUT.GetLastLinesFromFile(conf).Split('\n');

				// Assert - also that it does not throw
				CollectionAssert.AreEquivalent(new[] { "four", "five", "six" }, actual);
			}
		}
	}
}
