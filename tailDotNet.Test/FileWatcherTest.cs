using System;
using System.Collections.Generic;
using System.Text;
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

			fileWatcher.Pause();
			Assert.IsTrue(fileWatcher.IsPaused);
		}

		[TestMethod]
		public void Test()
		{
			var fileWatcher = new FakeFileWatcher();
			var observer = new FileWatchObserver();

			fileWatcher.Subscribe(observer);
			fileWatcher.Start();

			var expectedTailString = $"This is a string with a newline{Environment.NewLine}This is the next row";
			fileWatcher.AddTextToFile(expectedTailString);

			Assert.AreEqual(expectedTailString, observer.GetObservedStrings());
		}

		[TestMethod]
		public void Tail_should_not_be_considered_as_grown_when_exclusion_filter_matches_the_payload()
		{
			var target = new FileWatcher(new FakeStreamReader(), new FakeSleeper());
			var watchConf = new FileWatchConfiguration
			{
				WatchFilter = new WatchFilter { ExclusionFilter = new Filter { SimpleFilter = "splunk" } }
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
				WatchFilter = new WatchFilter { ExclusionFilter = new Filter { SimpleFilter = "hello" } }
			};

			var actual = target.TailHasGrownButShallObserversBeNotified(watchConf, "I'm a splunk!");
			Assert.IsTrue(actual);
		}

		[TestMethod]
		public void Inclusion_filter_should_be_case_insensetive()
		{
			var target = new FileWatcher(new FakeStreamReader(), new FakeSleeper());
			var watchConf = new FileWatchConfiguration
			{
				WatchFilter = new WatchFilter { InclusionFilter = new Filter { SimpleFilter = "hello" } }
			};

			var actual = target.TailHasGrownButShallObserversBeNotified(watchConf, "HELLO!");
			Assert.IsTrue(actual);
		}

		[TestMethod]
		public void Exclusion_filter_should_be_case_insensetive()
		{
			var target = new FileWatcher(new FakeStreamReader(), new FakeSleeper());
			var watchConf = new FileWatchConfiguration
			{
				WatchFilter = new WatchFilter { ExclusionFilter = new Filter { SimpleFilter = "hello" } }
			};

			var actual = target.TailHasGrownButShallObserversBeNotified(watchConf, "HELLO!");
			Assert.IsFalse(actual);
		}

		internal class FileWatchObserver : IObserver<TailPayload>
		{
			private StringBuilder StringBuilder { get; } = new StringBuilder();

			internal string GetObservedStrings()
			{
				return StringBuilder.ToString();
			}

			public void OnCompleted() { throw new NotImplementedException(); }

			public void OnError(Exception error) { throw error; }

			public void OnNext(TailPayload value)
			{
				if (value.TailEvent == FileEvent.TailGrown)
					StringBuilder.Append(value.TailString);
			}
		}

		private class FakeFileWatcher : IWatcher
		{

			public FakeFileWatcher()
			{
				_observers = new List<IObserver<TailPayload>>();
			}

			public IWatchConfiguration Configuration { get; set; }

		    public bool IsPaused { get; private set; }

		    public void Pause() { IsPaused = true; }
			public void Resume() { IsPaused = false; }

			public void Start() {}

			internal void AddTextToFile(string text)
			{
				var payload = new TailPayload { TailString = text, TailEvent = FileEvent.TailGrown };

				foreach (var observer in _observers)
				{
					observer.OnNext(payload);
				}
			}

			public void Dispose() { }

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