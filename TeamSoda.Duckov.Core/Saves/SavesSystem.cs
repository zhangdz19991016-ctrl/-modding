using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Saves
{
	// Token: 0x02000228 RID: 552
	public class SavesSystem
	{
		// Token: 0x170002ED RID: 749
		// (get) Token: 0x060010A5 RID: 4261 RVA: 0x00040ED8 File Offset: 0x0003F0D8
		// (set) Token: 0x060010A6 RID: 4262 RVA: 0x00040F37 File Offset: 0x0003F137
		public static int CurrentSlot
		{
			get
			{
				if (SavesSystem._currentSlot == null)
				{
					SavesSystem._currentSlot = new int?(PlayerPrefs.GetInt("CurrentSlot", 1));
					int? currentSlot = SavesSystem._currentSlot;
					int num = 1;
					if (currentSlot.GetValueOrDefault() < num & currentSlot != null)
					{
						SavesSystem._currentSlot = new int?(1);
					}
				}
				return SavesSystem._currentSlot.Value;
			}
			private set
			{
				SavesSystem._currentSlot = new int?(value);
				PlayerPrefs.SetInt("CurrentSlot", value);
				SavesSystem.CacheFile();
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x060010A7 RID: 4263 RVA: 0x00040F54 File Offset: 0x0003F154
		public static string CurrentFilePath
		{
			get
			{
				return SavesSystem.GetFilePath(SavesSystem.CurrentSlot);
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x060010A8 RID: 4264 RVA: 0x00040F60 File Offset: 0x0003F160
		public static bool IsSaving
		{
			get
			{
				return SavesSystem.saving;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x060010A9 RID: 4265 RVA: 0x00040F67 File Offset: 0x0003F167
		public static string SavesFolder
		{
			get
			{
				return "Saves";
			}
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x00040F6E File Offset: 0x0003F16E
		public static string GetFullPathToSavesFolder()
		{
			return Path.Combine(Application.persistentDataPath, SavesSystem.SavesFolder);
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00040F7F File Offset: 0x0003F17F
		public static string GetFilePath(int slot)
		{
			return Path.Combine(SavesSystem.SavesFolder, SavesSystem.GetSaveFileName(slot));
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x00040F91 File Offset: 0x0003F191
		public static string GetSaveFileName(int slot)
		{
			return string.Format("Save_{0}.sav", slot);
		}

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x060010AD RID: 4269 RVA: 0x00040FA4 File Offset: 0x0003F1A4
		// (remove) Token: 0x060010AE RID: 4270 RVA: 0x00040FD8 File Offset: 0x0003F1D8
		public static event Action OnSetFile;

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x060010AF RID: 4271 RVA: 0x0004100C File Offset: 0x0003F20C
		// (remove) Token: 0x060010B0 RID: 4272 RVA: 0x00041040 File Offset: 0x0003F240
		public static event Action OnSaveDeleted;

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x060010B1 RID: 4273 RVA: 0x00041074 File Offset: 0x0003F274
		// (remove) Token: 0x060010B2 RID: 4274 RVA: 0x000410A8 File Offset: 0x0003F2A8
		public static event Action OnCollectSaveData;

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x060010B3 RID: 4275 RVA: 0x000410DC File Offset: 0x0003F2DC
		// (remove) Token: 0x060010B4 RID: 4276 RVA: 0x00041110 File Offset: 0x0003F310
		public static event Action OnRestoreFailureDetected;

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x00041143 File Offset: 0x0003F343
		// (set) Token: 0x060010B6 RID: 4278 RVA: 0x0004114A File Offset: 0x0003F34A
		public static bool RestoreFailureMarker { get; private set; }

		// Token: 0x060010B7 RID: 4279 RVA: 0x00041152 File Offset: 0x0003F352
		public static bool IsOldSave(int index)
		{
			return !SavesSystem.KeyExisits("CreatedWithVersion", index);
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x00041162 File Offset: 0x0003F362
		public static void SetFile(int index)
		{
			SavesSystem.cached = false;
			SavesSystem.CurrentSlot = index;
			Action onSetFile = SavesSystem.OnSetFile;
			if (onSetFile == null)
			{
				return;
			}
			onSetFile();
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0004117F File Offset: 0x0003F37F
		public static SavesSystem.BackupInfo[] GetBackupList()
		{
			return SavesSystem.GetBackupList(SavesSystem.CurrentSlot);
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x0004118B File Offset: 0x0003F38B
		public static SavesSystem.BackupInfo[] GetBackupList(int slot)
		{
			return SavesSystem.GetBackupList(SavesSystem.GetFilePath(slot), slot);
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x0004119C File Offset: 0x0003F39C
		public static SavesSystem.BackupInfo[] GetBackupList(string mainPath, int slot = -1)
		{
			SavesSystem.BackupInfo[] array = new SavesSystem.BackupInfo[10];
			for (int i = 0; i < 10; i++)
			{
				try
				{
					string backupPathByIndex = SavesSystem.GetBackupPathByIndex(mainPath, i);
					ES3Settings es3Settings = new ES3Settings(backupPathByIndex, null);
					es3Settings.location = ES3.Location.File;
					bool flag = ES3.FileExists(backupPathByIndex, es3Settings);
					long num = 0L;
					if (flag && ES3.KeyExists("SaveTime", backupPathByIndex, es3Settings))
					{
						num = ES3.Load<long>("SaveTime", backupPathByIndex, es3Settings);
					}
					DateTime.FromBinary(num);
					SavesSystem.BackupInfo backupInfo = new SavesSystem.BackupInfo
					{
						slot = slot,
						index = i,
						path = backupPathByIndex,
						exists = flag,
						time_raw = num
					};
					array[i] = backupInfo;
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					array[i] = default(SavesSystem.BackupInfo);
				}
			}
			return array;
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x00041278 File Offset: 0x0003F478
		private static int GetEmptyOrOldestBackupIndex()
		{
			SavesSystem.BackupInfo[] backupList = SavesSystem.GetBackupList();
			int result = -1;
			DateTime t = DateTime.MaxValue;
			foreach (SavesSystem.BackupInfo backupInfo in backupList)
			{
				if (!backupInfo.exists)
				{
					return backupInfo.index;
				}
				if (backupInfo.Time < t)
				{
					result = backupInfo.index;
					t = backupInfo.Time;
				}
			}
			return result;
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x000412DC File Offset: 0x0003F4DC
		private static int GetOldestBackupIndex()
		{
			SavesSystem.BackupInfo[] backupList = SavesSystem.GetBackupList();
			int result = -1;
			DateTime t = DateTime.MaxValue;
			foreach (SavesSystem.BackupInfo backupInfo in backupList)
			{
				if (backupInfo.exists && backupInfo.Time < t)
				{
					result = backupInfo.index;
					t = backupInfo.Time;
				}
			}
			return result;
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x00041338 File Offset: 0x0003F538
		private static int GetNewestBackupIndex()
		{
			SavesSystem.BackupInfo[] backupList = SavesSystem.GetBackupList();
			int result = -1;
			DateTime t = DateTime.MinValue;
			foreach (SavesSystem.BackupInfo backupInfo in backupList)
			{
				if (backupInfo.exists && backupInfo.Time > t)
				{
					result = backupInfo.index;
					t = backupInfo.Time;
				}
			}
			return result;
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x00041393 File Offset: 0x0003F593
		private static string GetBackupPathByIndex(int index)
		{
			return SavesSystem.GetBackupPathByIndex(SavesSystem.CurrentSlot, index);
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x000413A0 File Offset: 0x0003F5A0
		private static string GetBackupPathByIndex(int slot, int index)
		{
			return SavesSystem.GetBackupPathByIndex(SavesSystem.GetFilePath(slot), index);
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x000413AE File Offset: 0x0003F5AE
		private static string GetBackupPathByIndex(string path, int index)
		{
			return string.Format("{0}.bac.{1:00}", path, index + 1);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x000413C4 File Offset: 0x0003F5C4
		private static void CreateIndexedBackup(int index = -1)
		{
			SavesSystem.LastIndexedBackupTime = DateTime.UtcNow;
			try
			{
				if (index < 0)
				{
					index = SavesSystem.GetEmptyOrOldestBackupIndex();
				}
				string backupPathByIndex = SavesSystem.GetBackupPathByIndex(index);
				ES3.DeleteFile(backupPathByIndex, SavesSystem.settings);
				ES3.CopyFile(SavesSystem.CurrentFilePath, backupPathByIndex);
				ES3.StoreCachedFile(backupPathByIndex);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				Debug.Log("[Saves] Failed creating indexed backup");
			}
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x0004142C File Offset: 0x0003F62C
		private static void CreateBackup()
		{
			try
			{
				SavesSystem.CreateBackup(SavesSystem.CurrentFilePath);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				Debug.Log("[Saves] Failed creating backup");
			}
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x00041468 File Offset: 0x0003F668
		private static void CreateBackup(string path)
		{
			try
			{
				string filePath = path + ".bac";
				ES3.DeleteFile(filePath, SavesSystem.settings);
				ES3.CreateBackup(path);
				ES3.StoreCachedFile(filePath);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				Debug.Log("[Saves] Failed creating backup for path " + path);
			}
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x000414C0 File Offset: 0x0003F6C0
		public static void UpgradeSaveFileAssemblyInfo(string path)
		{
			if (!File.Exists(path))
			{
				Debug.Log("没有找到存档文件：" + path);
				return;
			}
			string text;
			using (StreamReader streamReader = File.OpenText(path))
			{
				text = streamReader.ReadToEnd();
				if (text.Contains("TeamSoda.Duckov.Core"))
				{
					streamReader.Close();
					return;
				}
				text = text.Replace("Assembly-CSharp", "TeamSoda.Duckov.Core");
				streamReader.Close();
			}
			File.Delete(path);
			using (FileStream fileStream = File.OpenWrite(path))
			{
				StreamWriter streamWriter = new StreamWriter(fileStream);
				streamWriter.Write(text);
				streamWriter.Close();
				fileStream.Close();
			}
			Debug.Log("存档格式已更新：" + path);
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x0004158C File Offset: 0x0003F78C
		public static void RestoreIndexedBackup(int slot, int index)
		{
			string backupPathByIndex = SavesSystem.GetBackupPathByIndex(slot, index);
			SavesSystem.UpgradeSaveFileAssemblyInfo(Path.Combine(Application.persistentDataPath, backupPathByIndex));
			string filePath = SavesSystem.GetFilePath(slot);
			string text = filePath + ".bac";
			try
			{
				ES3.CacheFile(backupPathByIndex);
				ES3.DeleteFile(text, SavesSystem.settings);
				ES3.CopyFile(backupPathByIndex, text);
				ES3.DeleteFile(filePath, SavesSystem.settings);
				ES3.RestoreBackup(filePath, SavesSystem.settings);
				ES3.StoreCachedFile(filePath);
				ES3.CacheFile(filePath);
				Action onSetFile = SavesSystem.OnSetFile;
				if (onSetFile != null)
				{
					onSetFile();
				}
			}
			catch
			{
				SavesSystem.RestoreFailureMarker = true;
				Debug.LogError("文件损坏，且无法修复。");
				ES3.DeleteFile(filePath);
				File.Delete(filePath);
				ES3.Save<bool>("Created", true, filePath);
				ES3.StoreCachedFile(filePath);
				ES3.CacheFile(filePath);
				Action onRestoreFailureDetected = SavesSystem.OnRestoreFailureDetected;
				if (onRestoreFailureDetected != null)
				{
					onRestoreFailureDetected();
				}
			}
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x00041668 File Offset: 0x0003F868
		private static bool RestoreBackup(string path)
		{
			bool flag = false;
			try
			{
				string text = path + ".bac";
				SavesSystem.UpgradeSaveFileAssemblyInfo(Path.Combine(Application.persistentDataPath, text));
				ES3.CacheFile(text);
				ES3.DeleteFile(path, SavesSystem.settings);
				ES3.RestoreBackup(path, SavesSystem.settings);
				ES3.StoreCachedFile(path);
				ES3.CacheFile(path);
				ES3.CacheFile(path);
				flag = true;
			}
			catch
			{
				Debug.Log("默认备份损坏。");
			}
			if (!flag)
			{
				SavesSystem.RestoreFailureMarker = true;
				Debug.LogError("恢复默认备份失败");
				ES3.DeleteFile(path);
				ES3.Save<bool>("Created", true, path);
				ES3.StoreCachedFile(path);
				ES3.CacheFile(path);
				Action onRestoreFailureDetected = SavesSystem.OnRestoreFailureDetected;
				if (onRestoreFailureDetected != null)
				{
					onRestoreFailureDetected();
				}
			}
			return flag;
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x00041728 File Offset: 0x0003F928
		// (set) Token: 0x060010C9 RID: 4297 RVA: 0x0004174F File Offset: 0x0003F94F
		private static DateTime LastSavedTime
		{
			get
			{
				if (SavesSystem._lastSavedTime > DateTime.UtcNow)
				{
					SavesSystem._lastSavedTime = DateTime.UtcNow;
					GameManager.TimeTravelDetected();
				}
				return SavesSystem._lastSavedTime;
			}
			set
			{
				SavesSystem._lastSavedTime = value;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x060010CA RID: 4298 RVA: 0x00041757 File Offset: 0x0003F957
		private static TimeSpan TimeSinceLastSave
		{
			get
			{
				return DateTime.UtcNow - SavesSystem.LastSavedTime;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x060010CB RID: 4299 RVA: 0x00041768 File Offset: 0x0003F968
		// (set) Token: 0x060010CC RID: 4300 RVA: 0x0004178F File Offset: 0x0003F98F
		private static DateTime LastIndexedBackupTime
		{
			get
			{
				if (SavesSystem._lastIndexedBackupTime > DateTime.UtcNow)
				{
					SavesSystem._lastIndexedBackupTime = DateTime.UtcNow;
					GameManager.TimeTravelDetected();
				}
				return SavesSystem._lastIndexedBackupTime;
			}
			set
			{
				SavesSystem._lastIndexedBackupTime = value;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x060010CD RID: 4301 RVA: 0x00041797 File Offset: 0x0003F997
		private static TimeSpan TimeSinceLastIndexedBackup
		{
			get
			{
				return DateTime.UtcNow - SavesSystem.LastIndexedBackupTime;
			}
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x000417A8 File Offset: 0x0003F9A8
		public DateTime GetSaveTimeUTC(int slot = -1)
		{
			if (slot < 0)
			{
				slot = SavesSystem.CurrentSlot;
			}
			if (!SavesSystem.KeyExisits("SaveTime", slot))
			{
				return default(DateTime);
			}
			return DateTime.FromBinary(SavesSystem.Load<long>("SaveTime", slot));
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x000417E8 File Offset: 0x0003F9E8
		public DateTime GetSaveTimeLocal(int slot = -1)
		{
			if (slot < 0)
			{
				slot = SavesSystem.CurrentSlot;
			}
			DateTime saveTimeUTC = this.GetSaveTimeUTC(slot);
			if (saveTimeUTC == default(DateTime))
			{
				return default(DateTime);
			}
			return saveTimeUTC.ToLocalTime();
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x0004182C File Offset: 0x0003FA2C
		public static void SaveFile(bool writeSaveTime = true)
		{
			TimeSpan timeSinceLastIndexedBackup = SavesSystem.TimeSinceLastIndexedBackup;
			SavesSystem.LastSavedTime = DateTime.UtcNow;
			if (writeSaveTime)
			{
				SavesSystem.Save<long>("SaveTime", DateTime.UtcNow.ToBinary());
			}
			SavesSystem.saving = true;
			SavesSystem.CreateBackup();
			if (timeSinceLastIndexedBackup > TimeSpan.FromMinutes(5.0))
			{
				SavesSystem.CreateIndexedBackup(-1);
			}
			SavesSystem.SetAsOldGame();
			ES3.StoreCachedFile(SavesSystem.CurrentFilePath);
			SavesSystem.saving = false;
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x0004189D File Offset: 0x0003FA9D
		private static void CacheFile()
		{
			SavesSystem.CacheFile(SavesSystem.CurrentSlot);
			SavesSystem.cached = true;
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x000418B0 File Offset: 0x0003FAB0
		private static void CacheFile(int slot)
		{
			if (slot == SavesSystem.CurrentSlot && SavesSystem.cached)
			{
				return;
			}
			string filePath = SavesSystem.GetFilePath(slot);
			if (!SavesSystem.CacheFile(filePath))
			{
				Debug.Log("尝试恢复 indexed backups");
				List<SavesSystem.BackupInfo> list = (from e in SavesSystem.GetBackupList(filePath, slot)
				where e.exists
				select e).ToList<SavesSystem.BackupInfo>();
				list.Sort(delegate(SavesSystem.BackupInfo a, SavesSystem.BackupInfo b)
				{
					if (!(a.Time > b.Time))
					{
						return 1;
					}
					return -1;
				});
				if (list.Count > 0)
				{
					for (int i = 0; i < list.Count; i++)
					{
						SavesSystem.BackupInfo backupInfo = list[i];
						try
						{
							Debug.Log(string.Format("Restoreing {0}.bac.{1} \t", slot, backupInfo.index) + backupInfo.Time.ToString("MM/dd HH:mm:ss"));
							SavesSystem.RestoreIndexedBackup(slot, backupInfo.index);
							break;
						}
						catch
						{
							Debug.LogError(string.Format("slot:{0} backup_index:{1} 恢复失败。", slot, backupInfo.index));
						}
					}
				}
			}
			if (!ES3.FileExists(filePath))
			{
				ES3.Save<bool>("Created", true, filePath);
				ES3.StoreCachedFile(filePath);
				ES3.CacheFile(filePath);
			}
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x00041A0C File Offset: 0x0003FC0C
		private static bool CacheFile(string path)
		{
			bool result;
			try
			{
				ES3.CacheFile(path);
				result = true;
			}
			catch
			{
				result = SavesSystem.RestoreBackup(path);
			}
			return result;
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00041A40 File Offset: 0x0003FC40
		public static void Save<T>(string prefix, string key, T value)
		{
			SavesSystem.Save<T>(prefix + key, value);
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00041A4F File Offset: 0x0003FC4F
		public static void Save<T>(string realKey, T value)
		{
			if (!SavesSystem.cached)
			{
				SavesSystem.CacheFile();
			}
			if (string.IsNullOrWhiteSpace(SavesSystem.CurrentFilePath))
			{
				Debug.Log("Save failed " + realKey);
				return;
			}
			ES3.Save<T>(realKey, value, SavesSystem.CurrentFilePath);
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x00041A86 File Offset: 0x0003FC86
		public static T Load<T>(string prefix, string key)
		{
			return SavesSystem.Load<T>(prefix + key);
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x00041A94 File Offset: 0x0003FC94
		public static T Load<T>(string realKey)
		{
			if (!SavesSystem.cached)
			{
				SavesSystem.CacheFile();
			}
			string.IsNullOrWhiteSpace(realKey);
			if (ES3.KeyExists(realKey, SavesSystem.CurrentFilePath))
			{
				return ES3.Load<T>(realKey, SavesSystem.CurrentFilePath);
			}
			return default(T);
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00041AD6 File Offset: 0x0003FCD6
		public static bool KeyExisits(string prefix, string key)
		{
			return ES3.KeyExists(prefix + key);
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00041AE4 File Offset: 0x0003FCE4
		public static bool KeyExisits(string realKey)
		{
			if (!SavesSystem.cached)
			{
				SavesSystem.CacheFile();
			}
			return ES3.KeyExists(realKey, SavesSystem.CurrentFilePath);
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00041B00 File Offset: 0x0003FD00
		public static bool KeyExisits(string realKey, int slotIndex)
		{
			if (slotIndex == SavesSystem.CurrentSlot)
			{
				return SavesSystem.KeyExisits(realKey);
			}
			string filePath = SavesSystem.GetFilePath(slotIndex);
			SavesSystem.CacheFile(slotIndex);
			return ES3.KeyExists(realKey, filePath);
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x00041B30 File Offset: 0x0003FD30
		public static T Load<T>(string realKey, int slotIndex)
		{
			if (slotIndex == SavesSystem.CurrentSlot)
			{
				return SavesSystem.Load<T>(realKey);
			}
			string filePath = SavesSystem.GetFilePath(slotIndex);
			SavesSystem.CacheFile(slotIndex);
			if (ES3.KeyExists(realKey, filePath))
			{
				return ES3.Load<T>(realKey, filePath);
			}
			return default(T);
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x060010DC RID: 4316 RVA: 0x00041B73 File Offset: 0x0003FD73
		public static string GlobalSaveDataFilePath
		{
			get
			{
				return Path.Combine(SavesSystem.SavesFolder, SavesSystem.GlobalSaveDataFileName);
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x060010DD RID: 4317 RVA: 0x00041B84 File Offset: 0x0003FD84
		public static string GlobalSaveDataFileName
		{
			get
			{
				return "Global.json";
			}
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00041B8B File Offset: 0x0003FD8B
		public static void SaveGlobal<T>(string key, T value)
		{
			if (!SavesSystem.globalCached)
			{
				SavesSystem.CacheFile(SavesSystem.GlobalSaveDataFilePath);
				SavesSystem.globalCached = true;
			}
			ES3.Save<T>(key, value, SavesSystem.GlobalSaveDataFilePath);
			SavesSystem.CreateBackup(SavesSystem.GlobalSaveDataFilePath);
			ES3.StoreCachedFile(SavesSystem.GlobalSaveDataFilePath);
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x00041BC5 File Offset: 0x0003FDC5
		public static T LoadGlobal<T>(string key, T defaultValue = default(T))
		{
			if (!SavesSystem.globalCached)
			{
				SavesSystem.CacheFile(SavesSystem.GlobalSaveDataFilePath);
				SavesSystem.globalCached = true;
			}
			if (ES3.KeyExists(key, SavesSystem.GlobalSaveDataFilePath))
			{
				return ES3.Load<T>(key, SavesSystem.GlobalSaveDataFilePath);
			}
			return defaultValue;
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00041BF9 File Offset: 0x0003FDF9
		public static void CollectSaveData()
		{
			Action onCollectSaveData = SavesSystem.OnCollectSaveData;
			if (onCollectSaveData == null)
			{
				return;
			}
			onCollectSaveData();
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00041C0A File Offset: 0x0003FE0A
		public static bool IsOldGame()
		{
			return SavesSystem.Load<bool>("IsOldGame");
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00041C16 File Offset: 0x0003FE16
		public static bool IsOldGame(int index)
		{
			return SavesSystem.Load<bool>("IsOldGame", index);
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x00041C23 File Offset: 0x0003FE23
		private static void SetAsOldGame()
		{
			SavesSystem.Save<bool>("IsOldGame", true);
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00041C30 File Offset: 0x0003FE30
		public static void DeleteCurrentSave()
		{
			ES3.CacheFile(SavesSystem.CurrentFilePath);
			ES3.DeleteFile(SavesSystem.CurrentFilePath);
			ES3.Save<bool>("Created", false, SavesSystem.CurrentFilePath);
			ES3.StoreCachedFile(SavesSystem.CurrentFilePath);
			Debug.Log(string.Format("已删除存档{0}", SavesSystem.CurrentSlot));
			Action onSaveDeleted = SavesSystem.OnSaveDeleted;
			if (onSaveDeleted == null)
			{
				return;
			}
			onSaveDeleted();
		}

		// Token: 0x04000D46 RID: 3398
		private static int? _currentSlot = null;

		// Token: 0x04000D47 RID: 3399
		private static bool saving;

		// Token: 0x04000D48 RID: 3400
		private static ES3Settings settings = ES3Settings.defaultSettings;

		// Token: 0x04000D49 RID: 3401
		private static bool cached;

		// Token: 0x04000D4F RID: 3407
		private const int BackupListCount = 10;

		// Token: 0x04000D50 RID: 3408
		private static DateTime _lastSavedTime = DateTime.MinValue;

		// Token: 0x04000D51 RID: 3409
		private static DateTime _lastIndexedBackupTime = DateTime.MinValue;

		// Token: 0x04000D52 RID: 3410
		private static bool globalCached;

		// Token: 0x04000D53 RID: 3411
		private static ES3Settings GlobalFileSetting = new ES3Settings(null, null)
		{
			location = ES3.Location.File
		};

		// Token: 0x02000513 RID: 1299
		public struct BackupInfo
		{
			// Token: 0x1700075A RID: 1882
			// (get) Token: 0x060027E5 RID: 10213 RVA: 0x00092302 File Offset: 0x00090502
			public bool TimeValid
			{
				get
				{
					return this.time_raw > 0L;
				}
			}

			// Token: 0x1700075B RID: 1883
			// (get) Token: 0x060027E6 RID: 10214 RVA: 0x0009230E File Offset: 0x0009050E
			public DateTime Time
			{
				get
				{
					return DateTime.FromBinary(this.time_raw);
				}
			}

			// Token: 0x04001E16 RID: 7702
			public int slot;

			// Token: 0x04001E17 RID: 7703
			public int index;

			// Token: 0x04001E18 RID: 7704
			public string path;

			// Token: 0x04001E19 RID: 7705
			public bool exists;

			// Token: 0x04001E1A RID: 7706
			public long time_raw;
		}
	}
}
