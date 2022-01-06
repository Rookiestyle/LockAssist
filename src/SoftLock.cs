using KeePass;
using KeePass.Forms;
using KeePass.UI;
using KeePassLib.Security;
using PluginTools;
using PluginTranslation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace LockAssist
{
	internal struct SoftLockSettings
    {
		internal bool Active;
		internal int Seconds;
		internal bool SoftLockOnMinimize;
    }

    internal class SoftLock : IMessageFilter
    {
		private MethodInfo m_miApplyUICustomizations = null;
		private Timer m_SLTimer = new Timer();
		private List<int> m_lMessagesForTimerRestart = null;

        private bool m_SoftLocked = false;
        private UnlockForm m_UnlockForm = null;
		private Dictionary<Form, SLFormProperties> m_dHiddenForms = new Dictionary<Form, SLFormProperties>();

		private QuickUnlock _qu = null;

		private static KeePassLib.PwUuid _uuidCustomTbButtonClicked = KeePassLib.PwUuid.Zero;

		static SoftLock()
        {
			var x = typeof(Program).Assembly.GetType("KeePass.Ecas.EcasEventIDs");
			if (x == null) return;
			var u = x.GetField("CustomTbButtonClicked").GetValue(null) as KeePassLib.PwUuid;
			if (u != null) _uuidCustomTbButtonClicked = u;
		}

		internal SoftLock(QuickUnlock qu)
        {
			Init(qu);
        }

		private void Init(QuickUnlock qu)
		{
			FillMessages();
			_qu = qu;
			Program.MainForm.Resize += OnMinimize; //To allow Softlock mode on minimizing
			Program.MainForm.UIStateUpdated += OnUIStateUpdated; //Update menu and toolbar on switching databases, ...
			Program.MainForm.FileClosed += CheckSoftlockMode;
			Program.MainForm.FileOpened += CheckSoftlockMode;
			m_SLTimer.Enabled = false; //Activate only after opening at least one database
			m_SLTimer.Tick += OnTimerTick;
			Program.MainForm.TrayContextMenu.Opening += OnTrayOpening;
			m_miApplyUICustomizations = Program.MainForm.GetType().GetMethod("ApplyUICustomizations", BindingFlags.Instance | BindingFlags.NonPublic);
			if (m_miApplyUICustomizations == null) PluginDebug.AddError("Cound not locate method 'ApplyUICustomizations'", 0);
		}

		internal void EnsureLockedOptions(Form fOptions)
        {
			//If Softlock is active, the "Options..." menu is disabled
			//Might be that another plugin will display the form directly
			//
			//Make all elements except TabControl ansd TabPages readonly
			if (!m_SoftLocked || fOptions == null) return;
			PluginDebug.AddInfo("KeePass options shown", 0, "SoftLock active: true", "Locking options");
			foreach (Control c in fOptions.Controls)
			{
				if (!(c is TabControl))
				{
					c.Enabled = !(c is Button) || (c as Button).DialogResult == DialogResult.Cancel;
					continue;
				}
				foreach (TabPage tp in (c as TabControl).TabPages)
				{
					foreach (Control c2 in tp.Controls) c2.Enabled = false;
				}
			}
		}
        private void OnTrayOpening(object sender, CancelEventArgs e)
        {
            try { Program.MainForm.TrayContextMenu.Items["m_ctxTrayOptions"].Enabled &= !m_SoftLocked; }
            catch { }
        }

		//If timer ticks, we need to softlock
		//Timer will be reset in method PreFilterMessage if required
		private void OnTimerTick(object sender, EventArgs e)
		{
			m_SLTimer.Enabled = false;

			//Activate softlock
			if (!m_SoftLocked && Program.MainForm.IsAtLeastOneFileOpen())
			{
				//Only enter softlock mode if a Quick Unlock PIN is available...
				ProtectedString QuickUnlockKey = _qu.GetQuickUnlockKey(Program.MainForm.ActiveDatabase);
				if (QuickUnlockKey == null)
				{
					PluginDebug.AddError("Activate SoftLock", "Activation failed", "No Quick Unlock key found");
					return;
				}
				Application.RemoveMessageFilter(this);
				SetVisibility(false);
				m_SLTimer.Interval = 10000;
				return;
			}

			//Deactivate softlock
			if (!m_SoftLocked) return;

			KeyEventArgs k = e as KeyEventArgs;
			if (k != null)
			{
				if (k.KeyCode != k.KeyData) return;
				if ((k.KeyCode == Keys.LWin) || (k.KeyCode == Keys.RWin)) return;
			}
			//Unlock form already shown?
			if (m_UnlockForm != null) return;

			//Don't display unlock form if KeePass is minimized
			if (Program.MainForm.WindowState == FormWindowState.Minimized) return;

			//Don't display unlock form if an excluded form is shown
			//if ((GlobalWindowManager.WindowCount > 0) && LockAssistConfig.SL_ExcludeForms.Contains(GlobalWindowManager.TopWindow.GetType().Name)) return;

			Application.RemoveMessageFilter(this);
			m_UnlockForm = new UnlockForm();
			m_UnlockForm.Text = PluginTranslate.PluginName + " - Softlock";
			
			if (m_UnlockForm.ShowDialog(Program.MainForm) == DialogResult.OK)
			{
				ProtectedString CheckQuickUnlockKey = m_UnlockForm.QuickUnlockKey;
				ProtectedString QuickUnlockKey = _qu.GetQuickUnlockKey(Program.MainForm.ActiveDatabase);
				var lac = LockAssistConfig.GetQuickUnlockOptions(Program.MainForm.ActiveDatabase);
				if ((QuickUnlockKey == null) || CheckQuickUnlockKey.Equals(_qu.TrimQuickUnlockKey(QuickUnlockKey, lac), false))
				{
					SetVisibility(true);
					if (LockAssistConfig.SL_IsActive) m_SLTimer.Interval = LockAssistConfig.SL_Seconds * 1000;
				}
				else PluginDebug.AddError("Deactivate SoftLock", "Deactivation failed", "INvalid Quick Unlock key provided");
			}
			if (m_UnlockForm != null) m_UnlockForm.Dispose();
			m_UnlockForm = null;
			Application.AddMessageFilter(this);
		}

        internal void CheckSoftlockMode()
        {
			CheckSoftlockMode(null, null);
		}

		private void CheckSoftlockMode(object sender, EventArgs e)
        {
            Application.RemoveMessageFilter(this);
            m_SLTimer.Enabled = false;
            if (LockAssistConfig.SL_IsActive && Program.MainForm.IsAtLeastOneFileOpen())
            {
				PluginDebug.AddInfo("Enable SoftLock");
				Application.AddMessageFilter(this);
                m_SLTimer.Interval = LockAssistConfig.SL_Seconds * 1000;
                m_SLTimer.Enabled = true;
            }
            else
            {
				PluginDebug.AddInfo("Disable SoftLock");
				SetVisibility(true);
                if (m_UnlockForm != null) m_UnlockForm.Close();
                m_UnlockForm = null;
            }
        }

        internal void SetVisibility(bool bVisible)
        {
			List<string> lMsg = new List<string>();
			m_SoftLocked = !bVisible;
			lMsg.Add("SoftLock active: " + m_SoftLocked.ToString());
			if (m_SoftLocked)
			{
				m_dHiddenForms.Clear();
				foreach (Form f in Application.OpenForms)
				{
					if (f == Program.MainForm) continue;
					if (LockAssistConfig.SL_ExcludeForms.Contains(f.GetType().FullName.Replace("KeePass.Forms.", "")))
					{
						lMsg.Add("Not hiding form: " + f.GetType().FullName);
						continue;
					}
					m_dHiddenForms.Add(f, new SLFormProperties(f.Opacity));
					lMsg.Add("Hiding form: " + f.GetType().FullName);
				}
			}

			//Hide / unhide main elements
			HandlePanel(!m_SoftLocked, Program.MainForm);
			HandleMenu(!m_SoftLocked);

			//Hide all other forms
			//Changing visibility to false results in form closure which is bad...
			//Therefore hide elements inside the form and additionally try to make the form itself transparent
			foreach (KeyValuePair<Form, SLFormProperties> kvp in m_dHiddenForms)
			{
				if ((kvp.Key == null) || (kvp.Key.IsDisposed)) continue;
				try
				{
					foreach (Control c in kvp.Key.Controls)
					{
						if (!m_SoftLocked && kvp.Value.Controls.ContainsKey(c.Name))
							c.Visible = kvp.Value.Controls[c.Name]; //restore original visibility
						else
						{
							kvp.Value.Controls[c.Name] = c.Visible; //save original visibility
							c.Visible = !m_SoftLocked; //set new visibility
						}
						lMsg.Add("Set visibility " + kvp.Key.GetType().FullName+ " - " +c.Visible.ToString());
					}
					HandlePanel(!m_SoftLocked, kvp.Key);
					if (!kvp.Key.RightToLeftLayout)
						kvp.Key.Opacity = !m_SoftLocked ? kvp.Value.Opacity : .1; //0 makes the window not react on mouse input
				}
				catch (Exception ex) { lMsg.Add("Ex:" + ex.Message); }
			}
			if (!m_SoftLocked) m_dHiddenForms.Clear();
			PluginDebug.AddInfo("Toggle SoftLock", lMsg.ToArray());
		}

		private void HandleMenu(bool bVisible)
        {
			HandleMenuEntry(LockAssistExt.c_OptionsMenuItemName, bVisible);
			HandleMenuEntry("m_menuGroup", bVisible);
			HandleMenuEntry("m_menuEntry", bVisible);
			HandleMenuEntry("m_menuFind", bVisible);
			HandleMenuEntry("m_menuFileNew", bVisible);
			HandleMenuEntry("m_menuFileOpen", bVisible);
			HandleMenuEntry("m_menuFileRecent", bVisible);
			HandleMenuEntry("m_menuFileSave", bVisible);
			HandleMenuEntry("m_menuFileSaveAs", bVisible);
			HandleMenuEntry("m_menuFileDbSettings", bVisible);
			HandleMenuEntry("m_menuFileChangeMasterKey", bVisible);
			HandleMenuEntry("m_menuFilePrint", bVisible);
			HandleMenuEntry("m_menuFileImport", bVisible);
			HandleMenuEntry("m_menuFileExport", bVisible);
			HandleMenuEntry("m_menuFileSync", bVisible);
			HandleMenuEntry("m_menuEdit", bVisible);
			HandleMenuEntry("m_menuToolsDb", bVisible);
			HandleMenuEntry("m_menuToolsTriggers", bVisible);
			HandleMenuEntry("m_menuToolsOptions", bVisible);
			ToolStrip toolbar = (ToolStrip)Tools.GetControl("m_toolMain");
			if (toolbar != null)
			{
				toolbar.Items["m_tbNewDatabase"].Enabled = bVisible;
				toolbar.Items["m_tbOpenDatabase"].Enabled = bVisible;
				toolbar.Items["m_tbSaveDatabase"].Enabled = bVisible;
				toolbar.Items["m_tbSaveAll"].Enabled = bVisible;
				toolbar.Items["m_tbAddEntry"].Enabled = bVisible;
				toolbar.Items["m_tbFind"].Enabled = bVisible;
				toolbar.Items["m_tbEntryViewsDropDown"].Enabled = bVisible;
				toolbar.Items["m_tbQuickFind"].Enabled = bVisible;

				//Handle buttons for triggers
				foreach (var tItem in toolbar.Items)
                {
					if (_uuidCustomTbButtonClicked.Equals(KeePassLib.PwUuid.Zero)) break;
					ToolStripButton tsmi = tItem as ToolStripButton;
					if (tsmi == null) continue;
					if (!string.IsNullOrEmpty(tsmi.Name)) continue; //Trigger buttions don't have names
					if (string.IsNullOrEmpty((string)tsmi.Tag)) continue; //Trigger buttons have their trigger name as tag
					foreach (var t in Program.TriggerSystem.TriggerCollection)
					{
						foreach (var e in t.EventCollection)
						{
							if (!e.Type.Equals(_uuidCustomTbButtonClicked)) continue;
							//KeePass.Ecas.EcasEventType xxxx = new KeePass.Ecas.EcasEvent()
							//e.Type.Equals(KeePass.Ecas.EcasEvent.)
							if (e.Parameters.Count != 1) continue;
							if ((tsmi.Tag as string).Equals(e.Parameters[0], KeePassLib.Utility.StrUtil.CaseIgnoreCmp))
							{
								tsmi.Enabled = bVisible;
							}
						}
					}
				}
			}
			ApplyUICustomizations();
		}

		private void ApplyUICustomizations()
        {
			if (m_miApplyUICustomizations == null) return;
			m_miApplyUICustomizations.Invoke(Program.MainForm, null);
		}

        private void HandleMenuEntry(string sKey, bool bEnabled)
        {
			var tsmi = Tools.FindToolStripMenuItems(Program.MainForm.MainMenu.Items, sKey, true);
			if (tsmi.Length == 0) return;
			tsmi[0].Enabled = bEnabled;
		}

		private void HandlePanel(bool bVisible, Control c)
		{
			string buttonName = "LockAssistPlugin_SoftLock_HideButton" + c.GetType().Name + c.Name;
			Button bHide = Tools.GetControl(buttonName, c) as Button;
			if (bHide != null)
			{
				c.Controls.Remove(bHide);
				bHide.Click -= OnTimerTick;
				bHide.KeyDown -= OnTimerTick;
				bHide.Dispose();
			}
			if (bVisible) return;

			bHide = new Button();
			bHide.Name = buttonName;
			if (m_dHiddenForms.Count == 0)
				bHide.Text = PluginTranslate.SoftlockModeUnhide; //Display button on MainForm only, set text
			else if (Application.OpenForms.Count > 1 && c == Application.OpenForms[Application.OpenForms.Count - 1]) //Display button on topmost form, set text
				bHide.Text = PluginTranslate.SoftlockModeUnhideForms;

			bHide.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			bHide.Dock = DockStyle.Fill;
			bHide.Click += OnTimerTick;
			bHide.KeyDown += OnTimerTick;
			
			c.Controls.Add(bHide);
			bHide.BringToFront();
			bHide.Select();
		}
		private void OnUIStateUpdated(object sender, EventArgs e)
        {
			//Update menu when switching databases
			if (m_SoftLocked) HandleMenu(false);
        }

        private void OnMinimize(object sender, EventArgs e)
        {
			if (m_SoftLocked) return;
			if (!LockAssistConfig.SL_OnMinimize) return;
			if (Program.Config.Security.WorkspaceLocking.LockOnWindowMinimize) return;
			if (Program.Config.Security.WorkspaceLocking.LockOnWindowMinimizeToTray) return;
			if (!Program.MainForm.IsAtLeastOneFileOpen()) return;
			if (Program.MainForm.WindowState == FormWindowState.Minimized)
			{
				OnTimerTick(null, null);
				Application.AddMessageFilter(this);
			}
		}

        public void Clear()
        {
            m_SLTimer.Enabled = false;
			Program.MainForm.FileOpened -= CheckSoftlockMode;
            Program.MainForm.FileClosed -= CheckSoftlockMode;
            Program.MainForm.TrayContextMenu.Opening -= OnTrayOpening;
            //if (m_SLUnlockForm != null) m_SLUnlockForm.Dispose();
            //m_SLUnlockForm = null;
            Program.MainForm.Resize -= OnMinimize;
            Program.MainForm.UIStateUpdated -= OnUIStateUpdated;
        }

		private void FillMessages()
		{
			m_lMessagesForTimerRestart = new List<int>()
			{
				//WM_MOUSEMOVE, WM_NCMOUSEMOVE, Do NOT add it here, check seperately
				
				WM_NCLBUTTONDOWN, WM_NCLBUTTONUP, WM_NCLBUTTONDBLCLK,
				WM_NCRBUTTONDOWN, WM_NCRBUTTONUP, WM_NCRBUTTONDBLCLK, WM_NCMBUTTONDOWN,
				
				WM_LBUTTONDOWN, WM_LBUTTONUP, WM_LBUTTONDBLCLK, 
				WM_RBUTTONDOWN, WM_RBUTTONUP, WM_RBUTTONDBLCLK,
				WM_MBUTTONDOWN, WM_MBUTTONUP, WM_MBUTTONDBLCLK, 
				
				WM_MOUSEHWHEEL,
				
				WM_KEYDOWN, WM_KEYUP, WM_CHAR, WM_DEADCHAR, 
				WM_SYSKEYDOWN, WM_SYSKEYUP, WM_SYSCHAR, WM_SYSDEADCHAR,

				WM_KEYLAST, WM_UNICHAR, WM_MOVING, WM_SIZING, WM_NCHITTEST,
			};
		}

		private Message m_msgMouseMove = new Message();
		public bool PreFilterMessage(ref Message m)
		{
			bool bResetTimer = !m_SoftLocked && m_lMessagesForTimerRestart.Contains(m.Msg);

			if (!bResetTimer) bResetTimer = CheckMouseMovement(m);
			if (bResetTimer)
			{
				m_SLTimer.Enabled = false;
				m_SLTimer.Enabled = LockAssistConfig.SL_IsActive;
				//Skip message if fields are hidden and unlock form is not shown
				//to avoid changing any field value
				if (m_SoftLocked && (m_UnlockForm == null)) return true;
			}
			return false;
		}

        private bool CheckMouseMovement(Message m)
        {
			//Special check for mouse movement
			//Some devices trigger mousemove events although nothing actually is done
			if (m_msgMouseMove.Msg != m.Msg || (m.Msg != WM_MOUSEMOVE && m.Msg != WM_NCMOUSEMOVE))
			{
				m_msgMouseMove = m;
				return false;
			}
			var p1 = new Point((int)(m.LParam) & 0xFFFF, ((int)(m.LParam) >> 16) & 0xFFFF);
			var p2 = new Point((int)(m_msgMouseMove.LParam) & 0xFFFF, ((int)(m_msgMouseMove.LParam) >> 16) & 0xFFFF);
			bool bMouseMovement = Math.Abs(p1.X - p1.X) > 1 || Math.Abs(p1.Y - p2.Y) > 1;
			if (bMouseMovement) m_msgMouseMove = m;
			return bMouseMovement;
		}

        struct SLFormProperties
		{
			public double Opacity;
			public Dictionary<string, bool> Controls;

			public SLFormProperties(double Opacity)
			{
				this.Opacity = Opacity;
				Controls = new Dictionary<string, bool>();
			}
		}

		#region windows messages for SoftLock
		private const int WM_MOUSEMOVE = 0x200;
		private const int WM_NCMOUSEMOVE = 0xA0;
		private const int WM_NCLBUTTONDOWN = 0xA1;
		private const int WM_NCLBUTTONUP = 0xA2;
		private const int WM_NCLBUTTONDBLCLK = 0xA3;
		private const int WM_NCRBUTTONDOWN = 0xA4;
		private const int WM_NCRBUTTONUP = 0xA5;
		private const int WM_NCRBUTTONDBLCLK = 0xA6;
		private const int WM_NCMBUTTONDOWN = 0xA7;
		private const int WM_LBUTTONDOWN = 0x201;
		private const int WM_LBUTTONUP = 0x202;
		private const int WM_LBUTTONDBLCLK = 0x203;
		private const int WM_RBUTTONDOWN = 0x204;
		private const int WM_RBUTTONUP = 0x205;
		private const int WM_RBUTTONDBLCLK = 0x206;
		private const int WM_MBUTTONDOWN = 0x207;
		private const int WM_MBUTTONUP = 0x208;
		private const int WM_MBUTTONDBLCLK = 0x209;
		private const int WM_MOUSEWHEEL = 0x20A;
		private const int WM_MOUSEHWHEEL = 0x20E;
		private const int WM_KEYDOWN = 0x100;
		private const int WM_KEYUP = 0x101;
		private const int WM_CHAR = 0x102;
		private const int WM_DEADCHAR = 0x103;
		private const int WM_SYSKEYDOWN = 0x104;
		private const int WM_SYSKEYUP = 0x105;
		private const int WM_SYSCHAR = 0x106;
		private const int WM_SYSDEADCHAR = 0x107;
		private const int WM_KEYLAST = 0x108;
		private const int WM_UNICHAR = 0x109;
		private const int WM_MOVING = 0x216;
		private const int WM_SIZING = 0x214;
		private const int WM_NCHITTEST = 0x84;
		#endregion
	}
}
