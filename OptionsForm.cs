using System;
using System.Windows.Forms;

using KeePass;
using KeePassLib;
using KeePass.UI;

using PluginTranslation;
using PluginTools;

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
			tabSoftlock.Text = PluginTranslate.OptionsSLSettings;
			cbSLIdle.Text = PluginTranslate.OptionsSLSecondsToActivate;
			cbSLOnMinimize.Text = PluginTranslate.OptionsSLMinimize;
			tbSoftlockExplain.Lines = PluginTranslate.OptionsSLInfo.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None); ;
			tabAdditional.Text = KeePass.Resources.KPRes.LockWorkspace;
			cbLockWorkspace.Text = string.Format(PluginTranslate.OptionsLockWorkspace, KeePass.Resources.KPRes.LockWorkspace, KeePass.Resources.KPRes.LockMenuUnlock);
			tbLockWorkspace.Lines = PluginTranslate.OptionsLockWorkspaceDesc.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None); ;
		}

		public void SetOptions(LockAssistOptions options)
		{
			cbActive.Checked = options.QUActive;
			cbPINMode.SelectedIndex = options.UsePassword ? 1 : 0;
			tbPINLength.Text = options.Length.ToString();
			rbPINEnd.Checked = options.FromEnd;
			rbPINFront.Checked = !options.FromEnd;
			cbPINDBSpecific.Checked = options.DBSpecific;
			cbSLIdle.Checked = options.SLIdle && (options.SLSeconds > 0);
			if (options.SLSeconds < 1) options.SLSeconds = 60;
			//tSLIdleSeconds.Text = "60";
			tSLIdleSeconds.Text = options.SLSeconds.ToString();
			cbSLOnMinimize.Checked = options.SLMinimize;
			FirstTime = options.FirstTime;
			cbLockWorkspace.Checked = options.LockWorkspace;
		}

		public LockAssistOptions GetOptions()
		{
			LockAssistOptions options = new LockAssistOptions();
			options.QUActive = cbActive.Checked;
			options.UsePassword = cbPINMode.SelectedIndex == 1;
			if (!int.TryParse(tbPINLength.Text, out options.Length)) options.Length = 4;
			options.FromEnd = rbPINEnd.Checked;
			options.DBSpecific = cbPINDBSpecific.Checked;
			if (!int.TryParse(tSLIdleSeconds.Text, out options.SLSeconds)) options.SLSeconds = 60;
			if (options.SLSeconds <= 0) options.SLSeconds = 60;
			options.SLIdle = cbSLIdle.Checked;
			options.SLMinimize = cbSLOnMinimize.Checked;
			options.LockWorkspace = cbLockWorkspace.Checked;
			return options;
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
				PwEntry check = LockAssistExt.GetQuickUnlockEntry(Program.MainForm.ActiveDatabase);
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
