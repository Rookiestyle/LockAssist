using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeePassLib;

namespace LockAssist
{
  //Generic part
  internal partial class LockAssistConfig
  {
    public static KeePass.App.Configuration.AceCustomConfig _config = KeePass.Program.Config.CustomConfig;
  }
}
