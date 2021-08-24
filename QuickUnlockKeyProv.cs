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
	public class QuickUnlockOldKeyInfo
	{
		public ProtectedString QuickUnlockKey = ProtectedString.EmptyEx;
		public ProtectedBinary pwHash = null;
		public string keyFile = string.Empty;
		public bool account = false;
		public ProtectedBinary PINCheck = null;
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
			ProtectedBinary encryptedKey = new ProtectedBinary();
			if (!m_hashedKey.TryGetValue(ctx.DatabasePath, out encryptedKey))
			{
				Tools.ShowError(PluginTranslate.KeyProvNoQuickUnlock);
				return null;
			}

			var uForm = new UnlockForm();
			if (uForm.ShowDialog() != DialogResult.OK) return null;
			ProtectedString QuickUnlockKey = uForm.GetQuickUnlockKey();
			uForm.Close();

			m_hashedKey.Remove(ctx.DatabasePath);
			if (KeePass.UI.GlobalWindowManager.TopWindow is KeePass.Forms.KeyPromptForm)
			{
				QuickUnlockOldKeyInfo ok = null;
				if (m_originalKey.TryGetValue(ctx.DatabasePath, out ok))
				{
					byte[] comparePIN = DecryptKey(QuickUnlockKey, ok.PINCheck).ReadData();
					if (StrUtil.Utf8.GetString(comparePIN) != KeyProviderName)
					{
						LockAssistExt.OnKeyFormShown(KeePass.UI.GlobalWindowManager.TopWindow, true);
						Tools.ShowError(PluginTranslate.WrongPIN);
						return null;
					}
				}
			}
			return DecryptKey(QuickUnlockKey, encryptedKey).ReadData();
		}

		public QuickUnlockOldKeyInfo GetOldKey(PwDatabase db)
		{
			QuickUnlockOldKeyInfo ok = new QuickUnlockOldKeyInfo();
			if (m_originalKey.TryGetValue(db.IOConnectionInfo.Path, out ok))
			{
				if ((ok.pwHash != null) && (ok.pwHash.Length != 0))
					ok.pwHash = DecryptKey(ok.QuickUnlockKey, ok.pwHash);
				m_originalKey.Remove(db.IOConnectionInfo.Path);
				return ok;
			}
			return null;
		}

		public void AddDb(PwDatabase db, ProtectedString QuickUnlockKey, bool savePw)
		{
			RemoveDb(db);
			ProtectedBinary pbKey = CreateMasterKeyHash(db.MasterKey);
			m_hashedKey.Add(db.IOConnectionInfo.Path, EncryptKey(QuickUnlockKey, pbKey));
			AddOldMasterKey(db, QuickUnlockKey, savePw);
		}

		private void AddOldMasterKey(PwDatabase db, ProtectedString QuickUnlockKey, bool savePw)
		{
			QuickUnlockOldKeyInfo ok = new QuickUnlockOldKeyInfo();
			ok.QuickUnlockKey = QuickUnlockKey;
			if (db.MasterKey.ContainsType(typeof(KcpPassword)))
				ok.pwHash = EncryptKey(QuickUnlockKey, SerializePassword(db.MasterKey.GetUserKey(typeof(KcpPassword)) as KcpPassword, savePw));
			if (db.MasterKey.ContainsType(typeof(KcpKeyFile)))
				ok.keyFile = (db.MasterKey.GetUserKey(typeof(KcpKeyFile)) as KcpKeyFile).Path;
			ok.account = db.MasterKey.ContainsType(typeof(KcpUserAccount));
			ok.PINCheck = EncryptKey(QuickUnlockKey, new ProtectedBinary(true, m_PINCheck));
			m_originalKey.Add(db.IOConnectionInfo.Path, ok);
		}

		public static bool HasDB(string db)
		{
			if (m_hashedKey.ContainsKey(db)) return true;
			m_originalKey.Remove(db);
			return false;
		}

		private void RemoveDb(PwDatabase db)
		{
			m_hashedKey.Remove(db.IOConnectionInfo.Path);
			m_originalKey.Remove(db.IOConnectionInfo.Path);
		}

		private ProtectedBinary CreateMasterKeyHash(CompositeKey mk)
		{
			List<byte[]> keys = new List<byte[]>();
			int keysLength = 0;
			foreach (var key in mk.UserKeys) //Hopefully we never need to consider the sequence...
			{
				ProtectedBinary pb = key.KeyData;
				if (pb != null)
				{
					var pbArray = pb.ReadData();
					keys.Add(pbArray);
					keysLength += pbArray.Length;
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

		private ProtectedBinary SerializePassword(KcpPassword p, bool savePassword)
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

		public KcpPassword DeserializePassword(ProtectedBinary serialized, bool setPassword)
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
				return new KcpPassword(pws.ReadString());
			}

			if (serialized.Length != p.KeyData.Length)
				return null;
			ProtectedBinary pb = new ProtectedBinary(true, serialized.ReadData());
			MemUtil.ZeroByteArray(serialized.ReadData());
			typeof(KcpPassword).GetField("m_pbKeyData", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(p, pb);
			typeof(KcpPassword).GetField("m_psPassword", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(p, null);
			return p;
		}

		private ProtectedBinary EncryptKey(ProtectedString QuickUnlockKey, ProtectedBinary pbKey)
		{
			byte[] iv = CryptoRandom.Instance.GetRandomBytes(12);
			ChaCha20Cipher cipher = new ChaCha20Cipher(AdjustQuickUnlockKey(QuickUnlockKey), iv);

			byte[] bKey = pbKey.ReadData();
			cipher.Encrypt(bKey, 0, bKey.Length);

			byte[] result = new byte[iv.Length + bKey.Length];
			iv.CopyTo(result, 0);
			bKey.CopyTo(result, iv.Length);

			return new ProtectedBinary(true, result);
		}

		private ProtectedBinary DecryptKey(ProtectedString QuickUnlockKey, ProtectedBinary pbCrypted)
		{
			byte[] crypted = pbCrypted.ReadData();
			byte[] iv = new byte[12];
			Array.Copy(crypted, iv, iv.Length);

			byte[] cryptedKey = new byte[crypted.Length - iv.Length];
			Array.Copy(crypted, iv.Length, cryptedKey, 0, cryptedKey.Length);

			ChaCha20Cipher cipher = new ChaCha20Cipher(AdjustQuickUnlockKey(QuickUnlockKey), iv);
			byte[] bDecrypted = pbCrypted.ReadData();
			cipher.Decrypt(cryptedKey, 0, cryptedKey.Length);
			ProtectedBinary pbDecrypted = new ProtectedBinary(true, cryptedKey);
			MemUtil.ZeroByteArray(cryptedKey);
			return pbDecrypted;
		}

		private byte[] AdjustQuickUnlockKey(ProtectedString QuickUnlockKey)
		{
			byte[] result = QuickUnlockKey.ReadUtf8();
			SHA256Managed sha = new SHA256Managed();
			return sha.ComputeHash(result);
		}
	}
}
