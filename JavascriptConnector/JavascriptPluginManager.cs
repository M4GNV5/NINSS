using System;
using System.IO;
using NINSS;
using Noesis.Javascript;
namespace JavascriptConnector
{
	public class JavascriptPluginManager
	{
		public System.Collections.Generic.Dictionary<string, string> plugins {get; internal set;}
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
		}
		public void unloadPlugin(string name)
		{
			plugins.Remove(name);
			Console.WriteLine("Unloaded Plugin: "+name+".js");
		}
		public void loadPlugin(string name)
		{
			plugins.Add(name, File.ReadAllText("./plugins/"+name+".js"));
			Console.WriteLine("Loaded Javascript Plugin: "+name);
		}
		public void reloadPlugin(string name)
		{
			unloadPlugin(name);
			loadPlugin(name);
		}

		public void executeAll(string function)
		{
			foreach(string key in plugins.Keys)
				execute(key, function);
		}
		public void executeAll(string function, JavascriptContext jsContext)
		{
			foreach(string key in plugins.Keys)
				execute(key, function, jsContext);
		}
		public void execute(string plugin, string function)
		{
			JavascriptContext context = new JavascriptContext();
			addJsReferences(System.Reflection.Assembly.GetAssembly(typeof(MainClass)), "NINSS.API", ref context);
			addJsReferences(System.Reflection.Assembly.GetAssembly(typeof(JavascriptConnector)), "JavascriptConnector.API", ref context);
			execute(plugin, function, context);
		}
		public void execute(string plugin, string function, JavascriptContext jsContext)
		{
			if(!plugins.ContainsKey(plugin))
				return;
			else if(!plugins[plugin].Contains(function.Split('(')[0].Trim()))
				return;
			jsContext.Run(function+plugins[plugin]);
		}
		public void addJsReferences(System.Reflection.Assembly assembly, string _namespace, ref JavascriptContext jsContext)
		{
			foreach(System.Type type in assembly.GetTypes())
				if(type.Namespace == _namespace)
					if(type.GetConstructor(System.Type.EmptyTypes) != null)
						jsContext.SetParameter(type.Name, System.Activator.CreateInstance(type));
		}
	}
}

