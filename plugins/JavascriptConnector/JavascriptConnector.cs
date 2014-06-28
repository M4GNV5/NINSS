using System;
using NINSS;
namespace JavascriptConnector
{
	/// <summary>
	/// Javascript connector class that calls the events in all javascript plugins
	/// </summary>
	public class JavascriptConnector : INinssPlugin
	{
		public string Name { get { return "JavascriptConnector"; } }

		public JavascriptPluginManager manager;
		public JavascriptConnector()
		{
			manager = new JavascriptPluginManager();
			MinecraftConnector.PlayerJoin += onJoin;
			MinecraftConnector.PlayerLeave += onLeave;
			MinecraftConnector.PlayerPosition += onPosition;
			MinecraftConnector.ChatReceived += onChat;
			MinecraftConnector.OnCommand += onCommand;
			
			MinecraftConnector.ServerStart += onStart;
			MinecraftConnector.ServerStop += onStop;
		}
		
		public void onJoin(string name, string misc)
		{
            manager.executeAll("PlayerJoin(\""+name+"\");");
		}
		public void onLeave(string name, string misc)
		{
            manager.executeAll("PlayerLeave(\""+name+"\");");
		}
		public void onPosition(string name, string position)
		{
            manager.executeAll("PlayerPosition(\""+name+"\", \""+position+"\");");
		}
		public void onChat(string name, string message)
		{
            manager.executeAll("ChatReceived(\""+name+"\", \""+message+"\");");
		}
		public void onCommand(string name, string args)
		{
			if(args.Split(' ')[0] == "reload")
				manager = new JavascriptPluginManager();
			else if(args.Split(' ')[0] == "unload" && args.Split(' ').Length > 1)
				manager.unloadPlugin(args.Split(' ')[1]);
			else if(args.Split(' ')[0] == "load" && args.Split(' ').Length > 1)
				manager.loadPlugin("./plugins/"+args.Split(' ')[1]);
            manager.executeAll("OnCommand(\""+name+"\", \""+args+"\");");
		}
		
		public void onStart()
		{
            manager.executeAll("ServerStart();");
		}
		public void onStop()
		{
            manager.executeAll("ServerStop();");
		}
	}
}

