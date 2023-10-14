using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeePassLib;

namespace LockAssist
{
  //SoftLock part
  internal partial class LockAssistConfig
  {
    private const string LockAssistSoftLockActive = "LockAssist.SoftLockActive";
    private const string LockAssistSoftLockSeconds = "LockAssist.SoftLockSeconds";
    private const string LockAssistSoftlockOnMinimize = "LockAssist.SoftlockOnMinimize";
    private const string LockAssistSoftlockExcludeForms = "LockAssist.SoftlockExcludeForms";
    private const string LockAssistSoftlockValidityActive = "LockAssist.SoftlockValidityActive";
    private const string LockAssistSoftlockValiditySeconds = "LockAssist.SoftlockValiditySeconds";

    public static bool SL_ValidityActive
    {
      get { return _config.GetBool(LockAssistSoftlockValidityActive, false); }
      set { _config.SetBool(LockAssistSoftlockValidityActive, value); }
    }

    public static int SL_ValiditySeconds
    {
      get { return (int)_config.GetLong(LockAssistSoftlockValiditySeconds, 1800); }
      set { _config.SetLong(LockAssistSoftlockValiditySeconds, value); }
    }

    public static bool SL_Active
    {
      get { return _config.GetBool(LockAssistSoftLockActive, true); }
      set { _config.SetBool(LockAssistSoftLockActive, value); }
    }

    public static int SL_Seconds
    {
      get { return (int)_config.GetLong(LockAssistSoftLockSeconds, 60); }
      set { _config.SetLong(LockAssistSoftLockSeconds, value); }
    }

    public static bool SL_IsActive
    {
      get { return SL_Active && SL_Seconds > 0; }
    }

    public static bool SL_OnMinimize
    {
      get { return _config.GetBool(LockAssistSoftlockOnMinimize, true); }
      set { _config.SetBool(LockAssistSoftlockOnMinimize, value); }
    }

    private static List<string> m_SL_DefaultExcludeForms = new List<string>() {"AboutForm","AutoTypeCtxForm","CharPickerForm","ColumnsForm",
          "HelpSourceForm","KeyPromptForm","LanguageForm","PluginsForm","PwGeneratorForm","UpdateCheckForm", "OtpKeyProv.Forms.OtpKeyPromptForm" };
    private const string mc_ExcludeFormsText = "Enter form names to exclude from Softlock";
    public static List<string> SL_ExcludeForms
    {
      get
      {
        //Set defaults
        List<string> lForms = new List<string>();
        lForms.AddRange(m_SL_DefaultExcludeForms);

        string sForms = _config.GetString(LockAssistSoftlockExcludeForms, string.Empty);
        if (string.IsNullOrEmpty(sForms))
        {
          sForms = string.Empty;
          SL_ExcludeForms = new List<string>() { mc_ExcludeFormsText };
        }
        var aForms = sForms.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var sForm in aForms)
        {
          if (sForm == mc_ExcludeFormsText) continue;
          if (sForm.Trim().StartsWith("-")) continue;
          if (!lForms.Contains(sForm.Trim())) lForms.Add(sForm.Trim());
        }
        foreach (var sForm in aForms)
        {
          if (sForm == mc_ExcludeFormsText) continue; 
          if (!sForm.Trim().StartsWith("-")) continue;
          lForms.Remove(sForm.Trim().Substring(1));
        }
        return lForms;
      }
      set
      {
        string sForms = string.Empty;
        foreach (var s in value)
        {
          if (!string.IsNullOrEmpty(sForms)) sForms += ",";
          sForms += s;
        }
        _config.SetString(LockAssistSoftlockExcludeForms, sForms);
      }
    }
  }
}
