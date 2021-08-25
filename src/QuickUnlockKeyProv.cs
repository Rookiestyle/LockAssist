using System;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Security.Cryptography;

using KeePassLib.Keys;
using KeePassLib;
using KeePassLib.Cryptography;
using KeePassLib.Security;
using KeePassLib.Utility;
using KeePassLib.Cryptography.Cipher;

using PluginTranslation;
using PluginTools;

namespace LockAssist
{
	/* 
	* QuickUnlock part is inspired by KeePass2Android's QuickUnlock and https://github.com/JanisEst/KeePassQuickUnlock
	* Additional features added:
	*   - DB specific settings
	*   - Restore previously used masterkey (allow QuickUnlock multiple times in a row, allow printing of emergency sheets, ...)
	*   - Show explicit error message if wrong QuickUnlock key is entered
	*   - Automate creation of QuickUnlock entry
	*   - Additional smaller adjustments
	*/
	public class QuickUnlockOldKeyInfo
	{
		public ProtectedString QuickUnlockKey = ProtectedString.EmptyEx;
		public ProtectedBinary pwHash = null;
		public string keyFile = string.Empty;
		public bool account = false;
		public ProtectedBinary PINCheck = null;
		public bool HasPassword = false;
	}

	public class QuickUnlockKeyProv : KeyProvider
	{
		public static string KeyProviderName = PluginTranslate.PluginName + " - Quick Unlock";
		private static byte[] m_PINCheck = StrUtil.Utf8.GetBytes(KeyProviderName);
		public override string Name { get { return KeyProviderName; } }
		public override bool SecureDesktopCompatible { get { return true; } }
		public override bool Exclusive { get { return true; } }
		public override bool DirectKey { get { return true; } }
		public override bool GetKeyMightShowGui { get { return true; } }

		private static Dictionary<string, ProtectedBinary> m_hashedKey = new Dictionary<string, ProtectedBinary>();
		private static Dictionary<string, QuickUnlockOldKeyInfo> m_originalKey = new Dictionary<string, QuickUnlockOldKeyInfo>();

		public override byte[] GetKey(KeyProviderQueryContext ctx)
		{
			if (ctx.CreatingNewKey) //should not happen but you never know
			{
				Tools.ShowError(PluginTranslate.KeyProvNoCreate);
				return null;
			}

			//Check for existing Quick Unlock data
			ProtectedBinary encryptedKey = new ProtectedBinary();
			if (!m_hashedKey.TryGetValue(ctx.DatabasePath, out encryptedKey))
			{
				Tools.ShowError(PluginTranslate.KeyProvNoQuickUnlock);
				return null;
			}

			var fQuickUnlock = new UnlockForm();
			if (KeePass.UI.UIUtil.ShowDialogNotValue(fQuickUnlock, DialogResult.OK)) return null;
			ProtectedString psQuickUnlockKey = fQuickUnlock.QuickUnlockKey;
			KeePass.UI.UIUtil.DestroyForm(fQuickUnlock);

			//Remove Quick Unlock data - there is only one attempt
			m_hashedKey.Remove(ctx.DatabasePath);
			if (KeePass.UI.GlobalWindowManager.TopWindow is KeePass.Forms.KeyPromptForm && !VerifyPin(ctx, psQuickUnlockKey))
			{
				QuickUnlock.OnKeyFormShown_QU(KeePass.UI.GlobalWindowManager.TopWindow, true);
				Tools.ShowError(PluginTranslate.WrongPIN);
				return null;
			}
			return DecryptKey(psQuickUnlockKey, encryptedKey).ReadData();
		}

        private bool VerifyPin(KeyProviderQueryContext ctx, ProtectedString psQuickUnlockKey)
        {
			//Verify Quick Unlock PIN
			QuickUnlockOldKeyInfo quOldKey = null;
			if (!m_originalKey.TryGetValue(ctx.DatabasePath, out quOldKey)) return false;
			byte[] comparePIN = DecryptKey(psQuickUnlockKey, quOldKey.PINCheck).ReadData();
			return StrUtil.Utf8.GetString(comparePIN) == KeyProviderName;
		}

		public static QuickUnlockOldKeyInfo GetOldKey(PwDatabase db)
		{
			QuickUnlockOldKeyInfo quOldKey = null;
			if (!m_originalKey.TryGetValue(db.IOConnectionInfo.Path, out quOldKey)) return null;
			if ((quOldKey.pwHash != null) && (quOldKey.pwHash.Length != 0))
				quOldKey.pwHash = DecryptKey(quOldKey.QuickUnlockKey, quOldKey.pwHash);
			m_originalKey.Remove(db.IOConnectionInfo.Path);
				return quOldKey;
		}

		public static void AddDb(PwDatabase db, ProtectedString QuickUnlockKey, bool savePw)
		{
			RemoveDb(db);
			ProtectedBinary pbKey = CreateMasterKeyHash(db.MasterKey);
			m_hashedKey.Add(db.IOConnectionInfo.Path, EncryptKey(QuickUnlockKey, pbKey));
			AddOldMasterKey(db, QuickUnlockKey, savePw);
		}

		private static void AddOldMasterKey(PwDatabase db, ProtectedString QuickUnlockKey, bool savePw)
		{
			QuickUnlockOldKeyInfo quOldKey = new QuickUnlockOldKeyInfo();
			quOldKey.QuickUnlockKey = QuickUnlockKey;
			quOldKey.HasPassword = db.MasterKey.ContainsType(typeof(KcpPassword));
			if (quOldKey.HasPassword)
			{
				var pbPasswordSerialized = SerializePassword(db.MasterKey.GetUserKey(typeof(KcpPassword)) as KcpPassword, savePw);
				quOldKey.pwHash = EncryptKey(QuickUnlockKey, pbPasswordSerialized);
			}
			if (db.MasterKey.ContainsType(typeof(KcpKeyFile)))
				quOldKey.keyFile = (db.MasterKey.GetUserKey(typeof(KcpKeyFile)) as KcpKeyFile).Path;
			quOldKey.account = db.MasterKey.ContainsType(typeof(KcpUserAccount));
			quOldKey.PINCheck = EncryptKey(QuickUnlockKey, new ProtectedBinary(true, m_PINCheck));
			m_originalKey.Add(db.IOConnectionInfo.Path, quOldKey);
		}

		public static void Clear()
		{
			m_hashedKey.Clear();
			m_originalKey.Clear();
		}

		public static bool HasDB(string db)
		{
			if (m_hashedKey.ContainsKey(db)) return true;
			m_originalKey.Remove(db);
			return false;
		}

		public static void RemoveDb(PwDatabase db)
		{
			if (db == null) return;
			if (!string.IsNullOrEmpty(db.IOConnectionInfo.Path))
			{
				RemoveDb(db.IOConnectionInfo.Path);
				return;
			}
			KeePass.UI.PwDocument doc = KeePass.Program.MainForm.DocumentManager.FindDocument(db);
			if ((doc == null) || string.IsNullOrEmpty(doc.LockedIoc.Path)) return;
			RemoveDb(doc.LockedIoc.Path);
		}

		public static void RemoveDb(string ioc)
		{
			bool bRemoved = m_hashedKey.Remove(ioc);
			bRemoved |= m_originalKey.Remove(ioc);
			if (bRemoved) PluginDebug.AddInfo("Quick Unlock - Removed Quick Unlock data", 10, "Database: " + ioc);
		}

		private static ProtectedBinary CreateMasterKeyHash(CompositeKey mk)
		{
			List<byte[]> keys = new List<byte[]>();
			int keysLength = 0;
			foreach (var key in mk.UserKeys) //Hopefully we never need to consider the sequence...
			{
				ProtectedBinary pb = key.KeyData;
				if (pb != null)
				{
					var pbArray = pb.ReadData();
					keys.Add((byte[])pbArray.Clone());
					keysLength += pbArray.Length;
					MemUtil.ZeroByteArray(pbArray);
				}
			}

			byte[] allKeys = new byte[keysLength];
			int index = 0;
			foreach (byte[] key in keys)
			{
				Array.Copy(key, 0, allKeys, index, key.Length);
				index += key.Length;
				MemUtil.ZeroByteArray(key);
			}

			var result = new ProtectedBinary(true, allKeys);
			MemUtil.ZeroByteArray(allKeys);
			return result;
		}

		private static ProtectedBinary SerializePassword(KcpPassword p, bool savePassword)
		{
			//returned array always contains password hash
			//password is contained only if requested
			//check for p.Password != null as the user might disable Program.Config.Security.MasterPassword.RememberWhileOpen anytime
			if (savePassword && (p.Password != null) && !p.Password.IsEmpty)
			{
				byte[] result = new byte[p.KeyData.Length + p.Password.ReadUtf8().Length];
				Array.Copy(p.KeyData.ReadData(), result, p.KeyData.Length);
				Array.Copy(p.Password.ReadUtf8(), 0, result, p.KeyData.Length, p.Password.ReadUtf8().Length);
				return new ProtectedBinary(true, result);
			}
			return p.KeyData;
		}

		public static KcpPassword DeserializePassword(ProtectedBinary serialized, bool setPassword)
		{
			//if password is stored and should be retrieved 
			//simply create a new instance
			//instead of messing around with private members
			KcpPassword p = new KcpPassword(string.Empty);
			if (setPassword && (serialized.Length > p.KeyData.Length))
			{
				byte[] pw = new byte[serialized.Length - p.KeyData.Length];
				Array.Copy(serialized.ReadData(), p.KeyData.Length, pw, 0, pw.Length);
				ProtectedString pws = new ProtectedString(true, pw);
				MemUtil.ZeroByteArray(pw);
				return new KcpPassword(pws.ReadString());
			}

			if (serialized.Length != p.KeyData.Length) return null;

			ProtectedBinary pb = new ProtectedBinary(true, serialized.ReadData());
			typeof(KcpPassword).GetField("m_pbKeyData", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(p, pb);
			typeof(KcpPassword).GetField("m_psPassword", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(p, null);
			return p;
		}

		private static ProtectedBinary EncryptKey(ProtectedString QuickUnlockKey, ProtectedBinary pbKey)
		{
			byte[] iv = CryptoRandom.Instance.GetRandomBytes(12);
			ChaCha20Cipher cipher = new ChaCha20Cipher(AdjustQuickUnlockKey(QuickUnlockKey), iv);

			byte[] bKey = pbKey.ReadData();
			cipher.Encrypt(bKey, 0, bKey.Length);

			byte[] result = new byte[iv.Length + bKey.Length];
			iv.CopyTo(result, 0);
			bKey.CopyTo(result, iv.Length);
			MemUtil.ZeroByteArray(bKey);

			var pbResult = new ProtectedBinary(true, result);
			MemUtil.ZeroByteArray(result);
			return pbResult;
		}

		private static ProtectedBinary DecryptKey(ProtectedString QuickUnlockKey, ProtectedBinary pbCrypted)
		{
			byte[] crypted = pbCrypted.ReadData();
			byte[] iv = new byte[12];
			Array.Copy(crypted, iv, iv.Length);

			byte[] cryptedKey = new byte[crypted.Length - iv.Length];
			Array.Copy(crypted, iv.Length, cryptedKey, 0, cryptedKey.Length);

			ChaCha20Cipher cipher = new ChaCha20Cipher(AdjustQuickUnlockKey(QuickUnlockKey), iv);
			cipher.Decrypt(cryptedKey, 0, cryptedKey.Length);
			ProtectedBinary pbDecrypted = new ProtectedBinary(true, cryptedKey);
			MemUtil.ZeroByteArray(cryptedKey);
			return pbDecrypted;
		}

		private static byte[] AdjustQuickUnlockKey(ProtectedString QuickUnlockKey)
		{
			byte[] bUtf8 = QuickUnlockKey.ReadUtf8();
			SHA256Managed sha = new SHA256Managed();
			byte[] result = sha.ComputeHash(bUtf8);
			MemUtil.ZeroByteArray(bUtf8);
			return result;
		}
	}
}
