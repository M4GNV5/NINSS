using System;

namespace NINSS
{
	public abstract class PlayerEventArgs : EventArgs
	{
		public string Player { get; protected set; }

		public PlayerEventArgs (string player)
		{
			this.Player = player;
		}
	}
}

