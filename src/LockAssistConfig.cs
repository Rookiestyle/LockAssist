using KeePassLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockAssist
{
	internal class LockAssistConfig
	{
		public static KeePass.App.Configuration.AceCustomConfig _config = KeePass.Program.Config.CustomConfig;
		public static LockAssistConfig GetOptions(PwDatabase db)
		{
			LockAssistConfig conf = new LockAssistConfig();
			conf.GetOptionsInternal(db);
			return conf;
		}

		private void GetOptionsInternal(PwDatabase db)
		{
			QU_DBSpecific = (db != null) && (db.IsOpen) && db.CustomData.Exists(LockAssistQuickUnlockDBSpecific);
			if (QU_DBSpecific)
			{
				QU_Active = db.CustomData.Get(LockAssistActive) == "true";
				QU_UsePassword = db.CustomData.Get(LockAssistUsePassword) == "true";
				if (!int.TryParse(db.CustomData.Get(LockAssistKeyLength), out QU_PINLength)) QU_PINLength = 4;
				QU_UsePasswordFromEnd = db.CustomData.Get(LockAssistKeyFromEnd) == "false";
			}
			else
			{
				QU_Active = _config.GetBool(LockAssistActive, false);
				QU_UsePassword = _config.GetBool(LockAssistUsePassword, true);
				QU_PINLength = (int)_config.GetLong(LockAssistKeyLength, 4);
				QU_UsePasswordFromEnd = _config.GetBool(LockAssistKeyFromEnd, true);
			}
		}

		public static bool FirstTime
		{
			get { return _config.GetBool(LockAssistFirstTime, true); }
			set { _config.SetBool(LockAssistFirstTime, value); }
		}

		public bool QU_Active = false;
		public bool QU_DBSpecific = false;
		public bool QU_UsePassword = true;
		public bool QU_UsePasswordFromEnd = true;
		public int QU_PINLength = 4;

		public bool ConfigChanged(LockAssistConfig comp, bool CheckDBSpecific)
		{
			if (QU_Active != comp.QU_Active) return true;
			if (CheckDBSpecific && (QU_DBSpecific != comp.QU_DBSpecific)) return true;
			if (QU_UsePassword != comp.QU_UsePassword) return true;
			if (QU_PINLength != comp.QU_PINLength) return true;
			if (QU_UsePasswordFromEnd != comp.QU_UsePasswordFromEnd) return true;
			return false;
		}

		public bool CopyFrom(LockAssistConfig NewOptions)
		{
			bool SwitchToNoDBSpecific = QU_DBSpecific && !NewOptions.QU_DBSpecific;
			QU_DBSpecific = NewOptions.QU_DBSpecific;
			QU_Active = NewOptions.QU_Active;
			QU_UsePassword = NewOptions.QU_UsePassword;
			QU_PINLength = NewOptions.QU_PINLength;
			QU_UsePasswordFromEnd = NewOptions.QU_UsePasswordFromEnd;
			return SwitchToNoDBSpecific;
		}

		public void WriteConfig()
		{
			QU_DBSpecific = false;
			WriteConfig(null);
		}

		public void WriteConfig(PwDatabase db)
		{
			if (QU_DBSpecific)
			{
				if (db == null || !db.IsOpen) return;
				db.CustomData.Set(LockAssistActive, QU_Active ? "true" : "false");
				db.CustomData.Set(LockAssistUsePassword, QU_UsePassword ? "true" : "false");
				db.CustomData.Set(LockAssistKeyLength, QU_PINLength.ToString());
				db.CustomData.Set(LockAssistKeyFromEnd, QU_UsePasswordFromEnd ? "true" : "false");
				db.CustomData.Set(LockAssistQuickUnlockDBSpecific, "true");
				FlagDBChanged(db);
			}
			else
			{
				_config.SetBool(LockAssistActive, QU_Active);
				_config.SetBool(LockAssistUsePassword, QU_UsePassword);
				_config.SetLong(LockAssistKeyLength, QU_PINLength);
				_config.SetBool(LockAssistKeyFromEnd, QU_UsePasswordFromEnd);
				DeleteDBConfig(db);
			}
		}

		private void FlagDBChanged(PwDatabase db)
		{
			db.Modified = true;
			db.SettingsChanged = DateTime.UtcNow;
			KeePass.Program.MainForm.UpdateUI(false, KeePass.Program.MainForm.DocumentManager.FindDocument(db), false, null, false, null, true);
		}

		public void DeleteDBConfig(PwDatabase db)
		{
			if (db == null || !db.IsOpen) return;
			bool deleted = db.CustomData.Remove(LockAssistActive);
			deleted |= deleted = db.CustomData.Remove(LockAssistUsePassword);
			deleted |= db.CustomData.Remove(LockAssistKeyLength);
			deleted |= db.CustomData.Remove(LockAssistKeyFromEnd);
			deleted |= db.CustomData.Remove(LockAssistQuickUnlockDBSpecific);
			if (deleted)
			{
				FlagDBChanged(db);
				GetOptionsInternal(db);
			}
		}

		private const string LockAssistActive = "LockAssist.Active";
		private const string LockAssistUsePassword = "LockAssist.UsePassword";
		private const string LockAssistKeyLength = "LockAssist.KeyLength";
		private const string LockAssistKeyFromEnd = "LockAssist.KeyFromEnd";
		private const string LockAssistFirstTime = "LockAssist.FirstTime";
		private const string LockAssistQuickUnlockDBSpecific = "LockAssist.QuickUnlockDBSpecific";
	}
}
