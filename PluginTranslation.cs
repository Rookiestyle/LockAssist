using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using KeePass.Plugins;
using KeePass.Util;
using KeePassLib.Utility;

namespace PluginTranslation
{
	public static class PluginTranslate
	{
		#region Definitions of translated texts go here
		public const string PluginName = "LockAssist";
		public static string FirstTimeInfo
		{
			get { return TryGet("FirstTimeInfo", @"Quick Unlock offers two operation modes.
Please choose your preferred way of working."); }
		}
		public static string OptionsQUMode
		{
			get { return TryGet("OptionsQUMode", @"Mode:"); }
		}
		public static string Active
		{
			get { return TryGet("Active", @"Enable Quick Unlock"); }
		}
		public static string OptionsSLMinimize
		{
			get { return TryGet("OptionsSLMinimize", @"Softlock on minimize"); }
		}
		public static string KeyProvNoQuickUnlock
		{
			get { return TryGet("KeyProvNoQuickUnlock", @"No Quick Unlock key found. Quick Unlock is not possible."); }
		}
		public static string OptionsNotSavedSoftlock
		{
			get { return TryGet("OptionsNotSavedSoftlock", @"Softlock active. Settings have NOT been saved."); }
		}
		public static string OptionsQUReqInfoDB
		{
			get { return TryGet("OptionsQUReqInfoDB", @"Prerequsites for mode 'database password':
- Database masterkey contains a password
- Option 'Remember master password' is active

Quick Unlock entry will be used as fallback"); }
		}
		public static string ButtonCancel
		{
			get { return TryGet("ButtonCancel", @"Cancel"); }
		}
		public static string OptionsQUSettings
		{
			get { return TryGet("OptionsQUSettings", @"Quick Unlock"); }
		}
		public static string OptionsQUModeEntry
		{
			get { return TryGet("OptionsQUModeEntry", @"Quick Unlock entry only"); }
		}
		public static string OptionsQUModeDatabasePW
		{
			get { return TryGet("OptionsQUModeDatabasePW", @"Database password"); }
		}
		public static string OptionsSLInfo
		{
			get { return TryGet("OptionsSLInfo", @"Softlock hides following sensitive information while still allowing Auto-Type as well as other integration:
- group list
- entry list
- entry view
- all forms NOT mentioned in config file property QuickUnlock.SoftLock.ExcludeForms

Valid Quick Unlock settings required, Quick Unlock does NOT need to be active"); }
		}
		public static string OptionsQUEntryCreated
		{
			get { return TryGet("OptionsQUEntryCreated", @"Quick Unlock entry created. Please edit and set Quick Unlock PIN as password"); }
		}
		public static string OptionsSLSettings
		{
			get { return TryGet("OptionsSLSettings", @"Softlock"); }
		}
		public static string OptionsQUSettingsPerDB
		{
			get { return TryGet("OptionsQUSettingsPerDB", @"Settings are DB specific"); }
		}
		public static string SoftlockModeUnhide
		{
			get { return TryGet("SoftlockModeUnhide", @"Softlock active. Click to deactivate"); }
		}
		public static string OptionsSwitchDBToGeneral
		{
			get { return TryGet("OptionsSwitchDBToGeneral", @"Database specific settings switched off.
Revert back to general settings?
'No' will set current settings as global settings"); }
		}
		public static string OptionsQUPINLength
		{
			get { return TryGet("OptionsQUPINLength", @"PIN length:"); }
		}
		public static string ButtonUnlock
		{
			get { return TryGet("ButtonUnlock", @"Unlock"); }
		}
		public static string UnlockLabel
		{
			get { return TryGet("UnlockLabel", @"Quick Unlock PIN:"); }
		}
		public static string OptionsQUEntryCreate
		{
			get { return TryGet("OptionsQUEntryCreate", @"Quick Unlock entry could not be found. Create it now?"); }
		}
		public static string KeyProvNoCreate
		{
			get { return TryGet("KeyProvNoCreate", @"This key provider cannot be used to create keys."); }
		}
		public static string OptionsSLSecondsToActivate
		{
			get { return TryGet("OptionsSLSecondsToActivate", @"Softlock after inactivity (seconds):"); }
		}
		public static string ButtonOK
		{
			get { return TryGet("ButtonOK", @"OK"); }
		}
		public static string OptionsQUInfoRememberPassword
		{
			get { return TryGet("OptionsQUInfoRememberPassword", @"'Remember master password' needs to be active in Options -> Security.
Please don't forget to activate"); }
		}
		public static string OptionsQUUseLast
		{
			get { return TryGet("OptionsQUUseLast", @"Use last {0} characters as PIN"); }
		}
		public static string OptionsQUUseFirst
		{
			get { return TryGet("OptionsQUUseFirst", @"Use first {0} characters as PIN"); }
		}
		public static string SoftlockModeUnhideForms
		{
			get { return TryGet("SoftlockModeUnhideForms", @"Softlock active. Click topmost form to deactivate"); }
		}
		public static string WrongPIN
		{
			get { return TryGet("WrongPIN", @"The entered PIN was not correct.
Database stays locked and can only be unlocked with the original masterkey"); }
		}
		public static string OptionsLockWorkspace
		{
			get { return TryGet("OptionsLockWorkspace", @"Global '{0} / {1}'"); }
		}
		public static string OptionsLockWorkspaceDesc
		{
			get { return TryGet("OptionsLockWorkspaceDesc", @"This option changes the behaviour of 'Lock Workspace' for both the menu entry as well as the toolbar button.

If it's active ALL loaded databases are locked / unlocked by using these commands.
In this case it depends on the active document's state whether a global lock or global unlock is performed.

If the [Shift] key is pressed while using these commands only the active document is processed."); }
		}
		#endregion

		#region NO changes in this area
		private static bool Debug = KeePass.Program.CommandLineArgs[KeePass.App.AppDefs.CommandLineOptions.Debug] != null;
		private static StringDictionary m_translation = new StringDictionary();

		public static void Init(Plugin plugin, string LanguageCodeIso6391)
		{
			try
			{
				string filename = GetFilename(plugin.GetType().Namespace, LanguageCodeIso6391);

				string translation = File.ReadAllText(filename);

				XmlSerializer xs = new XmlSerializer(m_translation.GetType());
				m_translation = (StringDictionary)xs.Deserialize(new StringReader(translation));
			}
			catch (Exception) { }
		}

		public static string TryGet(string key, string def)
		{
			string result = string.Empty;
			if (m_translation.TryGetValue(key, out result))
				return result;
			else
				return def;
		}

		private static string GetFilename(string plugin, string lang)
		{
			string filename = UrlUtil.GetFileDirectory(WinUtil.GetExecutable(), true, true);
			filename += KeePass.App.AppDefs.PluginsDir + UrlUtil.LocalDirSepChar + "Translations" + UrlUtil.LocalDirSepChar;
			filename += plugin + "." + lang + ".language.xml";
			return filename;
		}
		#endregion
	}

	#region NO changes in this area
	[XmlRoot("Translation")]
	public class StringDictionary : Dictionary<string, string>, IXmlSerializable
	{
		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			bool wasEmpty = reader.IsEmptyElement;
			reader.Read();
			if (wasEmpty) return;
			while (reader.NodeType != XmlNodeType.EndElement)
			{
				reader.ReadStartElement("item");
				reader.ReadStartElement("key");
				string key = reader.ReadContentAsString();
				reader.ReadEndElement();
				reader.ReadStartElement("value");
				string value = reader.ReadContentAsString();
				reader.ReadEndElement();
				this.Add(key, value);
				reader.ReadEndElement();
				reader.MoveToContent();
			}
			reader.ReadEndElement();
		}

		public void WriteXml(XmlWriter writer)
		{
			foreach (string key in this.Keys)
			{
				writer.WriteStartElement("item");
				writer.WriteStartElement("key");
				writer.WriteString(key);
				writer.WriteEndElement();
				writer.WriteStartElement("value");
				writer.WriteString(this[key]);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
		}
	}
	#endregion
}