using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tailDotNet
{
	public interface IWatcher
	{
		bool IsPaused{ get; }
		void Pause();
		void Start();
	}
}
