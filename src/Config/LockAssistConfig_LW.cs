using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeePassLib;

namespace LockAssist
{
  //Lock workspace part
  internal partial class LockAssistConfig
  {
    private const string LockAssistLockWorkspaceActive = "LockAssist.LockWorkspaceActive";

    public static bool LW_Active
    {
      get { return _config.GetBool(LockAssistLockWorkspaceActive, true); }
      set { _config.SetBool(LockAssistLockWorkspaceActive, value); }
    }
  }
}
