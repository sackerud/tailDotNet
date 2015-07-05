using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using tailDotNet.Configuration;
using tailDotNet.SignalR.Host.Filesystem;
using tailDotNet.Watchers;

namespace tailDotNet.SignalR.Host
{
	class Program
	{
		private static MyHub _hub = new MyHub();

		static void Main(string[] args)
		{
			// This will *ONLY* bind to localhost, if you want to bind to all addresses
			// use http://*:8080 to bind to all addresses. 
			// See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
			// for more information.
			string url = "http://localhost:8080";
			using (WebApp.Start(url))
			{

				Console.WriteLine("Server running on {0}", url);
				var hubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
				hubContext.Clients.All.addMessage("tailDotNet",
											String.Format(
												"This is tailDotNet speaking. You've succeefully connected to the tailDotNet host running on {0}.",
												url));

				while (true)
				{
					var input = Console.ReadLine();
					if (input == "exit")// || input == "quit" || input = "q")
						Environment.Exit(0);

					hubContext.Clients.All.addMessage("tailDotNet", input);
				}
			}
		}
	}

	class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseCors(CorsOptions.AllowAll);
			app.MapSignalR();
		}
	}
	public class MyHub : Hub, IObserver<TailPayload>
	{
		public void Send(string name, string message)
		{
			Clients.All.addMessage(name, message);
		}

		public void Tail(string filename)
		{
			Console.WriteLine("Recieved request for tailing {0}", filename);
			//Task.Run(() => SpawnNewFileWatcher(filename));
			SpawnNewFileWatcher(filename);
		}

		public void TailOnHost(string filename)
		{
			Console.WriteLine("Recieved request for tailing {0} on this host", filename);
		}

		public void GetDirectoriesOfVolume(string volumeName)
		{
			if (volumeName == null) throw new ArgumentNullException();

			var fso = new FileSystemObject
			{
				Name = volumeName,
				Type = FsType.Drive
			};

			var dirs = new FileSystemManager().GetDirectories(fso);

			Clients.All.printVolumes(dirs);
		}

		public void GetVolumes()
		{
			Console.WriteLine("Recieved request to enumerate volumes");
			var vols = new FileSystemManager().GetVolumes();
			Console.WriteLine("{0} volume(s) found", vols.Count());
			Clients.All.printVolumes(vols);
			Console.WriteLine("Volume enumeration sent to clients");
		}

		private string _filename = null;
		private void SpawnNewFileWatcher(string filename)
		{
			var conf = new FileWatchConfiguration {FileName = filename,};
			ISleeper sleeper = new ThreadSleeper();
			IStreamReader streamReader = new TailStreamReader(conf.FileName);
			var filewatcher = new FileWatcher(conf, streamReader, sleeper);
			filewatcher.Subscribe(this);
			Task.Run(() => filewatcher.StartAsync());
			//TailWatcherProxy.StartWatcher(TailWatcherProxy.WatcherType.File, conf, streamReader, sleeper);
			Console.WriteLine("Started tailing {0}", filename);
		}

		public void OnNext(TailPayload payload)
		{
			Clients.All.addMessage(_filename, payload.TailString);
		}

		public void OnError(Exception error)
		{
			Clients.All.addMessage(_filename, error.Message);
		}

		public void OnCompleted()
		{
			throw new NotImplementedException();
		}
	}

	public class SignalRWatcher
	{
		
	}
}
