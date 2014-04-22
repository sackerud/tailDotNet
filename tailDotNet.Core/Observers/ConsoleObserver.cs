using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tailDotNet.Observers
{
	public class ConsoleObserver : IObserver<TailPayload>
	{
		public void OnNext(TailPayload value)
		{
			if (value.TailEvent == FileEvent.TailGrown)
				Console.WriteLine(value.TailString);
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
