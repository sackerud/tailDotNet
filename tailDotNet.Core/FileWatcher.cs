using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using tailDotNet.Configuration;
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
            if (streamReader == null) throw new ArgumentNullException("streamReader");
            if (sleeper == null) throw new ArgumentNullException("sleeper");
            if (fileWatchConfiguration == null) throw new ArgumentNullException("fileWatchConfiguration");

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
            if (streamReader == null) throw new ArgumentNullException("streamReader");
            if (sleeper == null) throw new ArgumentNullException("sleeper");

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
			var payload = new TailPayload {TailString = tailString, TailEvent = FileEvent.TailGrown};

			foreach (var observer in _observers)
			{
				observer.OnNext(payload);
			}
		}

		private string GetTail()
		{
			//if the file size has not changed, idle
			if (_streamReader.Length == _lastMaxOffset)
			{
				return string.Empty;
			}

			var stringBuffer = new StringBuilder();

			//seek to the last max offset
			FileReader.BaseStream.Seek(_lastMaxOffset, SeekOrigin.Begin);

			//read out of the file until the EOF
			string line;
			while ((line = _streamReader.ReadLine()) != null)
			{
				stringBuffer.AppendLine(line);
			}

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
					_reader = new StreamReader(new FileStream(((FileWatchConfiguration) _conf).FileName,
														  FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
				}

				return _reader;
			}
		}

		/// <summary>
		/// If an observer exists in the configuration sent to this class, its
		/// <see cref="IObservable{T}.Subscribe"/> method be calles.
		/// </summary>
		/// <param name="conf"></param>
		private void StartSubscriptionIfObserverExists(IWatchConfiguration conf)
		{
			if (conf == null) return;
			if (conf.Observer == null) return;

			this.Subscribe(conf.Observer);
		}

		/// <summary>
		/// Disposes the internal <see cref="StreamReader"/> and removes all observers.
		/// </summary>
		public void Dispose()
		{
			Pause();

			if (_reader != null)
			{
				_reader.Dispose();
				_observers.Clear();
			}
		}

		private List<IObserver<TailPayload>> _observers;
		public IDisposable Subscribe(IObserver<TailPayload> observer)
		{
			if (!_observers.Contains(observer))
				_observers.Add(observer);
			return new TailPayload();
		}
	}

	public class TailPayload : IDisposable
	{
		public string TailString { get; set; }
		/// <summary>
		/// The type of event that has occured. Usually it's <see cref="FileEvent.TailGrown"/>.
		/// </summary>
		public FileEvent TailEvent { get; set; }

		public void Dispose()
		{
			
		}
	}

	public enum FileEvent
	{
		/// <summary>
		/// The tailed file has be renamed
		/// </summary>
		Renamed = 1,
		/// <summary>
		/// A previously tailed file could not be found
		/// </summary>
		NotFound = 2,
		/// <summary>
		/// The tailed file has been deleted
		/// </summary>
		Deleted = 3,
		/// <summary>
		/// The tailed file has new (trailing) content
		/// </summary>
		TailGrown = 4
	}
}