using System;
using System.Diagnostics;

namespace NINSS
{
	public class MinecraftConnector
	{
		private ServerManager serverManager;

		public MinecraftConnector(ServerManager serverManager)
		{
			this.serverManager = serverManager;
			serverManager.OutputReceived += ReadServerOutput;
		}

		private void ReadServerOutput(object sender, DataReceivedEventArgs e)
		{
			string data = e.Data;
			string info = e.Data.Substring(11);
			string message = info.Remove(0, info.IndexOf(":")+1);

			if (message.Contains("Starting minecraft server"))
				TriggerEvent<ServerEventArgs>(ServerStart, new ServerEventArgs());
			if (data.Contains("[Server Shutdown Thread/INFO]: Stopping server"))
				TriggerEvent<ServerEventArgs>(ServerStop, new ServerEventArgs());

			if (message.Contains("joined the game"))
			{
				string name = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) [0];
				API.Player player = new API.Player (name);
				PlayerJoinedEventArgs eventArgs = new PlayerJoinedEventArgs (player);

				TriggerEvent<PlayerJoinedEventArgs>(PlayerJoin, eventArgs);
			}
			if (message.Contains("left the game"))
			{
				string name = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) [0];
				API.Player player = new API.Player (name);
				PlayerLeftEventArgs eventArgs = new PlayerLeftEventArgs (player);

				TriggerEvent<PlayerLeftEventArgs>(PlayerLeft, eventArgs);
			}

			if (message.Contains("<") && message.Contains(">"))
			{
				string[] split = message.Split(new char[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
				string name = split [1];
				string chat = split[2].Trim();
				API.Player player = new API.Player(name);

				PlayerChatEventArgs eventArgs = new PlayerChatEventArgs (player, chat);
				TriggerEvent<PlayerChatEventArgs>(PlayerChatReceived, eventArgs);
			}
			if (!message.Contains("<") && message.Contains("Teleported"))
			{
				string[] split = message.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
				string name = split [1];
				API.Player player = new API.Player (name);
				string position = split [split.Length - 3] + " " + split [split.Length - 2] + " " + split [split.Length - 1];

				PlayerPositionEventArgs eventArgs = new PlayerPositionEventArgs (player, position);
				TriggerEvent<PlayerPositionEventArgs>(PlayerPositionReceived, eventArgs);
			}
		}
		private void TriggerEvent<T>(EventHandler<T> ev, T args)
			where T : EventArgs
		{
			if (ev != null)
			{
				try
				{
					ev(this, args);
				}
				catch (Exception e)
				{
					Console.WriteLine("Error triggering event {0}: {1}\nMessage: {2}\nStacktrace: {3}", ev.GetType(), e.GetType(), e.Message, e.StackTrace);
				}
			}
		}

		public static event EventHandler<PlayerJoinedEventArgs> PlayerJoin;
		public static event EventHandler<PlayerLeftEventArgs> PlayerLeft;
		public static event EventHandler<PlayerChatEventArgs> PlayerChatReceived;
		public static event EventHandler<PlayerPositionEventArgs> PlayerPositionReceived;

		public static event EventHandler<ServerEventArgs> ServerStart;
		public static event EventHandler<ServerEventArgs> ServerStop;
	}
}

