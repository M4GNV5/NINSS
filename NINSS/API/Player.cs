using System;
namespace NINSS
{
	namespace API
	{
		public class Player
		{
			public static System.Collections.Generic.Dictionary<string, Player> onlinePlayer = new System.Collections.Generic.Dictionary<string, Player>();
			public string name {get; internal set;}
			public string position {get; internal set;}
			public Player ()
			{}
			public Player(string _name)
			{
				name = _name;
			}
			public Player (string _name, string _position)
			{
				name = _name;
				position = _position;
			}

			public void sendMessage(string jsonMessage)
			{
				sendMessageTo(name, jsonMessage);
			}
			public void sendMessage(string message, string color)
			{
				sendMessageTo(name, message, color);
			}
			public static void sendMessageTo(string _name, string jsonMessage)
			{
				Server.runCommand("tellraw "+_name+" "+jsonMessage);
			}
			public static void sendMessageTo(string _name, string message, string color)
			{
				Server.runCommand("tellraw "+_name+" {\"text\":\""+message+"\",\"color\":\""+color+"\"}");
			}
			public void refreshPosition()
			{
				Server.runCommand("tp "+name+" ~ ~ ~");
			}
			public static void refreshPosition(string _name)
			{
				Server.runCommand("tp "+_name+" ~ ~ ~");
			}
		}
	}
}