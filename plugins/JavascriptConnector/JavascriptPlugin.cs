using System;

using Noesis.Javascript;

namespace JavascriptConnector
{
	internal class JavascriptPlugin
	{
		internal string Name { get; set; }
		internal JavascriptContext Runtime { get; set; }

		internal JavascriptPlugin (string name, string source)
		{
			this.Name = name;

			Runtime = new JavascriptContext ();
			Runtime.Run(source);
			Runtime.SetParameter("Console", new API.Console ());
			Runtime.SetParameter("Player", new NINSS.API.Player ());
			Runtime.SetParameter("Server", new NINSS.API.Server ());
			Runtime.SetParameter("Config", new NINSS.API.Config (name));
		}
		internal void Run(string source)
		{
			Runtime.Run(source);
		}
	}
}

