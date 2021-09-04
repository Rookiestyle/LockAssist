using System;
using System.Windows.Forms;

using KeePass;
using KeePassLib;
using KeePass.UI;

using PluginTranslation;
using PluginTools;
using KeePass.Resources;

namespace LockAssist
{
	public partial class OptionsForm : UserControl
	{
		private bool FirstTime = false;

		public OptionsForm()
		{
			InitializeComponent();

			Text = PluginTranslate.PluginName;

			cbActive.Text = PluginTranslate.Active;
			tabQuickUnlock.Text = PluginTranslate.OptionsQUSettings;
			lQUMode.Text = PluginTranslate.OptionsQUMode;
			lQUPINLength.Text = PluginTranslate.OptionsQUPINLength;
			tbModeExplain.Lines = PluginTranslate.OptionsQUReqInfoDB.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None); ;
			cbPINDBSpecific.Text = PluginTranslate.OptionsQUSettingsPerDB;
			cbPINMode.Items.Clear();
			cbPINMode.Items.AddRange(new string[] { PluginTranslate.OptionsQUModeEntry, PluginTranslate.OptionsQUModeDatabasePW });

			cbQUValidity.Items.Add(PluginTranslate.Minutes);
			cbQUValidity.Items.Add(PluginTranslate.Hours);
			cbQUValidity.SelectedIndex = 0;
			cbQUValidityActive.Text = KPRes.ExpiryTime;


			tabLockWorkspace.Text = KPRes.LockWorkspace;
			cbLockWorkspace.Text = string.Format(PluginTranslate.OptionsLockWorkspace, KPRes.LockWorkspace, KPRes.LockMenuUnlock);
			tbLockWorkspace.Lines = string.Format(PluginTranslate.OptionsLockWorkspaceDesc, KPRes.LockWorkspace, KPRes.LockMenuUnlock).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

			tabSoftLock.Text = "SoftLock";
			cbSLActive.Text = PluginTranslate.SoftlockActive;
			cbSLOnMinimize.Text = PluginTranslate.SoftlockOnMinimize;
			tbSoftLockDesc.Lines = PluginTranslate.SoftlockDesc.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None); ;

			cbSLInterval.Items.Add(PluginTranslate.Seconds);
			cbSLInterval.Items.Add(PluginTranslate.Minutes);
			cbSLInterval.SelectedIndex = 0;

		}

		internal void InitEx(LockAssistConfig options)
		{
			Init_QuickUnlock(options);
			Init_LockWorkspace();
			Init_SoftLock();
		}

        private void Init_QuickUnlock(LockAssistConfig options)
        {
			cbActive.Checked = options.QU_Active;
			cbPINMode.SelectedIndex = options.QU_UsePassword ? 1 : 0;
			tbPINLength.Text = options.QU_PINLength.ToString();
			rbPINEnd.Checked = options.QU_UsePasswordFromEnd;
			rbPINFront.Checked = !options.QU_UsePasswordFromEnd;
			cbPINDBSpecific.Checked = options.QU_DBSpecific;
			FirstTime = LockAssistConfig.QU_FirstTime;
			cbQUValidityActive.Checked = options.QU_ValiditySeconds > 0;
			decimal minutes = options.QU_ValiditySeconds / 60;
			if (minutes < 60)
			{
				cbQUValidity.SelectedIndex = 0;
				nQUValidity.Value = minutes;
			}
			else
			{
				cbQUValidity.SelectedIndex = 1;
				nQUValidity.Value = minutes / 60;
			}
		}

		private void Init_LockWorkspace()
        {
			cbLockWorkspace.Checked = LockAssistConfig.LW_Active;
		}

		private void Init_SoftLock()
        {
			cbSLActive.Checked = LockAssistConfig.SL_IsActive;
			if (LockAssistConfig.SL_Seconds < 60)
			{
				cbSLInterval.SelectedIndex = 0;
				nSLSeconds.Value = LockAssistConfig.SL_Seconds;
			}
			else
			{
				cbSLInterval.SelectedIndex = 1;
				nSLSeconds.Value = LockAssistConfig.SL_Seconds / 60;
			}
			cbSLOnMinimize.Checked = LockAssistConfig.SL_OnMinimize;
        }

        internal LockAssistConfig GetQuickUnlockOptions()
		{
			LockAssistConfig options = new LockAssistConfig();
			options.QU_Active = cbActive.Checked;
			options.QU_UsePassword = cbPINMode.SelectedIndex == 1;
			if (!int.TryParse(tbPINLength.Text, out options.QU_PINLength)) options.QU_PINLength = 4;
			options.QU_UsePasswordFromEnd = rbPINEnd.Checked;
			options.QU_DBSpecific = cbPINDBSpecific.Checked;

			if (cbQUValidityActive.Checked)
			{
				if (cbQUValidity.SelectedIndex == 0) options.QU_ValiditySeconds = (int)(nQUValidity.Value * 60);
				else if (cbQUValidity.SelectedIndex == 1) options.QU_ValiditySeconds = (int)(nQUValidity.Value * 60 * 60);
			}
			else options.QU_ValiditySeconds = 0;
			return options;
		}

		internal bool GetLockWorkspace()
        {
			return cbLockWorkspace.Checked;
        }

		internal SoftLockSettings GetSoftLockOptions()
        {
			SoftLockSettings slsResult = new SoftLockSettings();
			slsResult.Active = cbSLActive.Checked;
			slsResult.Seconds = (int)nSLSeconds.Value;
			if (cbSLInterval.SelectedIndex == 1) slsResult.Seconds *= 60;
			slsResult.SoftLockOnMinimize = cbSLOnMinimize.Checked;
			return slsResult;
		}

		private void tbPINLength_TextChanged(object sender, EventArgs e)
		{
			int len = 0;
			if (!int.TryParse(tbPINLength.Text, out len)) len = 4;
			if (len < 1) len = 1;
			if (len > 32) len = 32;
			rbPINFront.Text = string.Format(PluginTranslate.OptionsQUUseFirst, len.ToString());
			rbPINEnd.Text = string.Format(PluginTranslate.OptionsQUUseLast, len.ToString());
		}

		private void cbPINMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			lQUMode.ForeColor = System.Drawing.SystemColors.ControlText;
			if (cbPINMode.SelectedIndex == 0)
			{
				PwEntry check = QuickUnlock.GetQuickUnlockEntry(Program.MainForm.ActiveDatabase);
				if (check != null) return;
				if (!FirstTime && (Tools.AskYesNo(PluginTranslate.OptionsQUEntryCreate) == DialogResult.No))
				{
					cbPINMode.SelectedIndex = 1;
				}
				else
				{
					check = new PwEntry(true, true);
					Program.MainForm.ActiveDatabase.RootGroup.AddEntry(check, true);
					check.Strings.Set(PwDefs.TitleField, new KeePassLib.Security.ProtectedString(false, QuickUnlockKeyProv.KeyProviderName));
					Tools.ShowInfo(PluginTranslate.OptionsQUEntryCreated);
					ShowQuickUnlockEntry(Program.MainForm.ActiveDatabase, check.Uuid);
				}
				return;
			}
			if (Program.Config.Security.MasterPassword.RememberWhileOpen) return;
			lQUMode.ForeColor = System.Drawing.Color.Red;
			if (FirstTime || (cbPINMode.SelectedIndex == 0)) return;
			Tools.ShowInfo(PluginTranslate.OptionsQUInfoRememberPassword);
		}

		private void tbValidating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			int len = 0;
			if (!int.TryParse((sender as TextBox).Text, out len))
			{
				if ((sender as TextBox).Name == "tbPinLength") len = 4;
				else len = 60;
			}
			if ((sender as TextBox).Name == "tbPinLength") len = Math.Max(1, len);
			if ((sender as TextBox).Name != "tbPinLength") len = Math.Max(0, len);
			int max = int.Parse((string)(sender as TextBox).Tag);
			if (len > max) len = max;
			if ((sender as TextBox).Text != len.ToString()) (sender as TextBox).Text = len.ToString();
		}

		private void ShowQuickUnlockEntry(PwDatabase db, PwUuid qu)
		{
			Program.MainForm.UpdateUI(false, null, false, db.RootGroup, true, db.RootGroup, true);
			Program.MainForm.EnsureVisibleEntry(qu);

			ListView lv = (Program.MainForm.Controls.Find("m_lvEntries", true)[0] as ListView);
			foreach (ListViewItem lvi in lv.Items)
			{
				PwListItem li = (lvi.Tag as PwListItem);
				if (li == null) continue;

				PwEntry pe = li.Entry;
				if (pe.Uuid != qu) continue;
				lv.FocusedItem = lvi;
				ToolStripItem[] tsmi = Program.MainForm.EntryContextMenu.Items.Find("m_ctxEntryEdit", false);
				if (tsmi != null) tsmi[0].PerformClick();
				break;
			}
		}

		private void UnlockOptions_Load(object sender, EventArgs e)
		{
			if ((Program.MainForm.ActiveDatabase == null) || !Program.MainForm.ActiveDatabase.IsOpen)
			{
				cbPINDBSpecific.Enabled = false;
				cbPINDBSpecific.Checked = false;
			}
		}
	}
}
