using System;
using System.Collections.Generic;
using System.IO;
using tailDotNet.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tailDotNet.Filtering;

namespace tailDotNet.Test
{
	[TestClass]
    public class FileWatcherTest
    {
		[TestMethod]
		public void IsPaused_should_return_after_calling_pause()
		{
			IWatcher fileWatcher = new FakeFileWatcher();

            var observer = new FileWatchObserver();
            fileWatcher.Subscribe(observer);
			fileWatcher.Start();
			//var expected = "Hello world!";
            fileWatcher.Pause();
			Assert.IsTrue(fileWatcher.IsPaused);
		}

		[TestMethod]
		public void Tail_should_not_be_considered_as_grown_when_exclusion_filter_matches_the_payload()
		{
			var target = new FileWatcher(new FakeStreamReader(), new FakeSleeper());
			var watchConf = new FileWatchConfiguration
			{
				WatchFilter = new WatchFilter() { ExclusionFilter = new Filter() { SimpleFilter = "splunk"} }
			};

			var actual = target.TailHasGrownButShallObserversBeNotified(watchConf, "I'm a splunk!");
			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void Tail_should_be_considered_as_grown_when_exclusion_filter_does_not_match_the_payload()
		{
			var target = new FileWatcher(new FakeStreamReader(), new FakeSleeper());
			var watchConf = new FileWatchConfiguration
			{
				WatchFilter = new WatchFilter() { ExclusionFilter = new Filter() { SimpleFilter = "hello" } }
			};

			var actual = target.TailHasGrownButShallObserversBeNotified(watchConf, "I'm a splunk!");
			Assert.IsTrue(actual);
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