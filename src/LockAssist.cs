using KeePass.Plugins;
using KeePassLib;
using System.Collections.Generic;
using System.Windows.Forms;

using PluginTranslation;
using PluginTools;
using System.Drawing;
using System;

namespace LockAssist
{
	public partial class LockAssistExt : Plugin
	{
		private static IPluginHost m_host = null;
		private ToolStripMenuItem m_menu = null;
		private static bool Terminated { get { return m_host == null; } }

		private QuickUnlock _qu = null;

		//private static LockAssistOptions m_options;

		public override bool Initialize(IPluginHost host)
		{
			if (m_host != null) Terminate();
			m_host = host;

			PluginTranslate.Init(this, KeePass.Program.Translation.Properties.Iso6391Code);
			Tools.DefaultCaption = PluginTranslate.PluginName;
			Tools.PluginURL = "https://github.com/rookiestyle/lockassist/";

			m_menu = new ToolStripMenuItem();
			m_menu.Text = PluginTranslate.PluginName + "...";
			m_menu.Click += (o, e) => Tools.ShowOptions();
			m_menu.Image = m_host.MainWindow.ClientIcons.Images[(int)PwIcon.LockOpen];
			m_host.MainWindow.ToolsMenu.DropDownItems.Add(m_menu);

			Tools.OptionsFormShown += OptionsFormShown;
			Tools.OptionsFormClosed += OptionsFormClosed;

			_qu = new QuickUnlock();

			return true;
		}

		public override void Terminate()
		{
			Tools.OptionsFormShown -= OptionsFormShown;
			Tools.OptionsFormClosed -= OptionsFormClosed;

			_qu.Clear();
			_qu = null;

			m_host.MainWindow.ToolsMenu.DropDownItems.Remove(m_menu);
			PluginDebug.SaveOrShow();
			m_host = null;
		}

		#region Options
		private void OptionsFormShown(object sender, Tools.OptionsFormsEventArgs e)
		{
			PluginDebug.AddInfo("Show options", 0);
			OptionsForm options = new OptionsForm();
			options.InitEx(LockAssistConfig.GetOptions(m_host.Database));
			Tools.AddPluginToOptionsForm(this, options);
		}

		private void OptionsFormClosed(object sender, Tools.OptionsFormsEventArgs e)
		{
			if (e.form.DialogResult != DialogResult.OK) return;
			bool shown = false;
			OptionsForm options = (OptionsForm)Tools.GetPluginFromOptions(this, out shown);
			if (!shown) return;
			var MyOptions = LockAssistConfig.GetOptions(m_host.Database);
			LockAssistConfig NewOptions = options.GetOptions();
			bool changedConfig = MyOptions.ConfigChanged(NewOptions, false);
			bool changedConfigTotal = MyOptions.ConfigChanged(NewOptions, true);
			PluginDebug.AddInfo("Options form closed", 0, "Config changed: " + changedConfig.ToString(), "Config total changed:" + changedConfigTotal.ToString());
		
			bool SwitchToNoDBSpecific = MyOptions.CopyFrom(NewOptions);

            if (SwitchToNoDBSpecific)
			{
				string sQuestion = string.Format(PluginTranslate.OptionsSwitchDBToGeneral, DialogResult.Yes.ToString(), DialogResult.No.ToString());
				if (Tools.AskYesNo(sQuestion) == DialogResult.No)
					//Make current configuration the new global configuration
					MyOptions.WriteConfig(null);
				//Remove DB specific configuration
				MyOptions.DeleteDBConfig(m_host.Database);
			}
			else MyOptions.WriteConfig(m_host.Database);
		}
#endregion

		public override string UpdateUrl
		{
			get { return "https://raw.githubusercontent.com/rookiestyle/lockassist/master/version.info"; }
		}

		public override Image SmallIcon { get { return m_menu.Image; } }
	}
}