using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SlingUriffic
{
	public class Settings
	{
		public Settings()
		{
			Browsers = new List<Browser>();
			BrowserList = new Dictionary<string, Browser>();
			Patterns = new List<Pattern>();
		}

		public List<Browser> Browsers { get; set; }
		public List<Pattern> Patterns { get; set; }

		internal Dictionary<string, Browser> BrowserList { get; set; }
		internal Browser DefaultBrowser { get; set; }


		public void Serialize(string filePath)
		{
			XmlSerializer s = new XmlSerializer( typeof( Settings ) );
			XmlTextWriter w = new XmlTextWriter( filePath, Encoding.ASCII );
			w.Formatting = Formatting.Indented;
			s.Serialize( w, this );
			w.Close();
		}

		public static Settings Deserialize(string filePath)
		{
			Settings settings = new Settings();
			using( FileStream fs = new FileStream( filePath, FileMode.Open, FileAccess.Read ) )
			{
				XmlSerializer s = new XmlSerializer( typeof( Settings ) );
				settings = (Settings)s.Deserialize( fs );
			}

			foreach( Browser browser in settings.Browsers )
			{
				settings.BrowserList[browser.Name] = browser;
				if( browser.IsDefault )
				{
					settings.DefaultBrowser = browser;
				}
			}

			return settings;
		}
	}

	public class Browser
	{
		public Browser()
		{
			IsDefault = false;
		}

		[XmlAttribute()]
		public string Name { get; set; }
		[XmlAttribute()]
		public string Path { get; set; }
		[XmlAttribute()]
		public string Arguments { get; set; }
		[XmlAttribute()]
		public bool IsDefault { get; set; }
	}

	public class Pattern
	{
		[XmlAttribute()]
		public string BrowserName { get; set; }
		[XmlAttribute()]
		public string Match { get; set; }
	}
}