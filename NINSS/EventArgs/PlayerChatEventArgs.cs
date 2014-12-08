using System;

namespace NINSS
{
	public class PlayerChatEventArgs : PlayerEventArgs
	{
		public string Message { get; private set; }

		public PlayerChatEventArgs (API.Player player, string message) : base(player)
		{
			this.Message = message;
		}
	}
}

