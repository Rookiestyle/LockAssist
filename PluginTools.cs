using KeePass.Forms;
using KeePass.UI;
using KeePassLib.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace PluginTools
{
	public static class Tools
	{
		private static Version m_KPVersion = null;
		public static Version KeePassVersion { get { return m_KPVersion; } }
		public static string DefaultCaption = string.Empty;
		public static string PluginURL = string.Empty;

		static Tools()
		{
			KeePass.UI.GlobalWindowManager.WindowAdded += OnWindowAdded;
			KeePass.UI.GlobalWindowManager.WindowRemoved += OnWindowRemoved;
			m_KPVersion = typeof(KeePass.Program).Assembly.GetName().Version;
		}

		#region Form and field handling
		public static object GetField(string field, object obj)
		{
			BindingFlags bf = BindingFlags.Instance | BindingFlags.NonPublic;
			return GetField(field, obj, bf);
		}

		public static object GetField(string field, object obj, BindingFlags bf)
		{
			if (obj == null) return null;
			FieldInfo fi = obj.GetType().GetField(field, bf);
			if (fi == null) return null;
			return fi.GetValue(obj);
		}

		public static Control GetControl(string control)
		{
			return GetControl(control, KeePass.Program.MainForm);
		}

		public static Control GetControl(string control, Control form)
		{
			if (form == null) return null;
			if (string.IsNullOrEmpty(control)) return null;
			Control[] cntrls = form.Controls.Find(control, true);
			if (cntrls.Length == 0) return null;
			return cntrls[0];
		}
		#endregion

		#region Plugin options and instance
		public static object GetPluginInstance(string PluginName)
		{
			string comp = PluginName + "." + PluginName + "Ext";
			BindingFlags bf = BindingFlags.Instance | BindingFlags.NonPublic;
			try
			{
				var PluginManager = GetField("m_pluginManager", KeePass.Program.MainForm);
				var PluginList = GetField("m_vPlugins", PluginManager);
				MethodInfo IteratorMethod = PluginList.GetType().GetMethod("System.Collections.Generic.IEnumerable<T>.GetEnumerator", bf);
				IEnumerator<object> PluginIterator = (IEnumerator<object>)(IteratorMethod.Invoke(PluginList, null));
				while (PluginIterator.MoveNext())
				{
					object result = GetField("m_pluginInterface", PluginIterator.Current);
					if (comp == result.GetType().ToString()) return result;
				}
			}

			catch (Exception) { }
			return null;
		}

		public static event EventHandler<OptionsFormsEventArgs> OptionsFormShown;
		public static event EventHandler<OptionsFormsEventArgs> OptionsFormClosed;

		private static bool OptionsEnabled = (KeePass.Program.Config.UI.UIFlags & (ulong)KeePass.App.Configuration.AceUIFlags.DisableOptions) != (ulong)KeePass.App.Configuration.AceUIFlags.DisableOptions;
		private static bool m_ActivatePluginTab = false;
		private static OptionsForm m_of = null;
		private const string c_tabRookiestyle = "m_tabRookiestyle";
		private const string c_tabControlRookiestyle = "m_tabControlRookiestyle";
		private static string m_TabPageName = string.Empty;
		private static bool m_OptionsShown = false;
		private static bool m_PluginContainerShown = false;

		public static void AddPluginToOptionsForm(KeePass.Plugins.Plugin p, UserControl uc)
		{
			m_OptionsShown = m_PluginContainerShown = false;
			TabPage tPlugin = new TabPage(DefaultCaption);
			tPlugin.CreateControl();
			tPlugin.Name = m_TabPageName = c_tabRookiestyle + p.GetType().Name;
			uc.Dock = DockStyle.Fill;
			uc.Padding = new Padding(15, 10, 15, 10);
			tPlugin.Controls.Add(uc);
			TabControl tcPlugins = AddPluginTabContainer();
			int i = 0;
			bool insert = false;
			for (int j = 0; j < tcPlugins.TabPages.Count; j++)
			{
				if (string.Compare(tPlugin.Text, tcPlugins.TabPages[j].Text, StringComparison.CurrentCultureIgnoreCase) < 0)
				{
					i = j;
					insert = true;
					break;
				}
			}
			if (!insert) i = tcPlugins.TabPages.Count;
			tcPlugins.TabPages.Insert(i, tPlugin);
			if (p.SmallIcon != null)
			{
				tcPlugins.ImageList.Images.Add(tPlugin.Name, p.SmallIcon);
				tPlugin.ImageKey = tPlugin.Name;
			}
			TabControl tcMain = Tools.GetControl("m_tabMain", m_of) as TabControl;
			if (m_ActivatePluginTab)
			{
				tcMain.SelectedIndex = tcMain.TabPages.IndexOfKey(c_tabRookiestyle);
				KeePass.Program.Config.Defaults.OptionsTabIndex = (uint)tcMain.SelectedIndex;
				tcPlugins.SelectedIndex = tcPlugins.TabPages.IndexOf(tPlugin);
			}
			m_ActivatePluginTab = false;
			if (!string.IsNullOrEmpty(PluginURL))
				AddPluginLink(uc);
		}

		private static void OnPluginTabsSelected(object sender, TabControlEventArgs e)
		{
			m_OptionsShown |= (e.TabPage.Name == m_TabPageName);
			m_PluginContainerShown |= (m_OptionsShown || (e.TabPage.Name == c_tabRookiestyle));
		}

		public static UserControl GetPluginFromOptions(KeePass.Plugins.Plugin p, out bool PluginOptionsShown)
		{
			PluginOptionsShown = m_OptionsShown && m_PluginContainerShown;
			TabPage tPlugin = Tools.GetControl(c_tabRookiestyle + p.GetType().Name, m_of) as TabPage;
			if (tPlugin == null) return null;
			return tPlugin.Controls[0] as UserControl;
		}

		public static void ShowOptions()
		{
			m_ActivatePluginTab = true;
			if (OptionsEnabled)
				KeePass.Program.MainForm.ToolsMenu.DropDownItems["m_menuToolsOptions"].PerformClick();
			else
			{
				m_of = new OptionsForm();
				m_of.InitEx(KeePass.Program.MainForm.ClientIcons);
				m_of.ShowDialog();
			}
		}

		private static void AddPluginLink(UserControl uc)
		{
			LinkLabel llUrl = new LinkLabel();
			llUrl.Links.Add(0, 0, PluginURL);
			llUrl.Text = PluginURL;
			uc.Controls.Add(llUrl);
			llUrl.Dock = DockStyle.Bottom;
			llUrl.LinkClicked += new LinkLabelLinkClickedEventHandler(PluginURLClicked);
		}

		private static void PluginURLClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string target = e.Link.LinkData as string;
			System.Diagnostics.Process.Start(target);
		}

		private static void OnOptionsFormShown(object sender, EventArgs e)
		{
			m_of.Shown -= OnOptionsFormShown;
			TabControl tcMain = Tools.GetControl("m_tabMain", m_of) as TabControl;
			if (!tcMain.TabPages.ContainsKey(c_tabRookiestyle)) return;
			TabPage tPlugins = tcMain.TabPages[c_tabRookiestyle];
			TabControl tcPlugins = Tools.GetControl(c_tabControlRookiestyle, tPlugins) as TabControl;
			tcMain.Selected += OnPluginTabsSelected;
			tcPlugins.Selected += OnPluginTabsSelected;
			tcMain.ImageList.Images.Add(c_tabRookiestyle + "Icon", (Image)KeePass.Program.Resources.GetObject("B16x16_BlockDevice"));
			tPlugins.ImageKey = c_tabRookiestyle + "Icon";
			m_PluginContainerShown |= tcMain.SelectedTab == tPlugins;
			m_OptionsShown |= (tcPlugins.SelectedTab.Name == m_TabPageName);
		}

		private static void OnWindowAdded(object sender, KeePass.UI.GwmWindowEventArgs e)
		{
			if (OptionsFormShown == null) return;
			if (e.Form is OptionsForm)
			{
				m_of = e.Form as OptionsForm;
				m_of.Shown += OnOptionsFormShown;
				OptionsFormsEventArgs o = new OptionsFormsEventArgs(m_of);
				OptionsFormShown(sender, o);
			}
		}

		private static void OnWindowRemoved(object sender, KeePass.UI.GwmWindowEventArgs e)
		{
			if (OptionsFormClosed == null) return;
			if (e.Form is OptionsForm)
			{
				OptionsFormsEventArgs o = new OptionsFormsEventArgs(m_of);
				OptionsFormClosed(sender, o);
			}
		}

		private static TabControl AddPluginTabContainer()
		{
			TabControl tcMain = Tools.GetControl("m_tabMain", m_of) as TabControl;
			TabPage tPlugins = null;
			TabControl tcPlugins = null;
			if (tcMain.TabPages.ContainsKey(c_tabRookiestyle))
			{
				tPlugins = tcMain.TabPages[c_tabRookiestyle];
				tcPlugins = (TabControl)tPlugins.Controls[c_tabControlRookiestyle];
			}
			else
			{
				tPlugins = new TabPage(KeePass.Resources.KPRes.Plugin + " " + m_of.Text);
				tPlugins.Name = c_tabRookiestyle;
				tPlugins.CreateControl();
				if (!OptionsEnabled)
				{
					while (tcMain.TabCount > 0)
						tcMain.TabPages.RemoveAt(0);
				}
				tcMain.TabPages.Add(tPlugins);
				tcPlugins = new TabControl();
				tcPlugins.Name = c_tabControlRookiestyle;
				tcPlugins.Dock = DockStyle.Fill;
				tcPlugins.Multiline = true;
				tcPlugins.CreateControl();
				if (tcPlugins.ImageList == null)
					tcPlugins.ImageList = new ImageList();
				tPlugins.Controls.Add(tcPlugins);
			}
			return tcPlugins;
		}

		public class OptionsFormsEventArgs : EventArgs
		{
			public Form form;

			public OptionsFormsEventArgs(Form form)
			{
				this.form = form;
			}
		}
		#endregion

		#region MessageBox shortcuts
		public static DialogResult ShowError(string msg)
		{
			return ShowError(msg, DefaultCaption);
		}

		public static DialogResult ShowInfo(string msg)
		{
			return ShowInfo(msg, DefaultCaption);
		}

		public static DialogResult AskYesNo(string msg)
		{
			return AskYesNo(msg, DefaultCaption);
		}

		public static DialogResult ShowError(string msg, string caption)
		{
			return MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static DialogResult ShowInfo(string msg, string caption)
		{
			return MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public static DialogResult AskYesNo(string msg, string caption)
		{
			return MessageBox.Show(msg, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
		}
		#endregion

		#region GlobalWindowManager
		public static void GlobalWindowManager(Form form)
		{
			if ((form == null) || (form.IsDisposed)) return;
			form.Load += FormLoaded;
			form.FormClosed += FormClosed;
		}

		private static void FormLoaded(object sender, EventArgs e)
		{
			KeePass.UI.GlobalWindowManager.AddWindow(sender as Form);
		}

		private static void FormClosed(object sender, FormClosedEventArgs e)
		{
			KeePass.UI.GlobalWindowManager.RemoveWindow(sender as Form);
		}
		#endregion
	}

	public static class DPIAwareness
	{
		public static readonly Size Size16 = new Size(DpiUtil.ScaleIntX(16), DpiUtil.ScaleIntY(16));

		public static Image Scale16x16(Image img)
		{
			return Scale(img, 16, 16);
		}

		public static Image Scale(Image img, int x, int y)
		{
			if (img == null) return null;
			return GfxUtil.ScaleImage(img, DpiUtil.ScaleIntX(x), DpiUtil.ScaleIntY(y));
		}
	}
}