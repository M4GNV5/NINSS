using System;
using NINSS;
namespace JavascriptConnector
{
	/// <summary>
	/// Javascript connector class that calls the events in all javascript plugins
	/// </summary>
	public class JavascriptConnector
	{
		public JavascriptPluginManager manager;
		public JavascriptConnector()
		{
			manager = new JavascriptPluginManager();
			MinecraftConnector.OnPlayerJoin += onJoin;
			MinecraftConnector.OnPlayerLeave += onLeave;
			MinecraftConnector.OnPlayerPosition += onPosition;
			MinecraftConnector.OnChat += onChat;
			MinecraftConnector.OnCommand += onCommand;
			
			MinecraftConnector.OnStart += onStart;
			MinecraftConnector.OnStop += onStop;
		}
		
		public void onJoin(string name, string misc)
		{
			manager.executeAll("onJoin(\""+name+"\");");
		}
		public void onLeave(string name, string misc)
		{
			manager.executeAll("onLeave(\""+name+"\");");
		}
		public void onPosition(string name, string position)
		{
			manager.executeAll("onPosition(\""+name+"\", \""+position+"\");");
		}
		public void onChat(string name, string message)
		{
			manager.executeAll("onChat(\""+name+"\", \""+message+"\");");
		}
		public void onCommand(string name, string args)
		{
			if(args.Split(' ')[0] == "reload")
				manager = new JavascriptPluginManager();
			else if(args.Split(' ')[0] == "unload" && args.Split(' ').Length > 1)
				manager.unloadPlugin(args.Split(' ')[1]);
			else if(args.Split(' ')[0] == "load" && args.Split(' ').Length > 1)
				manager.loadPlugin("./plugins/"+args.Split(' ')[1]);
			manager.executeAll("onCommand(\""+name+"\", \""+args+"\");");
		}
		
		public void onStart()
		{
			manager.executeAll("onStart();");
		}
		public void onStop()
		{
			manager.executeAll("onStop();");
		}
	}
}

