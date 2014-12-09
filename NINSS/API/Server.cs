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
			public static Server Instance { get; private set; }
			static Server()
			{
				Instance = new Server(MainClass.serverManager);
			}
			private Server(ServerManager manager)
			{
				this.manager = manager;
			}

			private ServerManager manager;

			/// <summary>
			/// Broadcasts a json tellraw message to all players
			/// </summary>
			/// <param name="jsonMessage">Json message for tellraw command</param>
			public void BroadcastMessage(string jsonMessage)
			{
				this.RunCommand("tellraw @a "+jsonMessage);
			}
			/// <summary>
			/// Broadcasts a message to all Players
			/// </summary>
			/// <param name="message">message to broadcast</param>
			/// <param name="color">Color of message</param>
			public void BroadcastMessage(string message, string color)
			{
				this.RunCommand("tellraw @a {\"text\":\""+message+"\",\"color\":\""+color+"\"}");
			}
			/// <summary>
			/// Runs a command
			/// </summary>
			/// <param name="command">The command that should be run</param>
			/// <example>runCommand("kick Notch");</example>
			public void RunCommand(string command)
			{
				manager.WriteLine(command);
			}
		}
	}
}