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
			public static Player Instance { get; private set; }
			static Player()
			{
				Instance = new Player();
			}
			private Player()
			{ }

			/// <summary>
			/// Sends a json message to the named player
			/// </summary>
			/// <param name="_name">_name of the player</param>
			/// <param name="jsonMessage">Json message</param>
			public static void SendMessageTo(string name, string jsonMessage)
			{
				Server.Instance.RunCommand("tellraw "+name+" "+jsonMessage);
			}
			/// <summary>
			/// Sends a message with the specific color to the named player
			/// </summary>
			/// <param name="_name">name of the player</param>
			/// <param name="message">Messag.</param>
			/// <param name="color">Color</param>
			public static void SendMessageTo(string name, string message, string color)
			{
				Server.Instance.RunCommand("tellraw "+name+" {\"text\":\""+message+"\",\"color\":\""+color+"\"}");
			}
			/// <summary>
			/// Calls onPosition with the new Player position
			/// </summary>
			public static void RefreshPosition(string name)
			{
				Server.Instance.RunCommand("tp "+name+" ~ ~ ~");
			}
		}
	}
}