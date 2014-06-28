using System;
using NINSS;
using System.Linq;

namespace UuidConnector
{
	public class UuidConnector : INinssPlugin
	{
		public string Name { get { return "UuidConnector"; } }

		public static event MinecraftConnector.PlayerEvent onUuid;
		private static System.Collections.Generic.Dictionary<string, string> knownPlayer; //UUID, last name
		private object jsManager;
		public static System.Collections.Generic.Dictionary<string, string> KnownPlayer
		{
			get
			{
				if(knownPlayer == null)
					LoadUuids();
				return knownPlayer;
			}
			internal set
			{
				knownPlayer = value;
			}
		}
		public UuidConnector()
		{
			MinecraftConnector.messageReader.Add("UUID of player", ReadUuid);
			MinecraftConnector.ServerStop += SaveUuids;
			MinecraftConnector.ServerStart += OnStart;
			if(knownPlayer == null)
				LoadUuids();
		}
		public void OnStart()
		{
			if (NINSS.MainClass.pluginManager.Plugins.First(p => p.Name == "JavascriptConnector") != null)
			{
				object jsPlugin = NINSS.MainClass.pluginManager.Plugins.First(p => p.Name == "JavascriptConnector");
				jsManager = jsPlugin.GetType().GetField("manager").GetValue(jsPlugin);
				object jsContext = jsManager.GetType().GetField("javascriptContext").GetValue(jsManager);
				jsContext.GetType().GetMethod("SetParameter", new System.Type[] { typeof(string), typeof(object) }).Invoke(jsContext, new Object[] {
                    "Uuid",
					this
				});
                Console.WriteLine("Inserted 'Uuid' lib into JavascriptConnector");
			}
			else
			{
				Console.WriteLine("JavascriptConnector could not be found! Cannot insert UUID lib and event!");
			}
		}
		public void JsUuidJoin(string uuid, string name)
		{
			if(jsManager != null)
				jsManager.GetType().GetMethod("executeAll", new System.Type[] {typeof(string)}).Invoke(jsManager, new object[] {"OnUuid(\""+uuid+"\", \""+name+"\");"});
		}

		public static void ReadUuid(string message) //UUID of player <name> is <uuid>
		{
			if(!knownPlayer.ContainsKey(message.Split(' ')[5]))
				knownPlayer.Add(message.Split(' ')[5], message.Split(' ')[3]);
			else if(knownPlayer[message.Split(' ')[5]] != message.Split(' ')[3])
				knownPlayer[message.Split(' ')[5]] = message.Split(' ')[3];
			else
				return;

			SaveUuids();
			if(onUuid != null)
				onUuid(message.Split(' ')[5], message.Split(' ')[3]);
		}

		public static void LoadUuids()
		{
			knownPlayer = new System.Collections.Generic.Dictionary<string, string>();
			NINSS.API.Config config = new NINSS.API.Config("UUIDs");
			foreach (System.Xml.XmlNode node in config.rootNode.ChildNodes)
			{
				knownPlayer.Add(node.Name, node.InnerText);
			}
		}
		public static void SaveUuids()
		{
			NINSS.API.Config config = new NINSS.API.Config("UUIDs");
			foreach (string key in knownPlayer.Keys)
			{
				config.SetValue(key, knownPlayer [key]);
			}
			config.SaveConfig("UUIDs");
		}
		
		public static string GetUuidOf(string name)
		{
			foreach (string uuid in knownPlayer.Keys)
			{
				if (knownPlayer [uuid] == name)
				{
					return uuid;
				}
			}
			return null;
		}
	}
}