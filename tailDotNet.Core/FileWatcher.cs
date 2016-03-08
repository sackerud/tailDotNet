using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using tailDotNet.Configuration;
using tailDotNet.Filtering;
using tailDotNet.Watchers;

namespace tailDotNet
{
	public class FileWatcher : IWatcher, IObservable<TailPayload>
	{
		private readonly IStreamReader _streamReader;
		private readonly ISleeper _sleeper;
		private IWatchConfiguration _conf;
		public IWatchConfiguration Configuration
		{
			get { return _conf; }
			set { _conf = value; }
		}

		private long _lastMaxOffset;

		public FileWatcher(FileWatchConfiguration fileWatchConfiguration, IStreamReader streamReader, ISleeper sleeper)
		{
			if (streamReader == null) throw new ArgumentNullException(nameof(streamReader));
			if (sleeper == null) throw new ArgumentNullException(nameof(sleeper));
			if (fileWatchConfiguration == null) throw new ArgumentNullException(nameof(fileWatchConfiguration));

			_conf = fileWatchConfiguration;
			_streamReader = streamReader;
			_sleeper = sleeper;
			_observers = new List<IObserver<TailPayload>>();
		}

		/// <summary>
		/// Internal constructor to enable adding the configuration after the instance has been created
		/// </summary>
		internal FileWatcher(IStreamReader streamReader, ISleeper sleeper)
		{
			if (streamReader == null) throw new ArgumentNullException(nameof(streamReader));
			if (sleeper == null) throw new ArgumentNullException(nameof(sleeper));

			_sleeper = sleeper;
			_streamReader = streamReader;
			_observers = new List<IObserver<TailPayload>>();
		}

		public WatchFilter Filter { get; set; }
		public bool Paused { get; private set; }
		public bool IsPaused { get { return Paused; } }
		public void Pause() { Paused = true; }
		public void Resume() { Paused = false; }

		/// <summary>
		/// Starts tailing a file. If changes are detected in the tailed file, the
		/// <see cref="IObserver{T}.OnNext"/> will be called. The subscription to this
		/// observable class will be started an observer exists in the <see cref="FileWatchConfiguration"/>
		/// sent to this class. 
		/// </summary>
		public void Start()
		{
			InternalStart();
		}

		public void StartAsync()
		{
			InternalStart();
		}

		private void InternalStart()
		{
			Paused = false;
			StartSubscriptionIfObserverExists(_conf);

			while (!Paused)
			{
				var tailString = GetTail();

				if (tailString != string.Empty)
					NotifyObserversThatTailHasGrown(tailString);

				_sleeper.Sleep(_conf.PollIntervalInMs);
			}
		}

		private void NotifyObserversThatTailHasGrown(string tailString)
		{
			if (TailHasGrownButShallObserversBeNotified(_conf, tailString) == false) return;

			var payload = new TailPayload { TailString = tailString, TailEvent = FileEvent.TailGrown };

			foreach (var observer in _observers)
			{
				observer.OnNext(payload);
			}
		}

		internal bool TailHasGrownButShallObserversBeNotified(IWatchConfiguration conf, string tailString)
		{
			if (conf.HasWatchFilter == false) return true;
			
			if (conf.WatchFilter.InclusionFilter != null)
				return tailString.ToLower().Contains(conf.WatchFilter.InclusionFilter.SimpleFilter.ToLower());

			if (conf.WatchFilter.ExclusionFilter != null)
				return !tailString.ToLower().Contains(conf.WatchFilter.ExclusionFilter.SimpleFilter.ToLower());

			return true;
		}

		private string GetTail()
		{
			//if the file size has not changed, idle
			if (_streamReader.Length == _lastMaxOffset) return string.Empty;

			var stringBuffer = new StringBuilder();

			//seek to the last max offset
			FileReader.BaseStream.Seek(_lastMaxOffset, SeekOrigin.Begin);

			//read out of the file until the EOF
			stringBuffer.Append(_streamReader.ReadToEnd());

			//update the last max offset
			_lastMaxOffset = _streamReader.Position;

			return stringBuffer.ToString();
		}

		private StreamReader _reader;
		private StreamReader FileReader
		{
			get
			{
				if (_reader == null)
				{
					_reader = new StreamReader(new FileStream(((FileWatchConfiguration)_conf).FileName,
														  FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
				}

				return _reader;
			}
		}

		/// <summary>
		/// If an observer exists in the configuration sent to this class, its
		/// <see cref="IObservable{T}.Subscribe"/> method be calles.
		/// </summary>
		private void StartSubscriptionIfObserverExists(IWatchConfiguration conf)
		{
			if (conf?.Observer == null) return;

			Subscribe(conf.Observer);
		}

		/// <summary>
		/// Disposes the internal <see cref="StreamReader"/> and removes all observers.
		/// </summary>
		public void Dispose()
		{
			Pause();

			if (_reader == null) return;

			_reader.Dispose();
			_observers.Clear();
		}

		private readonly List<IObserver<TailPayload>> _observers;
		public IDisposable Subscribe(IObserver<TailPayload> observer)
		{
			if (!_observers.Contains(observer))
				_observers.Add(observer);
			return new TailPayload();
		}
	}
}