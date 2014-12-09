using System;
using System.Diagnostics;

namespace NINSS
{
	public class ServerManager
	{
		private API.Config config;
		private Process mc;
		private MinecraftConnector connector;

		public event EventHandler<DataReceivedEventArgs> OutputReceived;

		public ServerManager (string jarFile)
		{
			config = new NINSS.API.Config ("NINSS");
			string executable = config.GetValue("JavaExecutable");
			string arguments = config.GetValue("JavaArguments").Replace("%jar%", "\""+jarFile+"\"");

			mc = new Process();
			mc.StartInfo = new ProcessStartInfo(executable, arguments);
			mc.StartInfo.UseShellExecute = false;
			mc.StartInfo.RedirectStandardOutput = true;
			mc.StartInfo.RedirectStandardInput = true;
			mc.StartInfo.RedirectStandardError = true;
			mc.StartInfo.CreateNoWindow = true;

			mc.OutputDataReceived += ReadMessage;
			mc.ErrorDataReceived += ReadMessage;

			connector = new MinecraftConnector (this);
		}
		public void Start()
		{
			Console.WriteLine("\nStarting java with arguments: {0}\n", mc.StartInfo.Arguments);
			mc.Start();
			mc.BeginOutputReadLine();
			mc.BeginErrorReadLine();
		}
		public void WriteLine(string message)
		{
			if(!mc.HasExited)
				mc.StandardInput.WriteLine(message);
		}

		private void ReadMessage(object sender, DataReceivedEventArgs e)
		{
			if (e == null || e.Data == null)
				return;

			Console.WriteLine(e.Data);

			if (OutputReceived != null)
				OutputReceived(this, e);
		}
		private void ServerStop(object sender, EventArgs e)
		{
			if (!mc.HasExited)
				WriteLine("stop");

			if(config.GetValue("Close_Window_on_Serverstop") == "true")
				Environment.Exit(0);
			Console.WriteLine("\nServer stopped!\nPress any key to Exit!");
			Console.ReadKey();
			Environment.Exit(0);
		}
	}
}

