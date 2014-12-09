using System;
using System.Text;

using NINSS;
namespace JavascriptConnector
{
	public class JavascriptConnector : INinssPlugin
	{
		public string Name { get { return "JavascriptConnector"; } }

		private JavascriptPluginManager manager;

		public JavascriptConnector()
		{
			manager = new JavascriptPluginManager();
			MinecraftConnector.PlayerJoin += PlayerJoin;
			MinecraftConnector.PlayerLeft += PlayerLeft;
			MinecraftConnector.PlayerPositionReceived += PlayerPosition;
			MinecraftConnector.PlayerChatReceived += PlayerChat;
			
			MinecraftConnector.ServerStart += ServerStart;
			MinecraftConnector.ServerStop += ServerStop;
		}
		
		private void PlayerJoin(object sender, PlayerJoinedEventArgs e)
		{
			TriggerEvent("PlayerJoin", e.Player);
		}
		private void PlayerLeft(object sender, PlayerLeftEventArgs e)
		{
			TriggerEvent("PlayerLeft", e.Player);
		}
		private void PlayerPosition(object sender, PlayerPositionEventArgs e)
		{
			TriggerEvent("PlayerPosition", e.Player, e.Position);
		}
		private void PlayerChat(object sender, PlayerChatEventArgs e)
		{
			TriggerEvent("ChatReceived", e.Player, e.Message);
		}
		
		private void ServerStart(object sender, ServerEventArgs e)
		{
			TriggerEvent("ServerStart");
		}
		private void ServerStop(object sender, ServerEventArgs e)
		{
			TriggerEvent("ServerStop");
		}

		private void TriggerEvent(string name, params object[] args)
		{
			foreach (JavascriptPlugin plugin in manager.Plugins)
			{
				for(int i = 0; i < args.Length; i++)
				{
					plugin.Runtime.SetParameter("arg" + i, args [i]);
				}
				StringBuilder sb = new StringBuilder ();
				sb.Append("if(typeof ");
				sb.Append(name);
				sb.Append(" != 'undefined') ");
				sb.Append(name);
				string s = sb.ToString();
				sb.Append("(");
				for (int i = 0; i < args.Length; i++)
				{
					sb.Append("arg");
					sb.Append(i);
					if(i != args.Length -1)
						sb.Append(", ");
				}
				sb.Append(");");

				string source = sb.ToString();
				plugin.Run(source);
			}
		}
	}
}

