using System;

namespace tailDotNet
{
	public class TailPayload : IDisposable
	{
		public string TailString { get; set; }
		/// <summary>
		/// The type of event that has occured. Usually it's <see cref="FileEvent.TailGrown"/>.
		/// </summary>
		public FileEvent TailEvent { get; set; }

		public void Dispose() {}
	}
}