using System;
using System.IO;
using NINSS;
using Noesis.Javascript;
namespace JavascriptConnector
{
	public class JavascriptPluginManager
	{
		/// <summary>
		/// List of all loaded Javascript Plugins
		/// </summary>
		public System.Collections.Generic.Dictionary<string, string> plugins {get; internal set;}
		public JavascriptContext javascriptContext;
		public JavascriptPluginManager ()
		{
			plugins = new System.Collections.Generic.Dictionary<string, string>();
			if(!Directory.Exists("./plugins"))
				Directory.CreateDirectory("./plugins");
			if(Directory.GetFiles("./plugins", "*.js").Length > 0)
			{
				plugins = new System.Collections.Generic.Dictionary<string, string>();
				Console.WriteLine("\nLoading Javascript Plugins!");
				foreach(string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory+"plugins", "*.js"))
					loadPlugin(file.Split('\\', '/')[file.Split('\\', '/').Length-1].Replace(".js", ""));
				Console.WriteLine("Finished Loading Javascript Plugins!");
			}
			else
				Console.WriteLine("\nNo Javascript Plugins found!");
			javascriptContext = new JavascriptContext();
			addJsReferences(System.Reflection.Assembly.GetAssembly(typeof(MainClass)), "NINSS.API", ref javascriptContext);
			addJsReferences(System.Reflection.Assembly.GetAssembly(typeof(JavascriptConnector)), "JavascriptConnector.API", ref javascriptContext);
		}
		/// <summary>
		/// Unloads a Javascript plugin
		/// </summary>
		/// <param name="name">Plugin name</param>
		public void unloadPlugin(string name)
		{
			plugins.Remove(name);
			Console.WriteLine("Unloaded Plugin: "+name+".js");
		}
		/// <summary>
		/// Loads a Javascript plugin
		/// </summary>
		/// <param name="name">Plugin name</param>
		public void loadPlugin(string name)
		{
			plugins.Add(name, File.ReadAllText("./plugins/"+name+".js"));
			Console.WriteLine("Loaded Javascript Plugin: "+name+".js");
		}
		/// <summary>
		/// Reloads a Javascript plugin
		/// </summary>
		/// <param name="name">Plugin name</param>
		public void reloadPlugin(string name)
		{
			unloadPlugin(name);
			loadPlugin(name);
		}
		/// <summary>
		/// Executes a function in all loaded Javascript plugins
		/// </summary>
		/// <param name="function">Function name</param>
		public void executeAll(string function)
		{
			foreach(string key in plugins.Keys)
				execute(key, function, javascriptContext);
		}
		/// <summary>
		/// Executes a function in all loaded Javascript plugins with specific libraries
		/// </summary>
		/// <param name="function">Function name</param>
		/// <param name="jsContext">Javascript context with custom libraries</param>
		public void executeAll(string function, JavascriptContext jsContext)
		{
			foreach(string key in plugins.Keys)
				execute(key, function, jsContext);
		}
		/// <summary>
		/// Executes a function in specific plugin
		/// </summary>
		/// <param name="plugin">Plugin name</param>
		/// <param name="function">Function name</param>
		public void execute(string plugin, string function)
		{
			execute(plugin, function, javascriptContext);
		}
		/// <summary>
		/// Executes a function in specific plugin with specific libraries
		/// </summary>
		/// <param name="plugin">Plugin name</param>
		/// <param name="function">Function name</param>
		/// <param name="jsContext">Javascript context with custom libraries</param>
		public void execute(string plugin, string function, JavascriptContext jsContext)
		{
			if(!plugins.ContainsKey(plugin))
				return;
			else if(!plugins[plugin].Contains(function.Split('(')[0].Trim()))
				return;
			jsContext.Run(function+" "+plugins[plugin]);
		}
		public void addJsReferences(System.Reflection.Assembly assembly, string _namespace)
		{
			addJsReferences(assembly, _namespace, ref javascriptContext);
		}
		/// <summary>
		/// Adds all Classes in the assembly with the specific namespace to the specific JavascriptContext
		/// </summary>
		/// <param name="assembly">Assembly</param>
		/// <param name="_namespace">Namespace</param>
		/// <param name="jsContext">Javascript Context</param>
		public void addJsReferences(System.Reflection.Assembly assembly, string _namespace, ref JavascriptContext jsContext)
		{
			foreach(System.Type type in assembly.GetTypes())
				if(type.Namespace == _namespace)
					if(type.GetConstructor(System.Type.EmptyTypes) != null)
						jsContext.SetParameter(type.Name, System.Activator.CreateInstance(type));
		}
	}
}



