using System;
using System.Xml;
namespace NINSS
{
	namespace API
	{
		public class Config
		{
			XmlDocument doc;
			XmlNode rootNode;
			public Config (string file)
			{
				doc = new XmlDocument();
				if(!System.IO.File.Exists(file))
				{
					createNewConfig();
					saveConfig(file);
				}
				doc.Load(file);
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
			/// <param name="file">File path</param>
			public void saveConfig(string file)
			{
				doc.Save(file);
			}
			/// <summary>
			/// Loads a config
			/// </summary>
			/// <param name="file">File path</param>
			public void loadConfig(string file)
			{
				doc.Load(file);
			}
		}
	}
}

