using KeePassLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockAssist
{
	//Generic part
	internal partial class LockAssistConfig
	{
		public static KeePass.App.Configuration.AceCustomConfig _config = KeePass.Program.Config.CustomConfig;
	}
}
