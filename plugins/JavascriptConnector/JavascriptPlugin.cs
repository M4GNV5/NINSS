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
			Runtime.SetParameter("Console", API.Console.Instance);
			Runtime.SetParameter("Player", NINSS.API.Player.Instance);
			Runtime.SetParameter("Server", NINSS.API.Server.Instance);
			Runtime.SetParameter("Config", new NINSS.API.Config (name));
			Run(source);
		}
		internal void Run(string source)
		{
			try
			{
				Runtime.Run(source);
			}
			catch (JavascriptException e)
			{
				Console.WriteLine("\n/!\\ Error in Javascript Plugin {0}\nError: {1}\nLine {2}, Column {3} to {4}\n", Name, e.Message, e.Line, e.StartColumn, e.EndColumn);
			}
		}
	}
}

