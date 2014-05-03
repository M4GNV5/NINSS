using System;
using NINSS;
namespace UuidConnector
{
	public class UuidConnector
	{
		public static event MinecraftConnector.PlayerEvent onUuid;
		private static System.Collections.Generic.Dictionary<string, string> knownPlayer; //UUID, last name
		private object jsManager;
		public static System.Collections.Generic.Dictionary<string, string> KnownPlayer
		{
			get
			{
				if(knownPlayer == null)
					loadUuids();
				return knownPlayer;
			}
			internal set
			{
				knownPlayer = value;
			}
		}
		public UuidConnector()
		{
			MinecraftConnector.messageReader.Add("UUID of player", readUuid);
			MinecraftConnector.OnStop += saveUuids;
			MinecraftConnector.OnStart += onStart;
			if(knownPlayer == null)
				loadUuids();
		}
		public void onStart()
		{
			if(MainClass.pluginManager.plugins.ContainsKey("JavascriptConnector"))
			{
				jsManager = MainClass.pluginManager.plugins["JavascriptConnector"].GetType().GetField("manager").GetValue(MainClass.pluginManager.plugins["JavascriptConnector"]);
				object jsContext = jsManager.GetType().GetField("javascriptContext").GetValue(jsManager);
				jsContext.GetType().GetMethod("SetParameter", new System.Type[] {typeof(string), typeof(object)}).Invoke(jsContext, new Object[] {"Uuid", this});
				onUuid += jsUuidJoin;
				Console.WriteLine("Inserted 'Uuid' lib and 'onUuid' event into JavascriptConnector");
			}
			else
				Console.WriteLine("JavascriptConnector could not be found! Cannot insert UUID lib and event!");
		}
		public void jsUuidJoin(string uuid, string name)
		{
			if(jsManager != null)
				jsManager.GetType().GetMethod("executeAll", new System.Type[] {typeof(string)}).Invoke(jsManager, new object[] {"onUuid(\""+uuid+"\", \""+name+"\");"});
		}

		public static void readUuid(string message) //UUID of player <name> is <uuid>
		{
			if(!knownPlayer.ContainsKey(message.Split(' ')[5]))
				knownPlayer.Add(message.Split(' ')[5], message.Split(' ')[3]);
			else if(knownPlayer[message.Split(' ')[5]] != message.Split(' ')[3])
				knownPlayer[message.Split(' ')[5]] = message.Split(' ')[3];
			else
				return;
			saveUuids();
			if(onUuid != null)
				onUuid(message.Split(' ')[5], message.Split(' ')[3]);
		}

		public static void loadUuids()
		{
			knownPlayer = new System.Collections.Generic.Dictionary<string, string>();
			NINSS.API.Config config = new NINSS.API.Config("UUIDs");
			foreach(System.Xml.XmlNode node in config.rootNode.ChildNodes)
				knownPlayer.Add(node.Name, node.InnerText);
		}
		public static void saveUuids()
		{
			NINSS.API.Config config = new NINSS.API.Config("UUIDs");
			foreach(string key in knownPlayer.Keys)
				config.setValue(key, knownPlayer[key]);
			config.saveConfig("UUIDs");
		}
		
		public static string getUuidOf(string name)
		{
			foreach(string uuid in knownPlayer.Keys)
				if(knownPlayer[uuid] == name)
					return uuid;
			return null;
		}
	}
}