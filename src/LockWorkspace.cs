using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using KeePass.Forms;
using KeePass.UI;
using KeePassLib;
using KeePassLib.Serialization;
using PluginTools;

namespace LockAssist
{
  internal class LockWorkspace
  {
    private const string c_LockAssistContinueUnlockWorkbench = "cbLockAssistContinueUnlockWorkbench";
    private static bool m_bContinueUnlock = false;
    private Dictionary<Component, EventHandlerList> m_EventHandlerList = new Dictionary<Component, EventHandlerList>();
    private Dictionary<Component, EventHandlers> m_EventHandlers = new Dictionary<Component, EventHandlers>();
    private static MethodInfo miIsCommandTypeInvokable = null;
    //private static MethodInfo miRestoreWindowState = null;
    //private MethodInfo miUpdateUIState = null;
    private static MethodInfo miCloseDocument = null;
    //private MethodInfo miGetDocuments = null;
    private static MethodInfo miOnFileLock = null;
    private ToolStripButton m_tsbLockWorkspace = null;
    private ToolStripMenuItem m_tsmiLockWorkspace = null;

    private bool m_NewLockWorkspacePossible = false;

    private static MainForm _mf = KeePass.Program.MainForm;
    private bool m_Terminated { get { return _mf == null; } }

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
      /*
      miRestoreWindowState = GetMethod(_mf, "RestoreWindowState", new Type[] { typeof(PwDatabase) });
      if (miRestoreWindowState == null)
      {
        error = true;
        Tools.ShowError("Could not find method RestoreWindowState");
      }
      */
      /*
      miUpdateUIState = GetMethod(_mf, "UpdateUIState", new Type[] { typeof(bool) });
      if (miUpdateUIState == null)
      {
        error = true;
        Tools.ShowError("Could not find method UpdateUIState");
      }
      */
      miCloseDocument = GetMethod(_mf, "CloseDocument", new Type[] { typeof(PwDocument), typeof(bool), typeof(bool), typeof(bool), typeof(bool) });
      if (miCloseDocument == null)
      {
        error = true;
        Tools.ShowError("Could not find method CloseDocument");
      }
      /*
      miGetDocuments = GetMethod(_mf.DocumentManager, "GetDocuments", new Type[] { typeof(int) });
      if (miGetDocuments == null)
      {
        error = true;
        Tools.ShowError("Could not find method GetDocuments");
      }
      */
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

    private static CheckBox m_cbContinueUnlock = null;
    internal static void OnKeyFormShown(object sender, EventArgs e)
    {
      if (!LockAssistConfig.LW_Active) return;
      if (!m_bContinueUnlock) return;

      KeyPromptForm fKeyPromptForm = sender as KeyPromptForm;
      if (fKeyPromptForm == null) return;

      //Create checkbox to continue/stop global unlock
      m_cbContinueUnlock = new CheckBox();
      m_cbContinueUnlock.AutoSize = true;
      m_cbContinueUnlock.Text = KeePass.Resources.KPRes.LockMenuUnlock;
      m_cbContinueUnlock.Checked = true;
      m_cbContinueUnlock.Name = c_LockAssistContinueUnlockWorkbench;
      m_cbContinueUnlock.CheckedChanged += (o1, e1) => { SetContinueUnlock(m_cbContinueUnlock.Checked); };
      m_cbContinueUnlock.Checked = m_cbContinueUnlock.Checked;

      //calculate checkbox position
      CheckBox m_cbKeyFile = (CheckBox)Tools.GetControl("m_cbKeyFile", fKeyPromptForm);
      CheckBox cbAccount = (CheckBox)Tools.GetControl("m_cbUserAccount", fKeyPromptForm);
      m_cbContinueUnlock.Left = cbAccount.Left;
      int iIncrement = cbAccount.Top - m_cbKeyFile.Top;
      m_cbContinueUnlock.Top = cbAccount.Top + iIncrement;
      m_cbContinueUnlock.TabIndex = cbAccount.TabIndex + 1;

      //Adjust position of all relevent controls
      cbAccount.Parent.Height += iIncrement;
      foreach (Control c in cbAccount.Parent.Controls)
      {
        if (c.Top <= cbAccount.Top) continue;
        c.Top += iIncrement;
        c.TabIndex++;
      }
      cbAccount.Parent.Controls.Add(m_cbContinueUnlock);
      KeeThemeStub.Visit(m_cbContinueUnlock);
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
        m_bGlobalUnlockRunning = false;
        return;
      }
      bool bSingle = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
      PwDocument doc = _mf.DocumentManager.ActiveDocument;
      bool bActiveLocked = _mf.IsFileLocked(doc);
      //Active document is unlocked and [Shift] is not pressed ==> Use KeePass standard
      if (!bSingle && !bActiveLocked)
      {
        miOnFileLock.Invoke(_mf, new object[] { sender, e });
        m_bGlobalUnlockRunning = false;
        return;
      }

      //Active document is locked and [Shift] is pressed ==> Use KeePass standard
      if (bSingle && bActiveLocked)
      {
        miOnFileLock.Invoke(_mf, new object[] { sender, e });
        m_bGlobalUnlockRunning = false;
        return;
      }

      //Active document is unlocked and [Shift] is pressed ==> Lock single document
      if (bSingle && !bActiveLocked)
      {
        if (!(bool)miIsCommandTypeInvokable.Invoke(_mf, new object[] { null, 1 })) { return; }
        PwDatabase pd = doc.Database;
        if (!pd.IsOpen)
        {
          m_bGlobalUnlockRunning = false;
          return; // Nothing to lock
        }
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
        m_bGlobalUnlockRunning = false;
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
            _mf.MakeDocumentActive(d);
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
    private static List<string> _Methods = new List<string>() { "StopGlobalUnlock", "OnFileLock", "OnTabMainSelectedIndexChanged", "OnSelectedIndexChanged" };
    internal static bool ShallStopGlobalUnlock()
    {
      if (m_bGlobalUnlockRunning) return false;
      if (m_bContinueUnlock) return false;
      if (_mf.DocumentManager.GetOpenDatabases().Count == _mf.DocumentManager.Documents.Count) return false;

      //Get relevant callstack data
      System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1);
      var lMethods = st.GetFrames().Select(x => x.GetMethod().Name).Where(x => _Methods.Contains(x)).ToList();

      //Stop Global Unlock if
      //	'OnFileLock' has been found
      //	AND
      //	no other relevant methods was found
      //
      // E. g. OnFileLock is called by OnTabMainSelectedIndexChanged and no Global Unlock shall be done in that case
      bool bStop = lMethods.Contains("OnFileLock") && lMethods.Count == 1;
      lMethods.Insert(0, bStop.ToString());
      PluginDebug.AddInfo("StopGlobalUnlock", 0, lMethods.ToArray());
      return bStop;
    }

    internal static void SetContinueUnlock(bool bContinue)
    {
      if (m_bContinueUnlock == bContinue) return;
      m_bContinueUnlock = bContinue;
      if (m_cbContinueUnlock != null) m_cbContinueUnlock.Checked = bContinue;
    }

    internal static bool GetContinueUnlock()
    {
      return LockAssistConfig.LW_Active && m_bContinueUnlock;
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
