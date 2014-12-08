using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace NINSS
{
	public class PluginManager
	{
		public List<INinssPlugin> Plugins { get; private set; }

		public PluginManager ()
		{
			Plugins = new List<INinssPlugin> ();

			AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
			if(!Directory.Exists(PluginPath))
				Directory.CreateDirectory(PluginPath);

			string[] pluginFiles = Directory.GetFiles(PluginPath, "*.dll");
			if (pluginFiles.Length > 0)
			{
				Console.WriteLine("Loading Plugins!");
				foreach (string file in pluginFiles)
				{
					Console.WriteLine("Loading Plugin: {0}", file);
					LoadPlugins(file);
				}
				Console.WriteLine("\nFinished Loading Plugins!");
			}
			else
			{
				Console.WriteLine("No Plugins found!");
			}

			Console.WriteLine("{0} Plugins loaded", Plugins.Count);
		}
			
		public void UnloadPlugin(string name)
		{
			Plugins.Remove(Plugins.First(p => p.Name == name));
		}

		public void LoadPlugins(string file)
		{
			try
			{
				Assembly ass = Assembly.LoadFrom(file);
				foreach(Type t in ass.GetTypes())
				{
					if(typeof(INinssPlugin).IsAssignableFrom(t))
					{
						Plugins.Add((INinssPlugin)Activator.CreateInstance(t));
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("\nCould not load Plugin: {0}\n{1}\nError: {2}\nSTacktrace: {3}", Path.GetFileName(file), e.GetType(), e.Message, e.StackTrace);
				if(e.InnerException != null)
					Console.WriteLine("InnerException: {0}\nMessage: {1}\nStacktrace: {2}", e.InnerException.GetType(), e.InnerException.Message, e.InnerException.StackTrace);
			}
		}
			
		private Assembly AssemblyResolve(object sender, ResolveEventArgs e)
		{
			if (Assembly.Load(new AssemblyName (e.Name)) != null)
				return Assembly.Load(new AssemblyName (e.Name));

			string name = new AssemblyName(e.Name).Name;
			return Assembly.LoadFile(Path.Combine(PluginPath, "libs/"+name+".dll"));
		}

		private string PluginPath
		{
			get {
				return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
			}
		}
	}
}

