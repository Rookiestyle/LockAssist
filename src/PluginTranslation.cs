using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using KeePass.Plugins;
using KeePass.Util;
using KeePassLib.Utility;
using PluginTools;

namespace PluginTranslation
{
  public class TranslationChangedEventArgs : EventArgs
  {
    public string OldLanguageIso6391 = string.Empty;
    public string NewLanguageIso6391 = string.Empty;

    public TranslationChangedEventArgs(string OldLanguageIso6391, string NewLanguageIso6391)
    {
      this.OldLanguageIso6391 = OldLanguageIso6391;
      this.NewLanguageIso6391 = NewLanguageIso6391;
    }
  }

  public static class PluginTranslate
  {
    public static long TranslationVersion = 0;
    public static event EventHandler<TranslationChangedEventArgs> TranslationChanged = null;
    private static string LanguageIso6391 = string.Empty;
    #region Definitions of translated texts go here
    public const string PluginName = "Lock Assist";
    /// <summary>
    /// Quick Unlock offers two operation modes.
    /// Please choose your preferred way of working.
    /// </summary>
    public static readonly string FirstTimeInfo = @"Quick Unlock offers two operation modes.
Please choose your preferred way of working.";
    /// <summary>
    /// Mode:
    /// </summary>
    public static readonly string OptionsQUMode = @"Mode:";
    /// <summary>
    /// Quick Unlock active
    /// </summary>
    public static readonly string Active = @"Quick Unlock active";
    /// <summary>
    /// No Quick Unlock key found.
    /// 
    /// Quick Unlock is not possible.
    /// </summary>
    public static readonly string KeyProvNoQuickUnlock = @"No Quick Unlock key found.

Quick Unlock is not possible.";
    /// <summary>
    /// Prerequsites for mode 'database password':
    /// - Database masterkey contains a password
    /// - Option 'Remember master password' is active
    /// 
    /// An existing Quick Unlock entry will be used as fallback
    /// </summary>
    public static readonly string OptionsQUReqInfoDB = @"Prerequsites for mode 'database password':
- Database masterkey contains a password
- Option 'Remember master password' is active

An existing Quick Unlock entry will be used as fallback";
    /// <summary>
    /// Quick Unlock
    /// </summary>
    public static readonly string OptionsQUSettings = @"Quick Unlock";
    /// <summary>
    /// Quick Unlock entry only
    /// </summary>
    public static readonly string OptionsQUModeEntry = @"Quick Unlock entry only";
    /// <summary>
    /// Database password
    /// </summary>
    public static readonly string OptionsQUModeDatabasePW = @"Database password";
    /// <summary>
    /// Quick Unlock entry created.
    /// 
    /// Please edit and set Quick Unlock PIN as password
    /// </summary>
    public static readonly string OptionsQUEntryCreated = @"Quick Unlock entry created.

Please edit and set Quick Unlock PIN as password";
    /// <summary>
    /// Settings are DB specific
    /// </summary>
    public static readonly string OptionsQUSettingsPerDB = @"Settings are DB specific";
    /// <summary>
    /// Quick Unlock Attempts (requires DB specific settings):
    /// </summary>
    public static readonly string OptionsQUSettingsPerDB_UnlockAttempts = @"Quick Unlock Attempts (requires DB specific settings):";
    /// <summary>
    /// Database specific settings switched off.
    /// 
    /// Click '{0}' to use the global settings for this database.
    /// Click '{1}' to make this database's settings the new global settings.
    /// </summary>
    public static readonly string OptionsSwitchDBToGeneral = @"Database specific settings switched off.

Click '{0}' to use the global settings for this database.
Click '{1}' to make this database's settings the new global settings.";
    /// <summary>
    /// PIN length:
    /// </summary>
    public static readonly string OptionsQUPINLength = @"PIN length:";
    /// <summary>
    /// Unlock
    /// </summary>
    public static readonly string ButtonUnlock = @"Unlock";
    /// <summary>
    /// Quick Unlock PIN:
    /// </summary>
    public static readonly string UnlockLabel = @"Quick Unlock PIN:";
    /// <summary>
    /// Quick Unlock entry could not be found.
    /// 
    /// Create it now?
    /// </summary>
    public static readonly string OptionsQUEntryCreate = @"Quick Unlock entry could not be found.

Create it now?";
    /// <summary>
    /// This key provider cannot be used to create keys.
    /// </summary>
    public static readonly string KeyProvNoCreate = @"This key provider cannot be used to create keys.";
    /// <summary>
    /// 'Remember master password' needs to be active in Options -> Security.
    /// Please don't forget to activate this setting
    /// </summary>
    public static readonly string OptionsQUInfoRememberPassword = @"'Remember master password' needs to be active in Options -> Security.
Please don't forget to activate this setting";
    /// <summary>
    /// Use last {0} characters as PIN
    /// </summary>
    public static readonly string OptionsQUUseLast = @"Use last {0} characters as PIN";
    /// <summary>
    /// Use first {0} characters as PIN
    /// </summary>
    public static readonly string OptionsQUUseFirst = @"Use first {0} characters as PIN";
    /// <summary>
    /// The entered PIN was not correct.
    /// 
    /// The database stays locked and can only be unlocked with the original masterkey
    /// </summary>
    public static readonly string WrongPIN = @"The entered PIN was not correct.

The database stays locked and can only be unlocked with the original masterkey";
    /// <summary>
    /// Hours
    /// </summary>
    public static readonly string Hours = @"Hours";
    /// <summary>
    /// Minutes
    /// </summary>
    public static readonly string Minutes = @"Minutes";
    /// <summary>
    /// Seconds
    /// </summary>
    public static readonly string Seconds = @"Seconds";
    /// <summary>
    /// Global '{0} / {1}'
    /// </summary>
    public static readonly string OptionsLockWorkspace = @"Global '{0} / {1}'";
    /// <summary>
    /// This option changes the behaviour of '{0} / {1}' for both the menu entry as well as the toolbar button.
    /// 
    /// If it's active ALL loaded databases are locked / unlocked by using these commands.
    /// In this case it depends on the active document's state whether a global lock or global unlock is performed.
    /// 
    /// If the [Shift] key is pressed while using these commands only the active document is processed.
    /// </summary>
    public static readonly string OptionsLockWorkspaceDesc = @"This option changes the behaviour of '{0} / {1}' for both the menu entry as well as the toolbar button.

If it's active ALL loaded databases are locked / unlocked by using these commands.
In this case it depends on the active document's state whether a global lock or global unlock is performed.

If the [Shift] key is pressed while using these commands only the active document is processed.";
    /// <summary>
    /// SoftLock active. Click topmost form to deactivate
    /// </summary>
    public static readonly string SoftlockModeUnhideForms = @"SoftLock active. Click topmost form to deactivate";
    /// <summary>
    /// SoftLock active. Click to deactivate
    /// </summary>
    public static readonly string SoftlockModeUnhide = @"SoftLock active. Click to deactivate";
    /// <summary>
    /// SoftLock after inactivity
    /// </summary>
    public static readonly string SoftlockActive = @"SoftLock after inactivity";
    /// <summary>
    /// SoftLock when minimizing KeePass
    /// </summary>
    public static readonly string SoftlockOnMinimize = @"SoftLock when minimizing KeePass";
    /// <summary>
    /// SoftLock hides following sensitive information while still allowing Auto-Type as well as other integration:
    /// - group list
    /// - entry list
    /// - entry view
    /// - all forms NOT mentioned in config file property LockAssist.SoftlockExcludeForms
    /// 
    /// Valid Quick Unlock settings are required, Quick Unlock itself does NOT need to be active
    /// </summary>
    public static readonly string SoftlockDesc = @"SoftLock hides following sensitive information while still allowing Auto-Type as well as other integration:
- group list
- entry list
- entry view
- all forms NOT mentioned in config file property LockAssist.SoftlockExcludeForms

Valid Quick Unlock settings are required, Quick Unlock itself does NOT need to be active";
    #endregion

    #region NO changes in this area
    private static StringDictionary m_translation = new StringDictionary();

    public static void Init(Plugin plugin, string LanguageCodeIso6391)
    {
      List<string> lDebugStrings = new List<string>();
      m_translation.Clear();
      bool bError = true;
      LanguageCodeIso6391 = InitTranslation(plugin, lDebugStrings, LanguageCodeIso6391, out bError);
      if (bError && (LanguageCodeIso6391.Length > 2))
      {
        LanguageCodeIso6391 = LanguageCodeIso6391.Substring(0, 2);
        lDebugStrings.Add("Trying fallback: " + LanguageCodeIso6391);
        LanguageCodeIso6391 = InitTranslation(plugin, lDebugStrings, LanguageCodeIso6391, out bError);
      }
      if (bError)
      {
        PluginDebug.AddError("Reading translation failed", 0, lDebugStrings.ToArray());
        LanguageCodeIso6391 = "en";
      }
      else
      {
        List<FieldInfo> lTranslatable = new List<FieldInfo>(
          typeof(PluginTranslate).GetFields(BindingFlags.Static | BindingFlags.Public)
          ).FindAll(x => x.IsInitOnly);
        lDebugStrings.Add("Parsing complete");
        lDebugStrings.Add("Translated texts read: " + m_translation.Count.ToString());
        lDebugStrings.Add("Translatable texts: " + lTranslatable.Count.ToString());
        foreach (FieldInfo f in lTranslatable)
        {
          if (m_translation.ContainsKey(f.Name))
          {
            lDebugStrings.Add("Key found: " + f.Name);
            f.SetValue(null, m_translation[f.Name]);
          }
          else
            lDebugStrings.Add("Key not found: " + f.Name);
        }
        PluginDebug.AddInfo("Reading translations finished", 0, lDebugStrings.ToArray());
      }
      if (TranslationChanged != null)
      {
        TranslationChanged(null, new TranslationChangedEventArgs(LanguageIso6391, LanguageCodeIso6391));
      }
      LanguageIso6391 = LanguageCodeIso6391;
      lDebugStrings.Clear();
    }

    private static string InitTranslation(Plugin plugin, List<string> lDebugStrings, string LanguageCodeIso6391, out bool bError)
    {
      if (string.IsNullOrEmpty(LanguageCodeIso6391))
      {
        lDebugStrings.Add("No language identifier supplied, using 'en' as fallback");
        LanguageCodeIso6391 = "en";
      }
      string filename = GetFilename(plugin.GetType().Namespace, LanguageCodeIso6391);
      lDebugStrings.Add("Translation file: " + filename);

      if (!File.Exists(filename)) //If e. g. 'plugin.zh-tw.language.xml' does not exist, try 'plugin.zh.language.xml'
      {
        lDebugStrings.Add("File does not exist");
        bError = true;
        return LanguageCodeIso6391;
      }
      else
      {
        string translation = string.Empty;
        try { translation = File.ReadAllText(filename); }
        catch (Exception ex)
        {
          lDebugStrings.Add("Error reading file: " + ex.Message);
          LanguageCodeIso6391 = "en";
          bError = true;
          return LanguageCodeIso6391;
        }
        XmlSerializer xs = new XmlSerializer(m_translation.GetType());
        lDebugStrings.Add("File read, parsing content");
        try
        {
          m_translation = (StringDictionary)xs.Deserialize(new StringReader(translation));
        }
        catch (Exception ex)
        {
          string sException = ex.Message;
          if (ex.InnerException != null) sException += "\n" + ex.InnerException.Message;
          lDebugStrings.Add("Error parsing file: " + sException);
          LanguageCodeIso6391 = "en";
          MessageBox.Show("Error parsing translation file\n\n" + sException, PluginName, MessageBoxButtons.OK, MessageBoxIcon.Error);
          bError = true;
          return LanguageCodeIso6391;
        }
        bError = false;
        return LanguageCodeIso6391;
      }
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
      bool bFirst = true;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (bFirst)
        {
          bFirst = false;
          try
          {
            reader.ReadStartElement("TranslationVersion");
            PluginTranslate.TranslationVersion = reader.ReadContentAsLong();
            reader.ReadEndElement();
          }
          catch { }
        }
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
      writer.WriteStartElement("TranslationVersion");
      writer.WriteString(PluginTranslate.TranslationVersion.ToString());
      writer.WriteEndElement();
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
