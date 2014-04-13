using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tailDotNet.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var options = new TailOptions();
			if (CommandLine.Parser.Default.ParseArguments(args, options))
			{
				
			}

			var input = System.Console.ReadLine();
		}
	}
}
