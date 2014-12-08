using System;

namespace NINSS
{
	public abstract class PlayerEventArgs : EventArgs
	{
		public API.Player Player { get; protected set; }

		public PlayerEventArgs (API.Player player)
		{
			this.Player = player;
		}
	}
}

