using KeePass.Plugins;
using KeePassLib;
using System.Collections.Generic;
using System.Windows.Forms;

using PluginTranslation;
using PluginTools;
using System.Drawing;
using System;
using KeePass.UI;
using KeePass.Forms;

namespace LockAssist
{
	public partial class LockAssistExt : Plugin
	{
		internal const string c_OptionsMenuItemName = "LockAssistPlugin_Options_MenuItem";
		private static IPluginHost m_host = null;
		private ToolStripMenuItem m_menu = null;
		private static bool Terminated { get { return m_host == null; } }

		private QuickUnlock _qu = null;
		private LockWorkspace _lw = null;
		private SoftLock _sl = null;

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
			m_menu.Name = c_OptionsMenuItemName;
			var tsbLockWorkspace = (ToolStripButton)Tools.GetField("m_tbLockWorkspace", m_host.MainWindow);
			if (tsbLockWorkspace != null) m_menu.Image = (Image)tsbLockWorkspace.Image;
			else m_menu.Image = m_host.MainWindow.ClientIcons.Images[(int)PwIcon.LockOpen];
			m_host.MainWindow.ToolsMenu.DropDownItems.Add(m_menu);

			Tools.OptionsFormShown += OptionsFormShown;
			Tools.OptionsFormClosed += OptionsFormClosed;

			GlobalWindowManager.WindowAdded += OnWindowAdded;

			_qu = new QuickUnlock();
			_lw = new LockWorkspace();
			_sl = new SoftLock(_qu);

			return true;
		}

        private void OnWindowAdded(object sender, GwmWindowEventArgs e)
        {
			if (!(e.Form is KeyPromptForm) && !(e.Form is KeyCreationForm)) return;
			PluginDebug.AddInfo(e.Form.GetType().Name + " added", 0);
			e.Form.Shown += OnKeyFormShown;
		}

        private void OnKeyFormShown(object sender, EventArgs e)
        {
			Form f = sender as Form;
			if (f == null) return;
			if (LockAssistConfig.LW_Active)
			{
				if ((f is KeyPromptForm) && LockWorkspace.ShallStopGlobalUnlock())
				{
					GlobalWindowManager.RemoveWindow(sender as Form);
					f.Close();
					f.Dispose();
					_lw.OnEnhancedWorkspaceLockUnlock(sender, null);
					return;
				}
				LockWorkspace.OnKeyFormShown(f, e);
			}
			QuickUnlock.OnKeyFormShown(f, false);
        }

        public override void Terminate()
		{
			Tools.OptionsFormShown -= OptionsFormShown;
			Tools.OptionsFormClosed -= OptionsFormClosed;

			GlobalWindowManager.WindowAdded -= OnWindowAdded;

			_lw.Clear();
			_lw = null;
			_qu.Clear();
			_qu = null;
			_sl.Clear();
			_sl = null;

			m_host.MainWindow.ToolsMenu.DropDownItems.Remove(m_menu);
			PluginDebug.SaveOrShow();
			m_host = null;
		}

		#region Options
		private void OptionsFormShown(object sender, Tools.OptionsFormsEventArgs e)
		{
			PluginDebug.AddInfo("Show options", 0);
			OptionsForm options = new OptionsForm();
			options.InitEx(LockAssistConfig.GetQuickUnlockOptions(m_host.Database));
			_sl.EnsureLockedOptions(e.form);
			Tools.AddPluginToOptionsForm(this, options);
		}

		private void OptionsFormClosed(object sender, Tools.OptionsFormsEventArgs e)
		{
			if (e.form.DialogResult != DialogResult.OK) return;
			bool shown = false;
			OptionsForm options = (OptionsForm)Tools.GetPluginFromOptions(this, out shown);
			if (!shown) return;
			var MyOptions = LockAssistConfig.GetQuickUnlockOptions(m_host.Database);
			LockAssistConfig NewOptions = options.GetQuickUnlockOptions();
			bool changedConfig = MyOptions.QU_ConfigChanged(NewOptions, false);
			bool changedConfigTotal = MyOptions.QU_ConfigChanged(NewOptions, true);
			PluginDebug.AddInfo("Options form closed", 0, "Config changed: " + changedConfig.ToString(), "Config total changed:" + changedConfigTotal.ToString());
		
			bool SwitchToNoDBSpecific = MyOptions.QU_CopyFrom(NewOptions);

            if (SwitchToNoDBSpecific)
			{
				string sQuestion = string.Format(PluginTranslate.OptionsSwitchDBToGeneral, KeePass.Resources.KPRes.Yes, KeePass.Resources.KPRes.No);
				if (Tools.AskYesNo(sQuestion) == DialogResult.No)
					//Make current configuration the new global configuration
					MyOptions.QU_WriteConfig(null, false);
				//Remove DB specific configuration
				MyOptions.QU_DeleteDBConfig(m_host.Database);
			}
			else MyOptions.QU_WriteConfig(m_host.Database, changedConfigTotal);

			if (LockAssistConfig.LW_Active != options.GetLockWorkspace())
			{
				LockAssistConfig.LW_Active = !LockAssistConfig.LW_Active;
				_lw.ActivateNewLockWorkspace(LockAssistConfig.LW_Active);
			}

			var slsResult = options.GetSoftLockOptions();
			LockAssistConfig.SL_Active = slsResult.Active;
			LockAssistConfig.SL_Seconds = slsResult.Seconds;
			LockAssistConfig.SL_OnMinimize = slsResult.SoftLockOnMinimize;
			_sl.CheckSoftlockMode();
		}
#endregion

		public override string UpdateUrl
		{
			get { return "https://raw.githubusercontent.com/rookiestyle/lockassist/master/version.info"; }
		}

		public override Image SmallIcon { get { return m_menu.Image; } }
	}
}