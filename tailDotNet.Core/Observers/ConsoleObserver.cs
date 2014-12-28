using System;

namespace tailDotNet.Observers
{
	/// <summary>
	/// Simple observer that outputs changes to the console
	/// </summary>
	public class ConsoleObserver : IObserver<TailPayload>
	{
		public void OnNext(TailPayload value)
		{
			if (value.TailEvent == FileEvent.TailGrown)
				Console.Write(value.TailString);
		}

		public void OnError(Exception error)
		{
			throw error;
		}

		public void OnCompleted()
		{
			throw new NotImplementedException();
		}
	}
}
