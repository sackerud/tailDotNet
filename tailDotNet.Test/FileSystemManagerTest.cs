using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tailDotNet.SignalR.Host;
using tailDotNet.SignalR.Host.Filesystem;

namespace tailDotNet.Test
{
	[TestClass]
	public class FileSystemManagerTest
	{
		[TestMethod]
		public void GetVolumesTest()
		{
			var fsMan = new FileSystemManager();
			var actualVolumes = fsMan.GetVolumes();
			
			foreach (var vol in actualVolumes)
			{
				System.Console.WriteLine(string.Join(Environment.NewLine, actualVolumes.Select(v => v.Name)));

				var dirs = fsMan.GetDirectories(vol);
				foreach (var dir in dirs)
				{
					System.Console.WriteLine(dir.Name);
				}
			}
		}
	}
}