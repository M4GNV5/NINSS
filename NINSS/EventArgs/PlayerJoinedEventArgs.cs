using System;

namespace NINSS
{
	public class PlayerJoinedEventArgs : PlayerEventArgs
	{
		public PlayerJoinedEventArgs(API.Player player) : base(player)
		{
		}
	}
}

