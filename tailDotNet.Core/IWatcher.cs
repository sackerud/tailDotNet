using System;
using tailDotNet.Configuration;

namespace tailDotNet
{
	public interface IWatcher : IDisposable
	{
		IWatchConfiguration Configuration { get; set; }
		bool IsPaused { get; }
		void Pause();
		void Resume();
		void Start();
		IDisposable Subscribe(IObserver<TailPayload> observer);
	}
}