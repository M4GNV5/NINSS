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
			public string getValue(string name)
			{
				if(doc.GetElementsByTagName(name)[0] != null)
					return doc.GetElementsByTagName(name)[0].InnerText;
				else
					return null;
			}
			public void setValue(string name, string value)
			{
				if(doc.GetElementsByTagName(name)[0] == null)
					rootNode.AppendChild(doc.CreateElement(name));
				doc.GetElementsByTagName(name)[0].InnerText = value;
			}
			public void createNewConfig()
			{
				doc = new XmlDocument();
				doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));
				rootNode = doc.CreateElement("root");
				doc.AppendChild(rootNode);
			}
			public void saveConfig(string file)
			{
				doc.Save(file);
			}
			public void loadConfig(string file)
			{
				doc.Load(file);
			}
		}
	}
}

