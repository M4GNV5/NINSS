using System;
using System.IO;
using System.Collections.Generic;

using NINSS;
using Noesis.Javascript;
namespace JavascriptConnector
{
	internal class JavascriptPluginManager
	{
		internal JavascriptPlugin[] Plugins { get; set; }

		internal JavascriptPluginManager()
		{
			Plugins = new JavascriptPlugin[0];

			string[] pluginFiles = Directory.GetFiles(PluginPath, "*.js");
			if (pluginFiles.Length > 0)
			{
				List<JavascriptPlugin> _plugins = new List<JavascriptPlugin> ();

				Console.WriteLine("\nLoading Javascript Plugins!");
				foreach (string file in pluginFiles)
				{
					Console.WriteLine("Loading Plugin: {0}", file);
					string source = File.ReadAllText(file);
					string name = Path.GetFileNameWithoutExtension(file);
					JavascriptPlugin plugin = new JavascriptPlugin (name, source);
					_plugins.Add(plugin);
				}
				Plugins = _plugins.ToArray();
				Console.WriteLine("Finished Loading Javascript Plugins!");
			}
			else
			{
				Console.WriteLine("No Javascript Plugins found!");
			}
			Console.WriteLine("{0} Javascript Plugins loaded\n", Plugins.Length);
		}


		private string PluginPath
		{
			get {
				return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
			}
		}
	}
}
