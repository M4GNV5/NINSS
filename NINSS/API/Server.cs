using System;

namespace NINSS
{
	namespace API
	{
		public class Server
		{
			public static void broadcastMessage(string jsonMessage)
			{
				MainClass.serverManager.writeMessage("tellraw @a "+jsonMessage);
			}
			public static void broadcastMessage(string message, string color)
			{
				Server.runCommand("tellraw @a {\"text\":\""+message+"\",\"color\":\""+color+"\"}");
			}
			public static void runCommand(string command)
			{
				MainClass.serverManager.writeMessage(command);
			}
		}
	}
}