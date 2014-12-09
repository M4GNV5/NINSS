using System;

namespace NINSS
{
	public class PlayerJoinedEventArgs : PlayerEventArgs
	{
		public PlayerJoinedEventArgs(string player) : base(player)
		{
		}
	}
}

