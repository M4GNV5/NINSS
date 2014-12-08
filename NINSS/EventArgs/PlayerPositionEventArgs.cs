using System;

namespace NINSS
{
	public class PlayerPositionEventArgs : PlayerEventArgs
	{
		public string Position { get; private set; }

		public PlayerPositionEventArgs(API.Player player, string position) : base(player)
		{
			this.Position = position;
		}
	}
}

