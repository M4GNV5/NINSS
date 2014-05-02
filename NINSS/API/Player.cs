using System;
namespace NINSS
{
	namespace API
	{
		/// <summary>
		/// Player class that contains Methods for different player related things
		/// </summary>
		public class Player
		{
			/// <summary>
			/// List of all online Players sorted by names
			/// </summary>
			public static System.Collections.Generic.Dictionary<string, Player> onlinePlayer = new System.Collections.Generic.Dictionary<string, Player>();
			/// <summary>
			/// Name of the current player
			/// </summary>
			public string name {get; internal set;}
			/// <summary>
			/// Last known position of the current player
			/// </summary>
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
			/// <summary>
			/// Sends a json message to the current player
			/// </summary>
			/// <param name="jsonMessage">Json message</param>
			public void sendMessage(string jsonMessage)
			{
				sendMessageTo(name, jsonMessage);
			}
			/// <summary>
			/// Sends a message with the specific color to the current player
			/// </summary>
			/// <param name="message">Message</param>
			/// <param name="color">Color</param>
			public void sendMessage(string message, string color)
			{
				sendMessageTo(name, message, color);
			}
			/// <summary>
			/// Sends a json message to the named player
			/// </summary>
			/// <param name="_name">_name of the player</param>
			/// <param name="jsonMessage">Json message</param>
			public static void sendMessageTo(string _name, string jsonMessage)
			{
				Server.runCommand("tellraw "+_name+" "+jsonMessage);
			}
			/// <summary>
			/// Sends a message with the specific color to the named player
			/// </summary>
			/// <param name="_name">name of the player</param>
			/// <param name="message">Messag.</param>
			/// <param name="color">Color</param>
			public static void sendMessageTo(string _name, string message, string color)
			{
				Server.runCommand("tellraw "+_name+" {\"text\":\""+message+"\",\"color\":\""+color+"\"}");
			}
			/// <summary>
			/// Calls onPosition with the new Player position
			/// </summary>
			public void refreshPosition()
			{
				Server.runCommand("tp "+name+" ~ ~ ~");
			}
			/// <summary>
			/// Calls onPosition with the new Player position
			/// </summary>
			public static void refreshPosition(string _name)
			{
				Server.runCommand("tp "+_name+" ~ ~ ~");
			}
		}
	}
}