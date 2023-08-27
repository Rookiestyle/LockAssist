using System.Drawing;
using System.Windows.Forms;
using KeePassLib.Security;
using PluginTools;
using PluginTranslation;

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
      bCancel.Text = KeePass.Resources.KPRes.Cancel;

      KeePass.UI.SecureTextBoxEx.InitEx(ref stbPIN);
      cbTogglePin.Checked = true;
      stbPIN.EnableProtection(cbTogglePin.Checked);

      cbContinueUnlock.Text = KeePass.Resources.KPRes.LockMenuUnlock;
      cbContinueUnlock.Visible = cbContinueUnlock.Checked = LockWorkspace.GetContinueUnlock();
    }

    public ProtectedString QuickUnlockKey
    {
      get { return stbPIN.TextEx; }
    }

    private void togglePIN_CheckedChanged(object sender, System.EventArgs e)
    {
      stbPIN.EnableProtection(cbTogglePin.Checked);
    }

    private void cbContinueUnlock_CheckedChanged(object sender, System.EventArgs e)
    {
      LockWorkspace.SetContinueUnlock(cbContinueUnlock.Checked);
    }
  }
}
