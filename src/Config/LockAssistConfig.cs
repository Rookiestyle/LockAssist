using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeePass.UI;
using KeePassLib;
using KeePassLib.Delegates;

namespace LockAssist
{
  //Generic part
  internal partial class LockAssistConfig
  {
    public static KeePass.App.Configuration.AceCustomConfig _config = KeePass.Program.Config.CustomConfig;
  }
}

namespace PluginTools
{
  public static partial class Tools
  {
    public static DialogResult ShowDialog<TForm, TResult>(bool bProtect,
      GFunc<TForm> fnConstruct, GFunc<TForm, TResult> fnResultBuilder,
      out TResult r)
      where TForm : Form
      where TResult : class
    {
      if (fnConstruct == null) { throw new ArgumentNullException("fnConstruct"); }
      if (fnResultBuilder == null) { throw new ArgumentNullException("fnResultBuilder"); }

      r = null;

      if (!bProtect)
      {
        TForm tf = fnConstruct();
        if (tf == null) { return DialogResult.None; }

        try
        {
          DialogResult drDirect = tf.ShowDialog();
          r = fnResultBuilder(tf); // Always
          return drDirect;
        }
        finally { UIUtil.DestroyForm(tf); }
      }

      UIFormConstructor fnUifC = delegate (object objParam)
      {
        return fnConstruct();
      };

      UIFormResultBuilder fnUifRB = delegate (Form f)
      {
        TForm tf = (f as TForm);
        if (tf == null) { return null; }

        return fnResultBuilder(tf);
      };

      ProtectedDialog dlg = new ProtectedDialog(fnUifC, fnUifRB);

      object objResult;
      DialogResult dr = dlg.ShowDialog(out objResult, null);
      r = (objResult as TResult);
      return dr;
    }

  }
}
