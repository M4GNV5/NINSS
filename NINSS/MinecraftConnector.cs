using System;
using System.Text;

namespace NINSS
{
	/// <summary>
	/// Class that handles Server output and translate it to events
	/// </summary>
	public class MinecraftConnector
	{
		public delegate void PlayerEvent(string Player, string args);
		/// <summary>
		/// Occurs when a player joins
		/// </summary>
		public static event PlayerEvent PlayerJoin;
		/// <summary>
		/// Occurs when a player leaves
		/// </summary>
		public static event PlayerEvent PlayerLeave;
		/// <summary>
		/// Occurs when a player is teleported to coordinates
		/// </summary>
		public static event PlayerEvent PlayerPosition;
		/// <summary>
		/// Occurs when a player says something in chat
		/// </summary>
		public static event PlayerEvent ChatReceived;
		/// <summary>
		/// Occurs when a player writes a command in chat
		/// </summary>
		public static event PlayerEvent OnCommand;

		public delegate void ServerEvent();
		/// <summary>
		/// Occurs when the server starts
		/// </summary>
		public static event ServerEvent ServerStart;
		/// <summary>
		/// Occurs when on stops
		/// </summary>
		public static event	ServerEvent ServerStop;

		public delegate void messageRead(string message);
		/// <summary>
		/// List of all methods that will be invoked when a server output contains the specific key string
		/// </summary>
		public static System.Collections.Generic.Dictionary<string, messageRead> messageReader = new System.Collections.Generic.Dictionary<string, messageRead>
		{
			{"joined the game", ReadJoin},
			{"left the game", ReadLeft},
			{"<", ReadChat},
			{"Done", ReadStart},
			{"Stopping the server", ReadStop},
			{"Teleported", ReadPosition}
		};
		public static void OnServerMessage(string message)
		{
			StringBuilder sb = new StringBuilder (message);
			sb.Remove(0, 12);
            sb.Replace(sb.ToString().Split(':')[0], "");
			message = sb.ToString().Trim(':', ' ', '[', ']');
			foreach (string readerKey in messageReader.Keys)
			{
				if (message.Contains(readerKey))
				{
					messageReader [readerKey](message);
				}
			}
		}
		
		public static void ReadPosition(string message)
		{
			API.Player.onlinePlayer[message.Split(' ')[1]].Position = message.Split(' ')[3].Replace(",", " ");
			if(PlayerPosition != null)
				PlayerPosition(message.Split(' ')[1], message.Split(' ')[3].Replace(",", " "));
		}
		public static void ReadJoin(string message)
		{
			API.Player.onlinePlayer.Add(message.Split(' ')[0], new API.Player(message.Split(' ')[0]));
			if(PlayerJoin != null)
				PlayerJoin(message.Split(' ')[0], null);
		}
		public static void ReadLeft(string message)
		{
			API.Player.onlinePlayer.Remove(message.Split(' ')[0]);
			if(PlayerJoin != null)
				PlayerLeave(message.Split(' ')[0], null);
		}
		public static void ReadChat(string message)
		{
			string player = message.Split('<', '>')[1].Trim();
			string chat = message.Replace("<"+message.Split('<', '>')[1]+">", "").Trim();
			if(ChatReceived != null)
				ChatReceived(player, chat);
			if(chat.Substring(0, 1) == "!")
				OnCommand(player, chat.TrimStart('!'));
		}
		public static void ReadStart(string message)
		{
			if(ServerStart != null)
				ServerStart();
		}
		public static void ReadStop(string message)
		{
			if(ServerStop != null)
				ServerStop();
		}
	}
}

