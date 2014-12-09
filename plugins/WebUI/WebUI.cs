using System;
using System.Threading;
using NINSS;

namespace WebUI
{
	public class WebUI : HttpServer, INinssPlugin
	{
		public string Name { get { return "WebUI"; } }

		private Thread serverThread;
		public delegate bool RequestAction(string url, HttpProcessor p);
		public static System.Collections.Generic.Dictionary<string, string> mineTypes = new System.Collections.Generic.Dictionary<string, string>()
		{
			{"js", "text/javascript"}
		};
		public static RequestAction OnRequest;
		public WebUI ()
		{
			NINSS.MinecraftConnector.ServerStop += OnStop;
			NINSS.MinecraftConnector.PlayerChatReceived += ChatReceived;
			OnRequest += LogAction;
			OnRequest += PluginsAction;
			OnRequest += CommandAction;
			OnRequest += ConfigAction;
			OnRequest += ConfigsAction;
			OnRequest += ConfigListAction;
			try
			{
				NINSS.API.Config config = new NINSS.API.Config("WebUI");
				string _ip = config.GetValue("IP_Adress");
				this.ip = System.Net.IPAddress.Parse(_ip);
				port = Convert.ToInt32(config.GetValue("Port"));
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
		public void ChatReceived(object sender, PlayerChatEventArgs e)
		{
			if(e.Message == ".webui stop")
				OnStop(null, null);
		}
		public void OnStop(object sender, ServerEventArgs e)
		{
			this.is_active = false;
			this.listener.Stop();
			Thread.Sleep(100);
			serverThread.Abort();
			listener = null;
		}

		public override void HandleGETRequest (HttpProcessor p)
		{
			string file = p.http_url.Trim(' ', '/');
			if (OnRequest != null)
			{
				foreach (RequestAction _action in OnRequest.GetInvocationList())
				{
					if (_action(file, p))
						return;
				}
			}
			if (file == "")
			{
				file = "index.html";
			}
			file = AppDomain.CurrentDomain.BaseDirectory+"plugins\\WebUI\\"+file.Split('?')[0];
			if(!System.IO.File.Exists(file))
			{
				p.writeSuccess("text/html");
				p.outputStream.WriteLine("<html><body><h1>404 Not found!</h1></body></html>");
			}
			else
			{
				if (mineTypes.ContainsKey(file.Split('.') [file.Split('.').Length - 1]))
				{
					p.writeSuccess(mineTypes [file.Split('.') [file.Split('.').Length - 1]]);
				}
				else
				{
					p.writeSuccess("text/" + file.Split('.') [file.Split('.').Length - 1]);
				}
				foreach (string line in System.IO.File.ReadAllLines(file))
				{
					p.outputStream.WriteLine(line);
				}
			}
		}

		public static bool LogAction(string url, HttpProcessor p)
		{
			if(url.Split('?').Length != 2 || url.Split('?')[1] != "log")
				return false;

			System.IO.File.Copy("logs\\latest.log", "plugins\\WebUI\\log.log");
			foreach (string line in System.IO.File.ReadAllLines("plugins\\WebUI\\log.log"))
			{
				p.outputStream.WriteLine(line);
			}
			System.IO.File.Delete("plugins\\WebUI\\log.log");
			return true;
		}
		public static bool PluginsAction(string url, HttpProcessor p)
		{
			if(url.Split('?').Length != 2 || url.Split('?')[1] != "plugins")
				return false;

			string plugins = "";
			foreach (INinssPlugin plugin in NINSS.MainClass.pluginManager.Plugins)
			{
				plugins += plugin.Name + ",";
			}
			p.outputStream.WriteLine(plugins.Trim(','));
			return true;
		}
		public static bool ConfigsAction(string url, HttpProcessor p)
		{
			if(url.Split('?').Length != 2 || url.Split('?')[1] != "configlist")
				return false;

			string values = "";
			foreach (string file in System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory+"plugins\\configs"))
			{
				values += file.Split('\\') [file.Split('\\').Length - 1].Replace(".xml", "") + ",";
			}
			p.outputStream.WriteLine(values.Trim(','));
			return true;
		}
		public static bool CommandAction(string url, HttpProcessor p)
		{
			if(url.Split('?').Length != 3 || url.Split('?')[1] != "command")
				return false;

			p.writeSuccess("text/html");
			string command = url.Split('?') [2].Replace("%20", " ");
			NINSS.API.Server.Instance.RunCommand(command);
			p.outputStream.WriteLine("true");
			return true;
		}
		public static bool ConfigAction(string url, HttpProcessor p)
		{
			if(url.Split('?').Length != 5 || url.Split('?')[1] != "config")
				return false;

			p.writeSuccess("text/html");
			NINSS.API.Config config = new NINSS.API.Config(url.Split('?')[2]);
			string name = url.Split('?')[3];
			string value = url.Split('?')[4].Replace("%20", " ");
			if(value == "get")
			{
				value = config.GetValue(name);
				if(value == null)
					value = "null";
				p.outputStream.WriteLine(value);
			}
			else
			{
				config.SetValue(name, value);
				config.SaveConfig(url.Split('?')[2]);
				p.outputStream.WriteLine("true");
			}
			return true;
		}
		public static bool ConfigListAction(string url, HttpProcessor p)
		{
			if(url.Split('?').Length != 3 || url.Split('?')[1] != "configvalues")
				return false;

			p.writeSuccess("text/html");
			string values = "";
			foreach (string val in new NINSS.API.Config(url.Split('?')[2]).GetValues())
			{
				values += val + ",";
			}
			p.outputStream.WriteLine(values.Trim(','));
			return true;
		}
	}
}

