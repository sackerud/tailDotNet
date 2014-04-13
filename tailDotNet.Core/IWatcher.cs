namespace tailDotNet
{
	public interface IWatcher
	{
		WatchFilter Filter { get; set; }
		bool IsPaused{ get; }
		void Pause();
		void Resume();
		void Start();
	}
}
