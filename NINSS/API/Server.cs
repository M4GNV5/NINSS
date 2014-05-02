using System;

namespace NINSS
{
	namespace API
	{
		/// <summary>
		/// Server class that contains Methods for different server related things
		/// </summary>
		public class Server
		{
			/// <summary>
			/// Broadcasts a json tellraw message to all players
			/// </summary>
			/// <param name="jsonMessage">Json message for tellraw command</param>
			public static void broadcastMessage(string jsonMessage)
			{
				MainClass.serverManager.writeMessage("tellraw @a "+jsonMessage);
			}
			/// <summary>
			/// Broadcasts a message to all Players
			/// </summary>
			/// <param name="message">message to broadcast</param>
			/// <param name="color">Color of message</param>
			public static void broadcastMessage(string message, string color)
			{
				Server.runCommand("tellraw @a {\"text\":\""+message+"\",\"color\":\""+color+"\"}");
			}
			/// <summary>
			/// Runs a command
			/// </summary>
			/// <param name="command">The command that should be run</param>
			/// <example>runCommand("kick Notch");</example>
			public static void runCommand(string command)
			{
				MainClass.serverManager.writeMessage(command);
			}
		}
	}
}