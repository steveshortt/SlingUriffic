using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SlingUriffic
{
	static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			string appDataPath = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + "\\SlingUriffic";
			string settingsFile = appDataPath + "\\settings.txt";

			if( args.Length > 0 )
			{
				if( !Directory.Exists( appDataPath ) )
				{
					Directory.CreateDirectory( appDataPath );
				}

				switch( args[0].ToLower() )
				{
					case "/settings":
					case "-settings":
					{
						EditSettingsFile( settingsFile );
						break;
					}
					case "/setup":
					case "-setup":
					{
						CreateSetupScript( appDataPath );
						break;
					}
					case "/?":
					case "-?":
					case "/help":
					case "-help":
					{
						ShowHelp();
						break;
					}
					default:
					{
						SlingUri( settingsFile, args[0] );
						break;
					}
				}
			}
			else
			{
				ShowHelp();
			}
		}

		private static void EditSettingsFile(string settingsFile)
		{
			if( !File.Exists( settingsFile ) )
			{
				Settings settings = new Settings();

				Browser ff = new Browser()
				{
					Name = "FF",
					Path = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe",
				};
				Pattern moz = new Pattern()
				{
					BrowserName = "FF",
					Match = "mozilla.com"
				};
				Browser ie = new Browser()
				{
					Name = "IE",
					Path = @"C:\Program Files (x86)\Internet Explorer\iexplore.exe",
					Arguments = "-nohome",
					IsDefault = true
				};
				Pattern ms = new Pattern()
				{
					BrowserName = "IE",
					Match = "microsoft.com"
				};

				settings.Browsers.Add( ff );
				settings.Patterns.Add( moz );
				settings.Browsers.Add( ie );
				settings.Patterns.Add( ms );
				settings.Serialize( settingsFile );
			}

			StartProcess( "notepad.exe", settingsFile );
		}

		private static void CreateSetupScript(string appDataPath)
		{
			string assmPath =
				Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ).Replace( @"\", @"\\" );
			string hkcu =
				Regex.Replace( SlingUriffic.Properties.Resources.setup, @"\[install_path\]", assmPath );

			using( StreamWriter setup_outfile =
				new StreamWriter( appDataPath + "\\setup.reg" ) )
			{
				setup_outfile.WriteLine( hkcu );
			}

			StartProcess( "explorer.exe", appDataPath );
		}

		private static void SlingUri(string settingsFile, string uri)
		{
			Settings settings = settings = Settings.Deserialize( settingsFile );
			string browser = settings.DefaultBrowser.Path;
			string arguments = string.Empty;

			foreach( Pattern pattern in settings.Patterns )
			{
				if( Regex.Match( uri, pattern.Match ).Success )
				{
					if( settings.BrowserList.ContainsKey( pattern.BrowserName ) )
					{
						browser = settings.BrowserList[pattern.BrowserName].Path;
						arguments = settings.BrowserList[pattern.BrowserName].Arguments;
						break;
					}
				}
			}

			//if( File.Exists( browser ) ) { }
			if( !string.IsNullOrEmpty( arguments ) )
			{
				arguments = string.Format( "{0} {1}", arguments, uri );
			}
			else
			{
				arguments = uri;
			}

			StartProcess( browser, arguments );
		}

		private static void StartProcess(string fileName, string arguments)
		{
			ProcessStartInfo psi = new ProcessStartInfo( fileName, arguments );
			Process p = new Process();
			p.StartInfo = psi;
			p.Start();
		}

		private static void ShowHelp()
		{
			string message =
@"Usage: SlingUriffic [option]
Options:
  /?		Show help
  /settings		Create/edit settings
  /setup		Generate setup file";

			System.Windows.Forms.MessageBox.Show(
				message, "SlingUriffic",
				System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information );
		}
	}
}