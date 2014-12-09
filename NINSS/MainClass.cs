using System;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace NINSS
{
	public class MainClass
	{
		internal static Thread inputThread;
		internal static ServerManager serverManager;
		public static PluginManager pluginManager;
		
		public static int Main (string[] args)
		{
			try
			{
				if(!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins/configs/NINSS.xml")))
				{
					Console.WriteLine("Missing config NINSS.xml!\nPlease use and edit the one from github!");
					Console.WriteLine("Press any key to exit");
					Console.ReadKey();
					return 1;
				}

				API.Config config = new API.Config("NINSS");

				try
				{
					SetConsoleCtrlHandler(new HandlerRoutine(OnExit), true);
				}
				catch
				{
					Console.WriteLine("It seems that you are on Linux/mac do not close this Program before typing 'stop' or server will be running in background!");
					Console.WriteLine("Remember to set 'Executable' to 'java' in config");
				}
				if(config.GetValue("ServerFile") == "%auto%")
				{
					string[] jarFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.jar");

					if (jarFiles.Length == 0)
					{
						System.Console.WriteLine("No *.jar File found! Please Download a Minecraftserver *.jar file from http://minecraft.net");
						System.Console.WriteLine("Press any key to exit");
						System.Console.ReadKey();
						return 1;
					}
					else if (jarFiles.Length > 1)
					{
						System.Console.WriteLine("Found {0} *.jar Files please remove {1} of them or select one in the config", jarFiles.Length, (jarFiles.Length-1));
						System.Console.WriteLine("\nPress any key to exit");
						System.Console.ReadKey();
						return 1;
					}
					else
					{
						serverManager = new ServerManager(jarFiles[0]);
					}
				}
				else
				{
					serverManager = new ServerManager(config.GetValue("ServerFile"));
				}
				if(config.GetValue("EnablePlugins") == "true")
					pluginManager = new PluginManager();

				serverManager.Start();

				ThreadStart ts = new ThreadStart(ReadInputs);
				inputThread = new Thread(ts);
				inputThread.Start();
			}
			catch (Exception e)
			{
				Console.WriteLine("Fatal Error during startup!\nMessage:\n"+e.Message+"\nStacktrace:\n"+e.StackTrace);
				if(e.InnerException != null)
					Console.WriteLine("InnerException:\nMessage:\n"+e.InnerException.Message+"\nStacktrace:\n"+e.InnerException.StackTrace);
			}
			return 0;
		}

		private static void ReadInputs()
		{
			string message = "";
			do
			{
				message = Console.ReadLine();
				if (!string.IsNullOrWhiteSpace(message))
				{
					serverManager.WriteLine(message);
				}
			} while(message != "stop");
		}
		
		[DllImport("Kernel32")]
		public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);
		public delegate bool HandlerRoutine(CtrlTypes CtrlType);
		public enum CtrlTypes
		{
			CTRL_C_EVENT = 0,
			CTRL_BREAK_EVENT,
			CTRL_CLOSE_EVENT,
			CTRL_LOGOFF_EVENT = 5,
			CTRL_SHUTDOWN_EVENT
		}
		public static bool OnExit(CtrlTypes CtrlType)
		{
			serverManager.WriteLine("stop");
			if(inputThread == null)
				return true;
			inputThread.Abort();
			return true;
		}
	}
}
