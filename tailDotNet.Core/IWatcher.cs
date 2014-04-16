using System;
using tailDotNet.Configuration;

namespace tailDotNet
{
	public interface IWatcher : IDisposable
	{
		WatchFilter Filter { get; set; }
		IWatchConfiguration Configuration { get; set; }
		bool IsPaused{ get; }
		void Pause();
		void Resume();
		void Start();
	}
}
