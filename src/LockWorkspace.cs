using KeePass.Forms;
using KeePass.UI;
using KeePassLib;
using KeePassLib.Serialization;
using PluginTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace LockAssist
{
	internal class LockWorkspace
	{
		internal const string c_LockAssistContinueUnlockWorkbench = "cbLockAssistContinueUnlockWorkbench";
		internal static bool m_bContinueUnlock = false;
		private Dictionary<Component, EventHandlerList> m_EventHandlerList = new Dictionary<Component, EventHandlerList>();
		private Dictionary<Component, EventHandlers> m_EventHandlers = new Dictionary<Component, EventHandlers>();
		private static MethodInfo miIsCommandTypeInvokable = null;
		private static MethodInfo miRestoreWindowState = null;
		private MethodInfo miUpdateUIState = null;
		private static MethodInfo miCloseDocument = null;
		private MethodInfo miGetDocuments = null;
		private static MethodInfo miOnFileLock = null;
		private ToolStripButton m_tsbLockWorkspace = null;
		private ToolStripMenuItem m_tsmiLockWorkspace = null;

		private bool m_NewLockWorkspacePossible = false;

		private static MainForm _mf = KeePass.Program.MainForm;
		private bool m_Terminated { get{ return _mf == null; } }

		public LockWorkspace()
        {
			Init();
        }
		private void Init()
		{
			bool error = false;
			m_NewLockWorkspacePossible = false;
			m_tsbLockWorkspace = (ToolStripButton)Tools.GetField("m_tbLockWorkspace", _mf);
			if (m_tsbLockWorkspace == null)
			{
				error = true;
				Tools.ShowError("Could not find toolbar button m_tbLockWorkspace");
			}
			m_tsmiLockWorkspace = (ToolStripMenuItem)_mf.MainMenu.Items.Find("m_menuFileLock", true)[0];
			if (m_tsmiLockWorkspace == null)
			{
				error = true;
				Tools.ShowError("Could not find menu item m_menuFileLock");
			}
			miIsCommandTypeInvokable = GetMethod(_mf, "IsCommandTypeInvokable");
			if (miIsCommandTypeInvokable == null)
			{
				error = true;
				Tools.ShowError("Could not find method IsCommandTypeInvokable");
			}
			miRestoreWindowState = GetMethod(_mf, "RestoreWindowState", new Type[] { typeof(PwDatabase) });
			if (miRestoreWindowState == null)
			{
				error = true;
				Tools.ShowError("Could not find method RestoreWindowState");
			}
			miUpdateUIState = GetMethod(_mf, "UpdateUIState", new Type[] { typeof(bool) });
			if (miUpdateUIState == null)
			{
				error = true;
				Tools.ShowError("Could not find method UpdateUIState");
			}
			miCloseDocument = GetMethod(_mf, "CloseDocument", new Type[] { typeof(PwDocument), typeof(bool), typeof(bool), typeof(bool), typeof(bool) });
			if (miCloseDocument == null)
			{
				error = true;
				Tools.ShowError("Could not find method CloseDocument");
			}
			miGetDocuments = GetMethod(_mf.DocumentManager, "GetDocuments", new Type[] { typeof(int) });
			if (miGetDocuments == null)
			{
				error = true;
				Tools.ShowError("Could not find method GetDocuments");
			}
			miOnFileLock = GetMethod(_mf, "OnFileLock");
			if (miOnFileLock == null)
			{
				error = true;
				Tools.ShowError("Could not find method OnFileLock");
			}
			if (error) return;
			m_EventHandlerList.Clear();
			m_EventHandlers.Clear();
			m_EventHandlerList.Add(m_tsbLockWorkspace, Events.GetEventHandlerList(m_tsbLockWorkspace, "Click"));
			m_EventHandlers.Add(m_tsbLockWorkspace, Events.GetEventHandlers(m_tsbLockWorkspace, "Click", m_EventHandlerList[m_tsbLockWorkspace]));
			m_EventHandlerList.Add(m_tsmiLockWorkspace, Events.GetEventHandlerList(m_tsmiLockWorkspace, "Click"));
			m_EventHandlers.Add(m_tsmiLockWorkspace, Events.GetEventHandlers(m_tsmiLockWorkspace, "Click", m_EventHandlerList[m_tsbLockWorkspace]));
			m_NewLockWorkspacePossible = true;
			if (LockAssistConfig.LW_Active) ActivateNewLockWorkspace(LockAssistConfig.LW_Active);
		}

        internal static void OnKeyFormShown(object sender, EventArgs e)
        {
			if (!LockAssistConfig.LW_Active) return;
			if (!m_bContinueUnlock) return;

			KeyPromptForm fKeyPromptForm = sender as KeyPromptForm;
			if (fKeyPromptForm == null) return;
			
			//Create checkbox to continue/stop global unlock
			CheckBox cbContinueUnlock = new CheckBox();
			cbContinueUnlock.AutoSize = true;
			cbContinueUnlock.Text = KeePass.Resources.KPRes.LockMenuUnlock;
			cbContinueUnlock.Checked = true;
			cbContinueUnlock.Name = c_LockAssistContinueUnlockWorkbench;
			cbContinueUnlock.CheckedChanged += (o1, e1) => { m_bContinueUnlock = cbContinueUnlock.Checked; };
			cbContinueUnlock.Checked = cbContinueUnlock.Checked;

			//calculate checkbox position
			CheckBox m_cbKeyFile = (CheckBox)Tools.GetControl("m_cbKeyFile", fKeyPromptForm);
			CheckBox cbAccount = (CheckBox)Tools.GetControl("m_cbUserAccount", fKeyPromptForm);
			cbContinueUnlock.Left = cbAccount.Left;
			int iIncrement = cbAccount.Top - m_cbKeyFile.Top;
			cbContinueUnlock.Top = cbAccount.Top + iIncrement;
			cbContinueUnlock.TabIndex = cbAccount.TabIndex + 1;
			
			//Adjust position of all relevent controls
			cbAccount.Parent.Height += iIncrement;
			foreach (Control c in cbAccount.Parent.Controls)
            {
				if (c.Top <= cbAccount.Top) continue;
				c.Top += iIncrement;
				c.TabIndex++;
            }
			cbAccount.Parent.Controls.Add(cbContinueUnlock);
		}

        internal void Clear()
		{
			ActivateNewLockWorkspace(false);
		}

		internal void ActivateNewLockWorkspace(bool newLogic)
		{
			if (!m_NewLockWorkspacePossible) return;
			if (newLogic)
			{
				Events.RemoveEventHandlers(m_tsbLockWorkspace, "Click", m_EventHandlerList[m_tsbLockWorkspace]);
				m_tsbLockWorkspace.Click += OnEnhancedWorkspaceLockUnlock;
				Events.RemoveEventHandlers(m_tsmiLockWorkspace, "Click", m_EventHandlerList[m_tsmiLockWorkspace]);
				m_tsmiLockWorkspace.Click += OnEnhancedWorkspaceLockUnlock;
			}
			else
			{
				m_tsbLockWorkspace.Click -= OnEnhancedWorkspaceLockUnlock;
				Events.AddEventHandlers(m_tsbLockWorkspace, "Click", m_EventHandlers[m_tsmiLockWorkspace], m_EventHandlerList[m_tsbLockWorkspace]);
				m_tsmiLockWorkspace.Click -= OnEnhancedWorkspaceLockUnlock;
				Events.AddEventHandlers(m_tsmiLockWorkspace, "Click", m_EventHandlers[m_tsmiLockWorkspace], m_EventHandlerList[m_tsmiLockWorkspace]);
			}
		}

		private MethodInfo GetMethod(object obj, string methodName)
		{
			return obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
		}
		
		private MethodInfo GetMethod(object obj, string methodName, Type[] types)
		{
			return obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic, null, types, null);
		}

		internal void OnEnhancedWorkspaceLockUnlock(object sender, EventArgs e)
		{
			m_bGlobalUnlockRunning = true;
			/* Use cases:
			 * 1) Only one document is loaded ==> Use KeePass standard
			 * 2) Active document is unlocked and [Shift] is not pressed ==> Use KeePass standard
			 * 3) Active document is locked and [Shift] is pressed ==> Use KeePass standard
			 * 4) Active document is unlocked and [Shift] is pressed ==> Lock single document
			 * 5) Active document is locked and [Shift] is not pressed ==> Unlock all documents
			*/

			//Call standard if only one document is loaded
			if (_mf.DocumentManager.DocumentCount < 2)
			{
				miOnFileLock.Invoke(_mf, new object[] { sender, e });
				return;
			}
			bool bSingle = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
            PwDocument doc = _mf.DocumentManager.ActiveDocument;
			bool bActiveLocked = _mf.IsFileLocked(doc);
			//Active document is unlocked and [Shift] is not pressed ==> Use KeePass standard
			if (!bSingle && !bActiveLocked)
			{
				miOnFileLock.Invoke(_mf, new object[] { sender, e });
				return;
			}

			//Active document is locked and [Shift] is pressed ==> Use KeePass standard
			if (bSingle && bActiveLocked)
			{
				miOnFileLock.Invoke(_mf, new object[] { sender, e });
				return;
			}

			//Active document is unlocked and [Shift] is pressed ==> Lock single document
			if (bSingle && !bActiveLocked)
			{
				if (!(bool)miIsCommandTypeInvokable.Invoke(_mf, new object[] { null, 1 })) { return; }
				PwDatabase pd = doc.Database;
				if (!pd.IsOpen) return; // Nothing to lock
				IOConnectionInfo ioIoc = pd.IOConnectionInfo;
				miCloseDocument.Invoke(_mf, new object[] { doc, true, false, false, false });
				if (!pd.IsOpen)
				{
					doc.LockedIoc = ioIoc;
					_mf.UpdateUI(true, null, true, null, true, null, false);
					if (KeePass.Program.Config.MainWindow.MinimizeAfterLocking &&
						!_mf.IsAtLeastOneFileOpen())
						UIUtil.SetWindowState(_mf, FormWindowState.Minimized);
				}
				return;
			}

			//Active document is locked and [Shift] is not pressed ==> Unlock all documents
			if (!bSingle && bActiveLocked)
			{
				List<PwDocument> lDocs = new List<PwDocument>();
				PwDocument active = _mf.DocumentManager.ActiveDocument;
				int idx = _mf.DocumentManager.Documents.IndexOf(active);
				for (int i = idx; i < _mf.DocumentManager.DocumentCount; i++)
					if (_mf.IsFileLocked(_mf.DocumentManager.Documents[i])) lDocs.Add(_mf.DocumentManager.Documents[i]);
				for (int i = 0; i < idx; i++)
					if (_mf.IsFileLocked(_mf.DocumentManager.Documents[i])) lDocs.Add(_mf.DocumentManager.Documents[i]);
				for (int i = 0; i < lDocs.Count; i++)
				{
					m_bContinueUnlock = (i < (lDocs.Count - 1));
					PwDocument d = lDocs[i];
					_mf.MakeDocumentActive(d);
					PwDatabase pd = d.Database;
					if (!_mf.IsFileLocked(d)) continue;
					_mf.OpenDatabase(d.LockedIoc, null, false);

					if (pd.IsOpen)
					{
						d.LockedIoc = new IOConnectionInfo(); // Clear lock
						miRestoreWindowState.Invoke(_mf, new object[] { pd });
					}
					if (m_Terminated || !m_bContinueUnlock) break;
				}
				m_bContinueUnlock = false;
				m_bGlobalUnlockRunning = false;
				if (!m_Terminated) _mf.MakeDocumentActive(active);
				return;
			}
		}

		private static bool m_bGlobalUnlockRunning = false;
		internal static void CheckGlobalUnlock(out bool bStopGlobalUnlock)
		{
			bStopGlobalUnlock = false;
			if (m_bGlobalUnlockRunning) return;
			if (m_bContinueUnlock) return;
			if (_mf.DocumentManager.GetOpenDatabases().Count == _mf.DocumentManager.Documents.Count) return;
			System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1);
			try
			{
				foreach (System.Diagnostics.StackFrame sf in st.GetFrames())
				{
					string sMethodName = sf.GetMethod().Name;
					if (sMethodName == "CheckGlobalUnlock")
					{
						bStopGlobalUnlock = false;
						return;
					}
					if (sMethodName == "OnFileLock")
					{
						bStopGlobalUnlock = true;
					}
					if ((sMethodName == "OnTabMainSelectedIndexChanged") || (sMethodName == "OnSelectedIndexChanged"))
					{
						bStopGlobalUnlock = false;
						break;
					}
				}
			}
			catch { }
			return;
		}
	}


	public class EventHandlers : Dictionary<object, List<Delegate>> { };

	public static class Events
	{
		public static EventHandlerList GetEventHandlerList(object obj, string eventName)
		{
			object eventObject = Tools.GetField("Event" + eventName, obj);
			PropertyInfo pi = obj.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
			return (EventHandlerList)pi.GetValue(obj, null);
		}

		public static EventHandlers GetEventHandlers(Component obj, string eventName, EventHandlerList list)
		{
			EventHandlers result = new EventHandlers();
			try
			{
				var head = Tools.GetField("head", list);
				if (head != null)
				{
					Type listEntryType = head.GetType();
					FieldInfo delegateFI = listEntryType.GetField("handler", BindingFlags.Instance | BindingFlags.NonPublic);
					FieldInfo keyFI = listEntryType.GetField("key", BindingFlags.Instance | BindingFlags.NonPublic);
					FieldInfo nextFI = listEntryType.GetField("next", BindingFlags.Instance | BindingFlags.NonPublic);
					BuildEventList(result, head, delegateFI, keyFI, nextFI);
				}
			}
			catch { }
			return result;
		}

		public static void RemoveEventHandlers(Component obj, string eventName, EventHandlerList list)
		{
			EventHandlers handlers = GetEventHandlers(obj, eventName, list);
			foreach (object key in handlers.Keys)
			{
				List<Delegate> d = new List<Delegate>();
				if (!handlers.TryGetValue(key, out d)) continue;
				for (int i = d.Count - 1; i >= 0; i--)
					list.RemoveHandler(key, d[i]);
			}
		}

		public static void AddEventHandlers(Component obj, string eventName, EventHandlers handlers, EventHandlerList list)
		{
			foreach (object key in handlers.Keys)
			{
				List<Delegate> d = new List<Delegate>();
				if (!handlers.TryGetValue(key, out d)) continue;
				for (int i = 0; i < d.Count; i++)
					list.AddHandler(key, d[i]);
			}
		}

		private static void BuildEventList(EventHandlers result, object entry, FieldInfo delegateFI, FieldInfo keyFI, FieldInfo nextFI)
		{
			if (entry != null)
			{
				Delegate dele = (Delegate)delegateFI.GetValue(entry);
				object key = keyFI.GetValue(entry);
				object next = nextFI.GetValue(entry);

				Delegate[] listeners = dele.GetInvocationList();
				if (listeners != null && listeners.Length > 0)
				{
					List<Delegate> list = new List<Delegate>();
					foreach (Delegate d in listeners)
						list.Add(d);
					result.Add(key, list);
				}

				if (next != null)
				{
					BuildEventList(result, next, delegateFI, keyFI, nextFI);
				}
			}
		}
	}
}
