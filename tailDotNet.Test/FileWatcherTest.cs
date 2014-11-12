using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tailDotNet.Configuration;
using tailDotNet.Observers;
using tailDotNet.Watchers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tailDotNet.Test
{
	[TestClass]
    public class FileWatcherTest
    {
		[TestMethod]
		public void FileWatchTest()
		{
			IWatcher fileWatcher = new FakeFileWatcher();

            var observer = new FileWatchObserver();
            fileWatcher.Subscribe(observer);
			fileWatcher.Start();
			//var expected = "Hello world!";
            fileWatcher.Pause();
			System.Console.WriteLine("Is paused: {0}", fileWatcher.IsPaused);
            fileWatcher.Dispose();
            
			//Assert.AreEqual(expected, actual.First());
		}

		private string GetTempFileWithContents()
		{
			var tempFileName = Path.GetTempFileName();
			File.WriteAllText(tempFileName, "Hello world!");
			return tempFileName;
		}

        private class FileWatchObserver : IObserver<TailPayload>
        {
            public void OnCompleted()
            {
                throw new NotImplementedException();
            }

            public void OnError(Exception error)
            {
                throw error;
            }

            public void OnNext(TailPayload value)
            {
                if (value.TailEvent == FileEvent.TailGrown)
                    System.Console.WriteLine(value.TailString);
            }
        }

        private class FakeFileWatcher : IWatcher
        {

            public FakeFileWatcher()
            {
                _observers = new List<IObserver<TailPayload>>();
            }

            public WatchFilter Filter
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public IWatchConfiguration Configuration { get; set;}

            private bool _isPaused;
            public bool IsPaused
            {
                get { return _isPaused; }
            }

            public void Pause() { _isPaused = true; }
            public void Resume() { _isPaused = false; }

            public void Start()
            {
                var tailString = string.Format("This is a string with a newline{0}This is the next row", Environment.NewLine);
                var payload = new TailPayload { TailString = tailString, TailEvent = FileEvent.TailGrown };

                foreach (var observer in _observers)
                {
                    observer.OnNext(payload);
                }
            }

            public void Dispose() {}

            private List<IObserver<TailPayload>> _observers;
            public IDisposable Subscribe(IObserver<TailPayload> observer)
            {
                if (!_observers.Contains(observer))
                    _observers.Add(observer);
                return new TailPayload();
            }
        }
    }
}