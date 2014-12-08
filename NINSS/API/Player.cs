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
			public string Name {get; internal set;}
			/// <summary>
			/// Last known position of the current player
			/// </summary>
			public string Position {get; internal set;}
			public Player ()
			{}
			public Player(string _name)
			{
				Name = _name.Trim();
			}
			public Player (string _name, string _position)
			{
				Name = _name;
				Position = _position;
			}
			/// <summary>
			/// Sends a json message to the current player
			/// </summary>
			/// <param name="jsonMessage">Json message</param>
			public void SendMessage(string jsonMessage)
			{
				SendMessageTo(Name, jsonMessage);
			}
			/// <summary>
			/// Sends a message with the specific color to the current player
			/// </summary>
			/// <param name="message">Message</param>
			/// <param name="color">Color</param>
			public void SendMessage(string message, string color)
			{
				SendMessageTo(Name, message, color);
			}
			/// <summary>
			/// Sends a json message to the named player
			/// </summary>
			/// <param name="_name">_name of the player</param>
			/// <param name="jsonMessage">Json message</param>
			public static void SendMessageTo(string name, string jsonMessage)
			{
				Server.RunCommand("tellraw "+name+" "+jsonMessage);
			}
			/// <summary>
			/// Sends a message with the specific color to the named player
			/// </summary>
			/// <param name="_name">name of the player</param>
			/// <param name="message">Messag.</param>
			/// <param name="color">Color</param>
			public static void SendMessageTo(string name, string message, string color)
			{
				Server.RunCommand("tellraw "+name+" {\"text\":\""+message+"\",\"color\":\""+color+"\"}");
			}
			/// <summary>
			/// Calls onPosition with the new Player position
			/// </summary>
			public void RefreshPosition()
			{
				Server.RunCommand("tp "+Name+" ~ ~ ~");
			}
			/// <summary>
			/// Calls onPosition with the new Player position
			/// </summary>
			public static void RefreshPosition(string _name)
			{
				Server.RunCommand("tp "+_name+" ~ ~ ~");
			}
		}
	}
}