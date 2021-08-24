using System.Windows.Forms;
using System.Drawing;
using KeePassLib.Security;

using PluginTranslation;
using PluginTools;

namespace LockAssist
{
	public partial class UnlockForm : Form
	{
		public UnlockForm()
		{
			InitializeComponent();
			cbTogglePin.Image = (Image)KeePass.Program.Resources.GetObject("B19x07_3BlackDots");
			if (cbTogglePin.Image != null)
			{
				cbTogglePin.AutoSize = false;
				cbTogglePin.Text = string.Empty;
				if (KeePass.UI.UIUtil.IsDarkTheme)
					cbTogglePin.Image = KeePass.UI.UIUtil.InvertImage(cbTogglePin.Image);
			}

			Text = QuickUnlockKeyProv.KeyProviderName;
			lLabel.Text = PluginTranslate.UnlockLabel;
			bUnlock.Text = PluginTranslate.ButtonUnlock;
			bCancel.Text = PluginTranslate.ButtonCancel;

			KeePass.UI.SecureTextBoxEx.InitEx(ref stbPIN);
			cbTogglePin.Checked = true;
			stbPIN.EnableProtection(cbTogglePin.Checked);

			cbContinueUnlock.Text = KeePass.Resources.KPRes.LockMenuUnlock;
			cbContinueUnlock.Visible = cbContinueUnlock.Checked = LockAssistExt.m_bContinueUnlock;
		}

		public ProtectedString GetQuickUnlockKey()
		{
			return stbPIN.TextEx;
		}

		private void togglePIN_CheckedChanged(object sender, System.EventArgs e)
		{
			stbPIN.EnableProtection(cbTogglePin.Checked);
		}

		private void cbContinueUnlock_CheckedChanged(object sender, System.EventArgs e)
		{
			CheckBox cbContinue = (CheckBox)Tools.GetControl(LockAssistExt.c_LockAssistContinueUnlockWorkbench, KeePass.UI.GlobalWindowManager.TopWindow);
			if (cbContinue != null)
				cbContinue.Checked = cbContinueUnlock.Checked;
		}
	}
}
