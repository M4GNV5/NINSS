using System;
using System.Threading;
namespace WebUI
{
	public class WebUI : HttpServer
	{
		private Thread serverThread;
		public delegate bool action(string url, HttpProcessor p);
		public static System.Collections.Generic.Dictionary<string, string> mineTypes = new System.Collections.Generic.Dictionary<string, string>()
		{
			{"js", "text/javascript"}
		};
		public static action onAction;
		public WebUI ()
		{
			NINSS.MinecraftConnector.OnStop += onStop;
			NINSS.MinecraftConnector.OnCommand += onCommand;
			onAction += logAction;
			onAction += pluginsAction;
			onAction += commandAction;
			onAction += configAction;
			onAction += configsAction;
			onAction += configListAction;
			try
			{
				NINSS.API.Config config = new NINSS.API.Config("WebUI");
				System.Net.IPAddress.TryParse(config.getValue("IP_Adress"), out ip);
				port = Convert.ToInt32(config.getValue("Port"));
				serverThread = new Thread(new ThreadStart(listen));
				serverThread.Start();
			}
			catch (Exception e)
			{
				Console.WriteLine("\nError loading WebUI: "+e.GetType().ToString()+": "+e.Message+"\nStacktrace:\n"+e.StackTrace+"\n");
				if(e.InnerException != null)
					Console.WriteLine("InnerException: "+e.InnerException.Message+"\nInner Stacktrace:\n"+e.InnerException.StackTrace);
			}
		}
		public void onCommand(string name, string arg)
		{
			if(arg == "webui stop")
				onStop();
		}
		public void onStop()
		{
			this.is_active = false;
			this.listener.Stop();
			Thread.Sleep(100);
			serverThread.Abort();
			listener = null;
		}

		public override void handleGETRequest (HttpProcessor p)
		{
			string file = p.http_url.Trim(' ', '/');
			file.Replace("/", "\\");
			if(onAction != null)
				foreach(action _action in onAction.GetInvocationList())
					if(_action(file, p))
						return;
			if(file == "")
				file = "index.html";
			file = AppDomain.CurrentDomain.BaseDirectory+"plugins\\WebUI\\"+file.Split('?')[0];
			if(!System.IO.File.Exists(file))
			{
				p.writeSuccess("text/html");
				p.outputStream.WriteLine("<html><body><h1>404 Not found!</h1></body></html>");
			}
			else
			{
				if(mineTypes.ContainsKey(file.Split('.')[file.Split('.').Length-1]))
					p.writeSuccess(mineTypes[file.Split('.')[file.Split('.').Length-1]]);
				else
					p.writeSuccess("text/"+file.Split('.')[file.Split('.').Length-1]);
				foreach(string line in System.IO.File.ReadAllLines(file))
					p.outputStream.WriteLine(line);
			}
		}

		public static bool logAction(string url, HttpProcessor p)
		{
			if(url.Split('?').Length != 2 || url.Split('?')[1] != "log")
				return false;
			System.IO.File.Copy("logs\\latest.log", "plugins\\WebUI\\log.log");
			foreach(string line in System.IO.File.ReadAllLines("plugins\\WebUI\\log.log"))
				p.outputStream.WriteLine(line);
			System.IO.File.Delete("plugins\\WebUI\\log.log");
			return true;
		}
		public static bool pluginsAction(string url, HttpProcessor p)
		{
			if(url.Split('?').Length != 2 || url.Split('?')[1] != "plugins")
				return false;
			string plugins = "";
			foreach(string key in NINSS.MainClass.pluginManager.plugins.Keys)
				plugins += key+",";
			p.outputStream.WriteLine(plugins.Trim(','));
			return true;
		}
		public static bool configsAction(string url, HttpProcessor p)
		{
			if(url.Split('?').Length != 2 || url.Split('?')[1] != "configlist")
				return false;
			string values = "";
			foreach(string file in System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory+"plugins\\configs"))
				values += file.Split('\\')[file.Split('\\').Length-1].Replace(".xml", "")+",";
			p.outputStream.WriteLine(values.Trim(','));
			return true;
		}
		public static bool commandAction(string url, HttpProcessor p)
		{
			if(url.Split('?').Length != 3 || url.Split('?')[1] != "command")
				return false;
			p.writeSuccess("text/html");
			NINSS.API.Server.runCommand(url.Split('?')[2].Replace("%20", " "));
			p.outputStream.WriteLine("true");
			return true;
		}
		public static bool configAction(string url, HttpProcessor p)
		{
			if(url.Split('?').Length != 5 || url.Split('?')[1] != "config")
				return false;
			p.writeSuccess("text/html");
			NINSS.API.Config config = new NINSS.API.Config(url.Split('?')[2]);
			string name = url.Split('?')[3];
			string value = url.Split('?')[4].Replace("%20", " ");
			if(value == "get")
			{
				value = config.getValue(name);
				if(value == null)
					value = "null";
				p.outputStream.WriteLine(value);
			}
			else
			{
				config.setValue(name, value);
				config.saveConfig(url.Split('?')[2]);
				p.outputStream.WriteLine("true");
			}
			return true;
		}
		public static bool configListAction(string url, HttpProcessor p)
		{
			if(url.Split('?').Length != 3 || url.Split('?')[1] != "configvalues")
				return false;
			p.writeSuccess("text/html");
			string values = "";
			foreach(string val in new NINSS.API.Config(url.Split('?')[2]).getValues())
				values += val+",";
			p.outputStream.WriteLine(values.Trim(','));
			return true;
		}
	}
}

