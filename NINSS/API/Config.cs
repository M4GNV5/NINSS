using System;
using System.Xml;
namespace NINSS
{
	namespace API
	{
		/// <summary>
		/// Config class that allows you to get and set values of simple XML configuration files
		/// </summary>
		public class Config
		{
			public XmlDocument doc;
			public XmlNode rootNode;
			public Config()
			{
				doc = new XmlDocument();
			}
			public Config (string name)
			{
				doc = new XmlDocument();
				loadConfig(name);
				rootNode = doc.GetElementsByTagName("root")[0];
			}
			/// <summary>
			/// Get a value from the configuration file
			/// </summary>
			/// <returns>The value or null if config does not contain the named node</returns>
			/// <param name="name">Name of node</param>
			public string getValue(string name)
			{
				if(doc.GetElementsByTagName(name)[0] != null)
					return doc.GetElementsByTagName(name)[0].InnerText;
				else
					return null;
			}
			public string[] getValues()
			{
				System.Collections.Generic.List<string> values = new System.Collections.Generic.List<string>();
				foreach(XmlNode node in rootNode.ChildNodes)
					if(node.NodeType == XmlNodeType.Element)
						values.Add(node.Name);
				return values.ToArray();
			}
			/// <summary>
			/// Sets the value of a node or creates a new node in the configuration file
			/// </summary>
			/// <param name="name">Name.</param>
			/// <param name="value">Value.</param>
			public void setValue(string name, string value)
			{
				if(doc.GetElementsByTagName(name)[0] == null)
					rootNode.AppendChild(doc.CreateElement(name));
				doc.GetElementsByTagName(name)[0].InnerText = value;
			}
			/// <summary>
			/// Creates a clean new config
			/// </summary>
			public void createNewConfig()
			{
				doc = new XmlDocument();
				doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));
				rootNode = doc.CreateElement("root");
				doc.AppendChild(rootNode);
			}
			/// <summary>
			/// Saves the config
			/// </summary>
			/// <param name="file">Config name</param>
			public void saveConfig(string name)
			{
				doc.Save(AppDomain.CurrentDomain.BaseDirectory+"plugins\\configs\\"+name+".xml");
			}
			/// <summary>
			/// Loads a config
			/// </summary>
			/// <param name="file">Config name</param>
			public void loadConfig(string name)
			{
				if(!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory+"plugins\\configs\\"+name+".xml"))
				{
					createNewConfig();
					saveConfig(name);
				}
				else
					doc.Load(AppDomain.CurrentDomain.BaseDirectory+"plugins\\configs\\"+name+".xml");
			}
		}
	}
}

