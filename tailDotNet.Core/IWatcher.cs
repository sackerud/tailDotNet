using System;
using tailDotNet.Configuration;
using tailDotNet.Filtering;

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
        IDisposable Subscribe(IObserver<TailPayload> observer);
	}
}
