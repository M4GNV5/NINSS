using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

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
		public List<INinssPlugin> Plugins { get; internal set; }
		public PluginManager ()
		{
			AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
			if(!Directory.Exists(PluginPath))
				Directory.CreateDirectory(PluginPath);

			if(Directory.GetFiles(PluginPath, "*.dll").Length > 0)
			{
				Plugins = new List<INinssPlugin>();
				Console.WriteLine("Loading Plugins!");
				foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory+"plugins", "*.dll"))
				{
					Console.WriteLine("Loading Plugin: {0}", file);
					LoadPlugin(file);
				}
				Console.WriteLine("\nFinished Loading Plugins!");
			}
			else
				Console.WriteLine("No Plugins found!");
			Console.WriteLine("Plugins: {0}", Plugins.Count);
		}

		/// <summary>
		/// Unloads a plugin
		/// </summary>
		/// <param name="name">Plugin name</param>
		public void UnloadPlugin(string name)
		{
			Plugins.Remove(Plugins.First(p => p.Name == name));
		}
		/// <summary>
		/// Loads a plugin
		/// </summary>
		/// <param name="name">Plugin name</param>
		public void LoadPlugin(string file)
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
				Console.WriteLine("\nCould not load Plugin: "+file+"\n"+e.GetType().ToString()+": "+e.Message+"\nStacktrace:\n"+e.StackTrace+"\n");
				if(e.InnerException != null)
					Console.WriteLine("InnerException: "+e.InnerException.Message+"\nInner Stacktrace:\n"+e.InnerException.StackTrace);
			}
		}

		/// <summary>
		/// Loads missing assemblies
		/// </summary>
		/// <returns>The Assembly</returns>
		/// <param name="sender">Sender</param>
		/// <param name="e">Resolve Event Arguments</param>
		public System.Reflection.Assembly OnAssemblyResolve(object sender, ResolveEventArgs e)
		{
			if (Assembly.Load(new AssemblyName (e.Name)) != null)
				return Assembly.Load(new AssemblyName (e.Name));
			string name = new System.Reflection.AssemblyName(e.Name).Name;
			return System.Reflection.Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins/libs/"+name+".dll"));
		}

		private string PluginPath
		{
			get {
				return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
			}
		}
	}
}

