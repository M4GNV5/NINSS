using System;
using System.Xml;
namespace SimplePermissions
{
	public class SimplePermissions
	{
		XmlNode groupNode;
		XmlNode defaultGroup;
		public SimplePermissions ()
		{
			loadPermissions("Permissions");
			NINSS.MinecraftConnector.OnStart += onStart;
		}
		public void onStart()
		{
			if(NINSS.MainClass.pluginManager.plugins.ContainsKey("JavascriptConnector"))
			{
				object jsManager = NINSS.MainClass.pluginManager.plugins["JavascriptConnector"].GetType().GetField("manager").GetValue(NINSS.MainClass.pluginManager.plugins["JavascriptConnector"]);
				object jsContext = jsManager.GetType().GetField("javascriptContext").GetValue(jsManager);
				jsContext.GetType().GetMethod("SetParameter", new System.Type[] {typeof(string), typeof(object)}).Invoke(jsContext, new Object[] {"Permission", this});
				Console.WriteLine("Inserted 'Permission' lib into JavascriptConnector");
			}
			else
				Console.WriteLine("JavascriptConnector could not be found! Cannot insert UUID lib and event!");
		}
		public void loadPermissions(string name)
		{
			XmlDocument doc = new XmlDocument();
			if(!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory+"plugins\\configs\\"+name+".xml"))
				throw new Exception("No Permissions File found!\nPlease use and edit the one github!");
			doc.Load(AppDomain.CurrentDomain.BaseDirectory+"plugins\\configs\\"+name+".xml");
			foreach(XmlNode node in doc.ChildNodes)
				if(node.Name == "Permission_config")
					groupNode = node;
			foreach(XmlNode node in groupNode.ChildNodes)
				if(node.Attributes != null && node.Attributes.GetNamedItem("default") != null && node.Attributes.GetNamedItem("default").Value == "true")
					defaultGroup = node;
		}
		public bool hasPermission(string player, string permission)
		{
			foreach(XmlNode node in getGroupNode(player).ChildNodes)
				if(node.Name == "permission" && isPermittableFrom(permission, node.InnerText))
					return true;
			return false;
		}
		public string getGroup(string player)
		{
			foreach(XmlNode gNode in groupNode.ChildNodes)
				foreach(XmlNode node in gNode.ChildNodes)
					if(node.InnerText == player)
						return gNode.Attributes.GetNamedItem("name").Value;
			return defaultGroup.Attributes.GetNamedItem("name").Value;
		}
		public XmlNode getGroupNode(string player)
		{
			foreach(XmlNode gNode in groupNode.ChildNodes)
				foreach(XmlNode node in gNode.ChildNodes)
					if(node.InnerText.ToLower().Trim() == player.ToLower())
						return gNode;
			return defaultGroup;
		}
		public bool isPermittableFrom(string searchedPerm, string givenPerm)
		{
			string[] _searchedPerm = searchedPerm.Split('.');
			string[] _givenPerm = givenPerm.Split('.');
			for(int i = 0; i < _givenPerm.Length; i++)
			{
				if(_searchedPerm.Length < i)
					return false;
				else if(_searchedPerm[i] == _givenPerm[i] || _givenPerm[i] == "*")
					continue;
				else
					return false;
			}
			return true;
		}
	}
}