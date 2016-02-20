using System;

namespace tailDotNet.Observers
{
	/// <summary>
	/// Simple observer that outputs changes to the console.
	/// </summary>
	public class ConsoleObserver : IObserver<TailPayload>
	{
		private bool _firstPayloadWrittenToConsole = false;

		public void OnNext(TailPayload value)
		{
			if (value.TailEvent != FileEvent.TailGrown) return;
			if (string.IsNullOrEmpty(value.TailString)) return;

			ChangeConsoleForegroundColorIfStringContainsWarningOrError(value.TailString);

			Console.Write(value.TailString);

			_firstPayloadWrittenToConsole = true;
		}

		public void OnError(Exception error)
		{
			throw error;
		}

		public void OnCompleted()
		{
			throw new NotImplementedException();
		}

		private void ChangeConsoleForegroundColorIfStringContainsWarningOrError(string s)
		{
			if (!_firstPayloadWrittenToConsole) return;

			var hasWarning = false;
			var hasError = false;

			if (s.ToLower().Contains("warn")) hasWarning = true;

			if (s.ToLower().Contains("error")) hasError = true;

			if (!hasWarning && !hasError)
			{
				Console.ResetColor();
				return;
			}

			if (hasWarning) Console.ForegroundColor = ConsoleColor.Yellow;
			if (hasError) Console.ForegroundColor = ConsoleColor.Red;
		}
	}
}