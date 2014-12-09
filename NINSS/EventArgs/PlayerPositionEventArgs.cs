using System;

namespace NINSS
{
	public class PlayerPositionEventArgs : PlayerEventArgs
	{
		public string Position { get; private set; }

		public PlayerPositionEventArgs(string player, string position) : base(player)
		{
			this.Position = position;
		}
	}
}

