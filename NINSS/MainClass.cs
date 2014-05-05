using System;
using System.IO;
using NINSS;
using NINSS.API;
using System.Runtime.InteropServices;

namespace NINSS
{
	/// <summary>
	/// Main class
	/// </summary>
	public class MainClass
	{
		internal static System.Threading.Thread inputThread;
		internal static MinecraftServerManager serverManager;
		public static PluginManager pluginManager;
		
		public static void Main (string[] args)
		{
			try
			{
				if(!File.Exists(AppDomain.CurrentDomain.BaseDirectory+"plugins\\configs\\NINSS.xml"))
				{
					Console.WriteLine("Missing config NINSS.xml!\nPlease use and edit the one github!");
					Console.WriteLine("Press any key to exit");
					Console.ReadKey();
					Environment.Exit(0);
				}
				Config config = new Config("NINSS");
				if(config.getValue("EnablePlugins") == "true")
					pluginManager = new PluginManager();
				try
				{
					SetConsoleCtrlHandler(new HandlerRoutine(onExit), true);
				}
				catch
				{
					Console.WriteLine("It seems that you are on Linux/mac do not close this Program before typing 'stop' or server will be running in background!");
					Console.WriteLine("Remember to set 'Executable' to 'java' in config");
				}
				if(config.getValue("ServerFile") == "%auto%")
				{
					if (Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.jar").Length == 0)
					{
						System.Console.WriteLine("No *.jar File found! Please Download a Minecraftserver *.jar file from http://minecraft.net");
						System.Console.WriteLine("Press any key to exit");
						System.Console.ReadKey();
						System.Environment.Exit(0);
					}
					else if (Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.jar").Length > 1)
					{
						System.Console.WriteLine("Found "+Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.jar").Length+
						                         " *.jar Files please remove "+(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.jar").Length-1)+
						                         " of them or select on in the config\n");
						System.Console.WriteLine("Press any key to exit");
						System.Console.ReadKey();
						System.Environment.Exit(0);
					}
					else
						serverManager = new MinecraftServerManager(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.jar")[0]);
				}
				else
					serverManager = new MinecraftServerManager(config.getValue("ServerFile"));
				inputThread = new System.Threading.Thread(new System.Threading.ThreadStart(MinecraftServerManager.readInputs));
				inputThread.Start();
			}
			catch (Exception e)
			{
				Console.WriteLine("Fatal Error during startup!\nMessage:\n"+e.Message+"\nStacktrace:\n"+e.StackTrace);
				if(e.InnerException != null)
					Console.WriteLine("InnerException:\nMessage:\n"+e.InnerException.Message+"\nStacktrace:\n"+e.InnerException.StackTrace);
			}
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
		public static bool onExit(CtrlTypes CtrlType)
		{
			inputThread.Abort();
			serverManager.writeMessage("stop");
			return true;
		}
	}
}
