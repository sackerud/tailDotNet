namespace tailDotNet.Watchers
{
	public class ThreadSleeper : ISleeper
	{
		public void Sleep(int milliSecondsTimeout)
		{
			System.Threading.Thread.Sleep(milliSecondsTimeout);
		}
	}
}