using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tailDotNet.Configuration.ResourceLocators
{
	public class FileResourceLocator : IResourceLocator
	{
		public string Filename { get; set; }
	}
}