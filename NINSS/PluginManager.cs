using System;
using System.IO;
namespace NINSS
{
	/// <summary>
	/// PluginManager class that loads, undloads and holds all .NET Plugins
	/// </summary>
	public class PluginManager
	{
		/// <summary>
		/// List of all loaded Plugins
		/// </summary>
		public System.Collections.Generic.Dictionary<string, object> plugins {get; internal set;}
		public PluginManager ()
		{
			AppDomain.CurrentDomain.AssemblyResolve += onAssemblyResolve;
			if(!Directory.Exists("./plugins"))
				Directory.CreateDirectory("./plugins");
			if(Directory.GetFiles("./plugins", "*.dll").Length > 0)
			{
				plugins = new System.Collections.Generic.Dictionary<string, object>();
				Console.WriteLine("Loading Plugins!");
				foreach(string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory+"plugins", "*.dll"))
					loadPlugin(file.Split('\\', '/')[file.Split('\\', '/').Length-1].Replace(".dll", ""));
				Console.WriteLine("\nFinished Loading Plugins!");
			}
			else
				Console.WriteLine("No Plugins found!");
		}
		/// <summary>
		/// Loads missing assemblies
		/// </summary>
		/// <returns>The Assembly</returns>
		/// <param name="sender">Sender</param>
		/// <param name="e">Resolve Event Arguments</param>
		public System.Reflection.Assembly onAssemblyResolve(object sender, ResolveEventArgs e)
		{
			foreach(System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				if(assembly.FullName == e.Name)
					return assembly;
			string name = new System.Reflection.AssemblyName(e.Name).Name;
			return System.Reflection.Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins/libs/"+name+".dll"));
		}

		/// <summary>
		/// Unloads a plugin
		/// </summary>
		/// <param name="name">Plugin name</param>
		public void unloadPlugin(string name)
		{
			plugins.Remove(name);
		}
		/// <summary>
		/// Loads a plugin
		/// </summary>
		/// <param name="name">Plugin name</param>
		public void loadPlugin(string name)
		{
			try
			{
				Type type = null;
				foreach(Type t in System.Reflection.Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory+"plugins/"+name+".dll").GetTypes())
					if(t.Name == name)
						type = t;
				if(type == null)
					throw new NotImplementedException("The Dll '"+name+".dll' Does not contain a Class with same name!");
				plugins.Add(name, Activator.CreateInstance(type));
				Console.WriteLine("Loaded Plugin: "+name+".dll");
			}
			catch (Exception e)
			{
				Console.WriteLine("\nCould not load Plugin: "+name+"\n"+e.GetType().ToString()+": "+e.Message+"\nStacktrace:\n"+e.StackTrace+"\n");
				if(e.InnerException != null)
					Console.WriteLine("InnerException: "+e.InnerException.Message+"\nInner Stacktrace:\n"+e.InnerException.StackTrace);
			}
		}
		/// <summary>
		/// Reloads a plugin
		/// </summary>
		/// <param name="name">Plugin name</param>
		public void reloadPlugin(string name)
		{
			unloadPlugin(name);
			loadPlugin(name);
		}
	}
}

