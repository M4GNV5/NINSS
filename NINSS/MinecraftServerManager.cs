using System;
namespace NINSS
{
	public class MinecraftServerManager
	{
		public delegate void ServerMessage(string message);
		public event ServerMessage OnServerMessage;
		public System.Diagnostics.Process McProcess {get; private set;}

		public MinecraftServerManager (string jarFile)
		{
			string executable = new API.Config("NINSS").GetValue("JavaExecutable");
			string arguments = new API.Config("NINSS").GetValue("JavaArguments").Replace("%jar%", "\""+jarFile+"\"");
			Console.WriteLine("\nStarting java with arguments: "+arguments+"\n");
			OnServerMessage += MinecraftConnector.OnServerMessage;
			MinecraftConnector.ServerStop += OnStop;
			McProcess = new System.Diagnostics.Process();
			McProcess.StartInfo = new System.Diagnostics.ProcessStartInfo(executable, arguments);
			McProcess.StartInfo.UseShellExecute = false;
			McProcess.StartInfo.RedirectStandardOutput = true;
			McProcess.StartInfo.RedirectStandardInput = true;
			McProcess.StartInfo.RedirectStandardError = true;
			McProcess.StartInfo.CreateNoWindow = true;
			McProcess.OutputDataReceived += ReadMessage;
			McProcess.ErrorDataReceived += ReadMessage;
			McProcess.Start();
			McProcess.BeginOutputReadLine();
			McProcess.BeginErrorReadLine();
		}
		public void ReadMessage(object sender, System.Diagnostics.DataReceivedEventArgs e)
		{
			if(e == null || e.Data == null)
				return;
			Console.WriteLine(e.Data.Remove(0, 12).Replace(e.Data.Remove(0, 12).Split(':')[0], "").Trim(':', ' ', '[', ']'));
			if(OnServerMessage != null && e.Data != null)
			{
				try
				{
					OnServerMessage(e.Data);
				}
				catch (Exception ex)
				{
					Console.WriteLine("\nError while calling Event!");
					Console.WriteLine("Error: "+ex.Message);
					Console.WriteLine("Stacktrace:\n"+ex.StackTrace+"\n");
				}
			}
		}
		public void WriteMessage(string message)
		{
			McProcess.StandardInput.WriteLine(message);
		}
		public static void ReadInputs()
		{
			while(MainClass.serverManager.McProcess != null)
			{
				string message = System.Console.ReadLine();
				if(message.Length > 0 && message[0] == '!' && MainClass.serverManager.OnServerMessage != null)
				{
					try
					{
						MainClass.serverManager.OnServerMessage("[XX:XX:XX] [NINSS/onCommand]: <Console> "+message);
					}
					catch (Exception ex)
					{
						Console.WriteLine("\nError while calling Event!");
						Console.WriteLine("Error: "+ex.Message);
						Console.WriteLine("Stacktrace:\n"+ex.StackTrace+"\n");
					}
				}
				else
					MainClass.serverManager.WriteMessage(message);
			}
		}
		public void OnStop()
		{
			System.Threading.Thread exitThread = new System.Threading.Thread(new System.Threading.ThreadStart(OnExit));
			exitThread.Start();
		}
		public void OnExit()
		{
			MainClass.inputThread.Abort();
			System.Threading.Thread.Sleep(300);
			if(new API.Config("NINSS").GetValue("Close_Window_on_Serverstop") == "true")
				Environment.Exit(0);
			Console.WriteLine("\nServer stopped!\nPress any key to Exit!");
			Console.ReadKey();
			Environment.Exit(0);
		}
	}
}

