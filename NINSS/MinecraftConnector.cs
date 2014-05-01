using System;
namespace NINSS
{
	public class MinecraftConnector
	{
		public delegate void PlayerEvent(string Player, string args);
		public static event PlayerEvent OnPlayerJoin;
		public static event PlayerEvent OnPlayerLeave;
		public static event PlayerEvent OnPlayerPosition;
		public static event PlayerEvent OnChat;
		public static event PlayerEvent OnCommand;

		public delegate void ServerEvent();
		public static event ServerEvent OnStart;
		public static event	ServerEvent OnStop;

		public delegate void messageRead(string message);
		public static System.Collections.Generic.Dictionary<string, messageRead> messageReader = new System.Collections.Generic.Dictionary<string, messageRead>
		{
			{"joined the game", readJoin},
			{"left the game", readLeft},
			{"<", readChat},
			{"Done", readStart},
			{"Stopping the server", readStop},
			{"Teleported", readPosition}
		};
		public static void OnServerMessage(string message)
		{
			message = message.Remove(0, 12);
			message = message.Replace(message.Split(':')[0], "");
			message = message.Trim(':', ' ', '[', ']');
			foreach(string readerKey in messageReader.Keys)
				if(message.Contains(readerKey))
					messageReader[readerKey](message);
		}
		
		public static void readPosition(string message)
		{
			API.Player.onlinePlayer[message.Split(' ')[1]].position = message.Split(' ')[3].Replace(",", " ");
			if(OnPlayerPosition != null)
				OnPlayerPosition(message.Split(' ')[1], message.Split(' ')[3].Replace(",", " "));
		}
		public static void readJoin(string message)
		{
			API.Player.onlinePlayer.Add(message.Split(' ')[0], new API.Player(message.Split(' ')[0]));
			if(OnPlayerJoin != null)
				OnPlayerJoin(message.Split(' ')[0], null);
		}
		public static void readLeft(string message)
		{
			API.Player.onlinePlayer.Remove(message.Split(' ')[0]);
			if(OnPlayerJoin != null)
				OnPlayerLeave(message.Split(' ')[0], null);
		}
		public static void readChat(string message)
		{
			string player = message.Split('<', '>')[1].Trim();
			string chat = message.Replace("<"+message.Split('<', '>')[1]+">", "").Trim();
			if(OnChat != null)
				OnChat(player, chat);
			if(chat.Substring(0, 1) == "!")
				OnCommand(player, chat.TrimStart('!'));
		}
		public static void readStart(string message)
		{
			if(OnStart != null)
				OnStart();
		}
		public static void readStop(string message)
		{
			if(OnStop != null)
				OnStop();
		}
	}
}

