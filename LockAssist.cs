using KeePass.Plugins;
using KeePassLib;
using System.Collections.Generic;
using System.Windows.Forms;

using PluginTranslation;
using PluginTools;
using System.Drawing;

namespace LockAssist
{
	public class LockAssistOptions
	{
		public IPluginHost host;
		public bool FirstTime = false;
		public bool QUActive = false;
		public bool DBSpecific = false;
		public bool UsePassword = true;
		public int Length = 4;
		public bool FromEnd = true;
		public bool SLMinimize = false;
		public int SLSeconds = 60;
		public bool SLIdle = true;
		public bool SLIdleActive { get { return SLIdle && (SLSeconds > 0); } }
		public List<string> SLExcludeForms = new List<string>()
		{
			"AboutForm", "AutoTypeCtxForm", "CharPickerForm", "ColumnsForm",
			"HelpSourceForm", "KeyPromptForm", "LanguageForm", "PluginsForm",
			"PwGeneratorForm", "UpdateCheckForm"
		};
		public bool LockWorkspace = false;

		public bool ConfigChanged(LockAssistOptions comp, bool CheckDBSpecific)
		{
			if (QUActive != comp.QUActive) return true;
			if (CheckDBSpecific && (DBSpecific != comp.DBSpecific)) return true;
			if (UsePassword != comp.UsePassword) return true;
			if (Length != comp.Length) return true;
			if (FromEnd != comp.FromEnd) return true;
			if (SLMinimize != comp.SLMinimize) return true;
			if (SLIdle != comp.SLIdle) return true;
			if (SLSeconds != comp.SLSeconds) return true;
			if (LockWorkspace != comp.LockWorkspace) return true;
			return false;
		}

		public bool CopyFrom(LockAssistOptions NewOptions)
		{
			bool SwitchToNoDBSpecific = DBSpecific && !NewOptions.DBSpecific;
			DBSpecific = NewOptions.DBSpecific;
			QUActive = NewOptions.QUActive;
			UsePassword = NewOptions.UsePassword;
			Length = NewOptions.Length;
			FromEnd = NewOptions.FromEnd;
			SLMinimize = NewOptions.SLMinimize;
			SLIdle = NewOptions.SLIdle;
			SLSeconds = NewOptions.SLSeconds;
			LockWorkspace = NewOptions.LockWorkspace;
			return SwitchToNoDBSpecific;
		}

		public void ReadConfig()
		{
			ReadConfig(host.Database);
		}

		public void ReadConfig(PwDatabase db)
		{
			FirstTime = host.CustomConfig.GetBool("LockAssist.FirstTime", true);
			DBSpecific = (db != null) && (db.IsOpen) && db.CustomData.Exists("LockAssist.UsePassword");
			if (DBSpecific)
			{
				QUActive = db.CustomData.Get("LockAssist.Active") == "true";
				UsePassword = db.CustomData.Get("LockAssist.UsePassword") == "true";
				if (!int.TryParse(db.CustomData.Get("LockAssist.KeyLength"), out Length)) Length = 4;
				FromEnd = db.CustomData.Get("LockAssist.KeyFromEnd") == "false";
			}
			else
			{
				QUActive = host.CustomConfig.GetBool("LockAssist.Active", false);
				UsePassword = host.CustomConfig.GetBool("LockAssist.UsePassword", true);
				Length = (int)host.CustomConfig.GetLong("LockAssist.KeyLength", 4);
				FromEnd = host.CustomConfig.GetBool("LockAssist.KeyFromEnd", true);
			}

			SLIdle = host.CustomConfig.GetBool("LockAssist.SoftlockMode.HideOnIdle", true);
			SLSeconds = (int)host.CustomConfig.GetLong("LockAssist.SoftlockMode.Seconds", 0);
			SLMinimize = host.CustomConfig.GetBool("LockAssist.SoftlockMode.HideOnMinimize", false);
			string excludeForms = host.CustomConfig.GetString("LockAssist.SoftlockMode.ExcludeForms", "");
			if (!string.IsNullOrEmpty(excludeForms))
			{
				SLExcludeForms.Clear();
				SLExcludeForms.AddRange(excludeForms.Split(new char[1] { ',' }));
			}
			LockWorkspace = host.CustomConfig.GetBool("LockAssist.LockWorkspace", false);
		}

		public void WriteConfig()
		{
			WriteConfig(host.Database);
		}

		public void WriteConfig(PwDatabase db)
		{
			if (DBSpecific)
			{
				host.Database.CustomData.Set("LockAssist.Active", QUActive ? "true" : "false");
				host.Database.CustomData.Set("LockAssist.UsePassword", UsePassword ? "true" : "false");
				host.Database.CustomData.Set("LockAssist.KeyLength", Length.ToString());
				host.Database.CustomData.Set("LockAssist.KeyFromEnd", FromEnd ? "true" : "false");
			}
			else
			{
				host.CustomConfig.SetBool("LockAssist.Active", QUActive);
				host.CustomConfig.SetBool("LockAssist.UsePassword", UsePassword);
				host.CustomConfig.SetLong("LockAssist.KeyLength", Length);
				host.CustomConfig.SetBool("LockAssist.KeyFromEnd", FromEnd);
				DeleteDBConfig();
			}
			host.CustomConfig.SetBool("LockAssist.SoftlockMode.HideOnIdle", SLIdle);
			host.CustomConfig.SetLong("LockAssist.SoftlockMode.Seconds", SLSeconds);
			host.CustomConfig.SetBool("LockAssist.SoftlockMode.HideOnMinimize", SLMinimize);
			string excludeForms = string.Empty;
			foreach (string form in SLExcludeForms)
				excludeForms += "," + form;
			excludeForms = excludeForms.Substring(1);
			host.CustomConfig.SetString("LockAssist.SoftlockMode.ExcludeForms", excludeForms);
			host.CustomConfig.SetBool("LockAssist.LockWorkspace", LockWorkspace);
		}

		public void DeleteDBConfig()
		{
			bool deleted = host.Database.CustomData.Remove("LockAssist.Active");
			deleted |= deleted = host.Database.CustomData.Remove("LockAssist.UsePassword");
			deleted |= host.Database.CustomData.Remove("LockAssist.KeyLength");
			deleted |= host.Database.CustomData.Remove("LockAssist.KeyFromEnd");
			if (deleted)
			{
				host.MainWindow.UpdateUI(false, null, false, null, false, null, true);
				host.Database.Modified = true;
			}
		}
	}

	public partial class LockAssistExt : Plugin, IMessageFilter
	{
		private static IPluginHost m_host = null;
		private QuickUnlockKeyProv m_kp = null;
		private ToolStripMenuItem m_menu = null;
		private static bool Terminated { get { return m_host == null; } }

		private LockAssistOptions m_options;

		public override bool Initialize(IPluginHost host)
		{
			if (m_host != null) Terminate();
			m_host = host;

			PluginTranslate.Init(this, KeePass.Program.Translation.Properties.Iso6391Code);
			Tools.DefaultCaption = PluginTranslate.PluginName;

			m_menu = new ToolStripMenuItem();
			m_menu.Text = PluginTranslate.PluginName + "...";
			m_menu.Click += (o, e) => Tools.ShowOptions();
			m_menu.Image = m_host.MainWindow.ClientIcons.Images[51];
			m_host.MainWindow.ToolsMenu.DropDownItems.Add(m_menu);

			Tools.OptionsFormShown += OptionsFormShown;
			Tools.OptionsFormClosed += OptionsFormClosed;
			m_options = new LockAssistOptions();
			m_options.host = m_host;
			m_options.ReadConfig();

			QuickUnlock_Init();
			Softlock_Init();
			LockWorkspace_Init();

			return true;
		}

		public override void Terminate()
		{
			Application.RemoveMessageFilter(this);
			Tools.OptionsFormShown -= OptionsFormShown;
			Tools.OptionsFormClosed -= OptionsFormClosed;

			QuickUnlock_Terminate();
			Softlock_Terminate();
			LockWorkspace_Terminate();

			m_host.MainWindow.ToolsMenu.DropDownItems.Remove(m_menu);
			m_host = null;
		}

		#region Options
		private void OptionsFormShown(object sender, Tools.OptionsFormsEventArgs e)
		{
			m_options.ReadConfig();
			OptionsForm options = new OptionsForm();
			options.SetOptions(m_options);
			Tools.AddPluginToOptionsForm(this, options);
		}

		private void OptionsFormClosed(object sender, Tools.OptionsFormsEventArgs e)
		{
			if (e.form.DialogResult != DialogResult.OK) return;
			bool shown = false;
			OptionsForm options = (OptionsForm)Tools.GetPluginFromOptions(this, out shown);
			if (!shown) return;
			LockAssistOptions NewOptions = options.GetOptions();
			bool changedConfig = m_options.ConfigChanged(NewOptions, false);
			bool changedConfigTotal = m_options.ConfigChanged(NewOptions, true);
			if (m_SLHide)
			{
				if (changedConfig) Tools.ShowError(PluginTranslate.OptionsNotSavedSoftlock);
				return;
			}
			if (m_options.LockWorkspace != NewOptions.LockWorkspace)
				ActivateNewLockWorkspace(NewOptions.LockWorkspace);
			bool SwitchToNoDBSpecific = m_options.CopyFrom(NewOptions);
			CheckSoftlockMode();
			if (SwitchToNoDBSpecific)
			{
				if (Tools.AskYesNo(PluginTranslate.OptionsSwitchDBToGeneral) == DialogResult.Yes)
				{
					//Remove DB specific configuration
					m_options.DeleteDBConfig();
					m_options.ReadConfig();
				}
				else
				{
					//Make current configuration the new global configuration
					m_options.WriteConfig();
				}
			}
			else
				m_options.WriteConfig();
			CheckSoftlockMode();
			if ((changedConfigTotal && m_options.DBSpecific) || SwitchToNoDBSpecific)
			{
				m_host.MainWindow.UpdateUI(false, null, false, null, false, null, true);
				m_host.Database.Modified = true;
			}
		}
		#endregion

		public override string UpdateUrl
		{
			get { return "https://firebasestorage.googleapis.com/v0/b/rookiestyle-43398.appspot.com/o/versioninfo.txt?alt=media&token=89da60c0-f331-4334-94bb-0dccf617818f"; }
		}

		public override Image SmallIcon { get { return m_menu.Image; } }
	}
}