using KeePass;
using KeePass.Forms;
using KeePass.Plugins;
using KeePass.UI;
using KeePassLib;
using KeePassLib.Collections;
using KeePassLib.Keys;
using KeePassLib.Security;
using KeePassLib.Serialization;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using PluginTranslation;
using PluginTools;

namespace LockAssist
{
	public partial class LockAssistExt : Plugin, IMessageFilter
	{
		private void QuickUnlock_Init()
		{
			m_kp = new QuickUnlockKeyProv();
			m_host.KeyProviderPool.Add(m_kp);
			m_host.MainWindow.FileClosingPre += OnFileClosePre;
			m_host.MainWindow.FileOpened += OnFileOpened;
			GlobalWindowManager.WindowAdded += OnWindowAdded;
		}

		#region Eventhandler for opening and closing a DB
		private void OnFileOpened(object sender, FileOpenedEventArgs e)
		{
			CheckSoftlockMode();
			m_options.ReadConfig(e.Database);
			if (m_options.FirstTime &&
				(!m_options.UsePassword && (GetQuickUnlockEntry(e.Database) == null)
				|| (m_options.UsePassword && !Program.Config.Security.MasterPassword.RememberWhileOpen)))
			{
				Tools.ShowInfo(PluginTranslate.FirstTimeInfo);
				Tools.ShowOptions();
				m_host.CustomConfig.SetBool("LockAssist.FirstTime", false);
			}
			if (!m_options.QUActive) return;
			//Restore previously stored information about the masterkey
			QuickUnlockOldKeyInfo ok = m_kp.GetOldKey(e.Database);
			if (ok == null) return;
			KcpCustomKey ck = (KcpCustomKey)e.Database.MasterKey.GetUserKey(typeof(KcpCustomKey));
			if ((ck == null) || (ck.Name != QuickUnlockKeyProv.KeyProviderName)) return;

			e.Database.MasterKey.RemoveUserKey(ck);
			if (ok.pwHash != null)
			{
				KcpPassword p = m_kp.DeserializePassword(ok.pwHash, Program.Config.Security.MasterPassword.RememberWhileOpen);
				e.Database.MasterKey.AddUserKey(p);
			}
			if (!string.IsNullOrEmpty(ok.keyFile)) e.Database.MasterKey.AddUserKey(new KcpKeyFile(ok.keyFile));
			if (ok.account) e.Database.MasterKey.AddUserKey(new KcpUserAccount());
			Program.Config.Defaults.SetKeySources(e.Database.IOConnectionInfo, e.Database.MasterKey);
		}

		private void OnFileClosePre(object sender, FileClosingEventArgs e)
		{
			//Do quick unlock only in case of locking
			//Do NOT do quick unlock in case of closing the database
			if (e.Flags != FileEventFlags.Locking) return;
			m_options.ReadConfig(e.Database);
			if (!m_options.QUActive) return;
			ProtectedString QuickUnlockKey = null;
			if (Program.Config.Security.MasterPassword.RememberWhileOpen && m_options.UsePassword)
				QuickUnlockKey = GetQuickUnlockKeyFromMasterKey(e.Database);
			if (QuickUnlockKey == null)
				QuickUnlockKey = GetQuickUnlockKeyFromEntry(e.Database);
			QuickUnlockKey = TrimQuickUnlockKey(QuickUnlockKey);
			if ((QuickUnlockKey == null) || QuickUnlockKey.IsEmpty) return;
			m_kp.AddDb(e.Database, QuickUnlockKey, Program.Config.Security.MasterPassword.RememberWhileOpen && m_options.UsePassword);
		}
		#endregion

		#region Unlock / KeyPromptForm
		private void OnWindowAdded(object sender, GwmWindowEventArgs e)
		{
			if (!(e.Form is KeyPromptForm) && !(e.Form is KeyCreationForm)) return;
			e.Form.Shown += (o, x) => OnKeyFormShown(o, false);
		}

		public static void OnKeyFormShown(object sender, bool resetFile)
		{
			if (CheckGlobalUnlock())
			{
				GlobalWindowManager.RemoveWindow(sender as Form);
				(sender as Form).Close();(sender as Form).Dispose();
				GlobalUnlock(sender, null);
				return;
			}
			Form keyform = (sender as Form);
			try
			{
				ComboBox cmbKeyFile = (ComboBox)Tools.GetControl("m_cmbKeyFile", keyform);
				if (cmbKeyFile == null) return;
				int index = cmbKeyFile.Items.IndexOf(QuickUnlockKeyProv.KeyProviderName);
				//Quick Unlock cannot be used to create a key ==> Remove it from list of key providers
				if (keyform is KeyCreationForm)
				{
					if (index == -1) return;
					cmbKeyFile.Items.RemoveAt(index);
					List<string> keyfiles = (List<string>)Tools.GetField("m_lKeyFileNames", keyform);
					if (keyfiles != null) keyfiles.Remove(QuickUnlockKeyProv.KeyProviderName);
					return;
				}

				//Key prompt form is shown
				IOConnectionInfo dbIOInfo = (IOConnectionInfo)Tools.GetField("m_ioInfo", keyform);
				if (m_bContinueUnlock)
				{
					CheckBox cbContinueUnlock = new CheckBox();
					cbContinueUnlock.AutoSize = true;
					cbContinueUnlock.Text = KeePass.Resources.KPRes.LockMenuUnlock;
					cbContinueUnlock.Checked = true;
					cbContinueUnlock.Name = c_LockAssistContinueUnlockWorkbench;
					cbContinueUnlock.CheckedChanged += (o, e) => { m_bContinueUnlock = cbContinueUnlock.Checked; };
					cbContinueUnlock.Checked = cbContinueUnlock.Checked;
					CheckBox cbPassword = (CheckBox)Tools.GetControl("m_cbPassword", keyform);
					CheckBox cbAccount = (CheckBox)Tools.GetControl("m_cbUserAccount", keyform);
					cbContinueUnlock.Left = cbAccount.Left;
					cbContinueUnlock.Top = cbAccount.Top + 30;
					cbAccount.Parent.Controls.Add(cbContinueUnlock);
				}
				//If Quick Unlock is possible show the Quick Unlock form
				if ((index != -1) && (dbIOInfo != null) && QuickUnlockKeyProv.HasDB(dbIOInfo.Path))
				{
					cmbKeyFile.SelectedIndex = index;
					CheckBox cbPassword = (CheckBox)Tools.GetControl("m_cbPassword", keyform);
					CheckBox cbAccount = (CheckBox)Tools.GetControl("m_cbUserAccount", keyform);
					Button bOK = (Button)Tools.GetControl("m_btnOK", keyform);
					if ((bOK != null) && (cbPassword != null) && (cbAccount != null))
					{
						UIUtil.SetChecked(cbPassword, false);
						UIUtil.SetChecked(cbAccount, false);
						bOK.PerformClick();
					}
					return;
				}

				//Quick Unlock is not possible => Remove it from list of key providers
				if ((resetFile || ((dbIOInfo != null) && !QuickUnlockKeyProv.HasDB(dbIOInfo.Path))) && (index != -1))
				{
					cmbKeyFile.Items.RemoveAt(index);
					List<string> keyfiles = (List<string>)Tools.GetField("m_lKeyFileNames", keyform);
					if (keyfiles != null) keyfiles.Remove(QuickUnlockKeyProv.KeyProviderName);
					if (resetFile) cmbKeyFile.SelectedIndex = 0;
				}
			}
			catch (Exception) { }
		}
		#endregion

		#region QuickUnlockKey handling
		private ProtectedString GetQuickUnlockKeyFromMasterKey(PwDatabase db)
		{
			/*
             * Try to create QuickUnlockKey based on password
             * 
             * If no password is contained in MasterKey there 
             * EITHER is no password at all
             * OR the database was unlocked with Quick Unlock
             * In these case ask our key provider for the original password
             */
			ProtectedString QuickUnlockKey = null;
			try
			{
				KcpPassword pw = (KcpPassword)db.MasterKey.GetUserKey(typeof(KcpPassword));
				if (pw != null)
					QuickUnlockKey = pw.Password;
			}
			catch (Exception) { }
			if ((QuickUnlockKey != null) && (QuickUnlockKey.Length > 0)) return QuickUnlockKey;
			return null;
		}

		private ProtectedString GetQuickUnlockKeyFromEntry(PwDatabase db)
		{
			PwEntry QuickUnlockEntry = GetQuickUnlockEntry(db);
			if (QuickUnlockEntry == null) return null;
			return QuickUnlockEntry.Strings.GetSafe(PwDefs.PasswordField);
		}

		public static PwEntry GetQuickUnlockEntry(PwDatabase db)
		{
			if ((db == null) || !db.IsOpen) return null;
			SearchParameters sp = new SearchParameters();
			sp.SearchInTitles = true;
			sp.ExcludeExpired = true;
			sp.SearchString = QuickUnlockKeyProv.KeyProviderName;
			PwObjectList<PwEntry> entries = new PwObjectList<PwEntry>();
			db.RootGroup.SearchEntries(sp, entries);
			if ((entries == null) || (entries.UCount == 0)) return null;
			return entries.GetAt(0);
		}

		private ProtectedString TrimQuickUnlockKey(ProtectedString QuickUnlockKey)
		{
			if ((QuickUnlockKey == null) || (QuickUnlockKey.Length <= m_options.Length)) return QuickUnlockKey;
			int startIndex = 0;
			if (!m_options.FromEnd)
				startIndex = m_options.Length;
			QuickUnlockKey = QuickUnlockKey.Remove(startIndex, QuickUnlockKey.Length - m_options.Length);
			return QuickUnlockKey;
		}
		#endregion

		private void QuickUnlock_Terminate()
		{
			m_host.KeyProviderPool.Remove(m_kp);
			m_kp = null;
			m_host.MainWindow.FileClosingPre -= OnFileClosePre;
			m_host.MainWindow.FileOpened -= OnFileOpened;
			GlobalWindowManager.WindowAdded -= OnWindowAdded;
		}
	}
}
