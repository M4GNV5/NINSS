using System;
namespace NINSS
{
	internal class MinecraftServerManager
	{
		internal delegate void serverMessage(string message);
		internal event serverMessage OnServerMessage;
		internal System.Diagnostics.Process mcProcess;
		internal MinecraftServerManager (string jarFile)
		{
			string executable = new API.Config("..\\..\\NINASS_config").getValue("JavaExecutable");
			string arguments = new API.Config("..\\..\\NINASS_config").getValue("JavaArguments").Replace("%jar%", "\""+jarFile+"\"");
			Console.WriteLine("\nStarting java with arguments: "+arguments+"\n");
			OnServerMessage += MinecraftConnector.OnServerMessage;
			mcProcess = new System.Diagnostics.Process();
			mcProcess.StartInfo = new System.Diagnostics.ProcessStartInfo(executable, arguments);
			mcProcess.StartInfo.UseShellExecute = false;
			mcProcess.StartInfo.RedirectStandardOutput = true;
			mcProcess.StartInfo.RedirectStandardInput = true;
			mcProcess.StartInfo.RedirectStandardError = true;
			mcProcess.StartInfo.CreateNoWindow = true;
			mcProcess.OutputDataReceived += readMessage;
			mcProcess.ErrorDataReceived += readMessage;
			mcProcess.Start();
			mcProcess.BeginOutputReadLine();
			mcProcess.BeginErrorReadLine();
		}
		internal void readMessage(object sender, System.Diagnostics.DataReceivedEventArgs e)
		{
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
		internal void writeMessage(string message)
		{
			mcProcess.StandardInput.WriteLine(message);
		}
		internal static void readInputs()
		{
			while(MainClass.serverManager.mcProcess != null)
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
					MainClass.serverManager.writeMessage(message);
			}
		}
	}
}

