using KeePassLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

		public static List<string> SL_ExcludeForms
		{
			get
			{
				string sForms = _config.GetString(LockAssistSoftlockExcludeForms, "AboutForm,AutoTypeCtxForm,CharPickerForm,ColumnsForm," +
					"HelpSourceForm,KeyPromptForm,LanguageForm,PluginsForm,PwGeneratorForm,UpdateCheckForm");
				var aForms = sForms.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				List<string> lForms = new List<string>();
				foreach (var sForm in aForms)
                {
					if (!lForms.Contains(sForm.Trim())) lForms.Add(sForm.Trim());
                }
				return lForms;
			}
		}
	}
}
