using System;

namespace NINSS
{
	public class PlayerLeftEventArgs : PlayerEventArgs
	{
		public PlayerLeftEventArgs(API.Player player) : base(player)
		{
		}
	}
}

