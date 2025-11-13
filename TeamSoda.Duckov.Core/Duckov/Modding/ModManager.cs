using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Saves;
using Sirenix.Utilities;
using UnityEngine;

namespace Duckov.Modding
{
	// Token: 0x0200026F RID: 623
	public class ModManager : MonoBehaviour
	{
		// Token: 0x14000080 RID: 128
		// (add) Token: 0x0600137A RID: 4986 RVA: 0x00048D24 File Offset: 0x00046F24
		// (remove) Token: 0x0600137B RID: 4987 RVA: 0x00048D58 File Offset: 0x00046F58
		public static event Action OnReorder;

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x0600137C RID: 4988 RVA: 0x00048D8B File Offset: 0x00046F8B
		// (set) Token: 0x0600137D RID: 4989 RVA: 0x00048D98 File Offset: 0x00046F98
		public static bool AllowActivatingMod
		{
			get
			{
				return SavesSystem.LoadGlobal<bool>("AllowLoadingMod", false);
			}
			set
			{
				SavesSystem.SaveGlobal<bool>("AllowLoadingMod", value);
				if (ModManager.Instance != null && value)
				{
					ModManager.Instance.ScanAndActivateMods();
				}
			}
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x00048DBF File Offset: 0x00046FBF
		private void Awake()
		{
			if (this.modParent == null)
			{
				this.modParent = base.transform;
			}
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x00048DDB File Offset: 0x00046FDB
		private void Start()
		{
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x00048DE0 File Offset: 0x00046FE0
		public void ScanAndActivateMods()
		{
			if (!ModManager.AllowActivatingMod)
			{
				return;
			}
			ModManager.Rescan();
			foreach (ModInfo modInfo in ModManager.modInfos)
			{
				if (!this.activeMods.ContainsKey(modInfo.name))
				{
					bool flag = this.ShouldActivateMod(modInfo);
					Debug.Log(string.Format("ModActive_{0}: {1}", modInfo.name, flag));
					if (flag && this.ActivateMod(modInfo) == null)
					{
						this.SetShouldActivateMod(modInfo, false);
					}
				}
			}
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x00048E88 File Offset: 0x00047088
		private static void SortModInfosByPriority()
		{
			ModManager.modInfos.Sort(delegate(ModInfo a, ModInfo b)
			{
				int modPriority = ModManager.GetModPriority(a.name);
				int modPriority2 = ModManager.GetModPriority(b.name);
				return modPriority - modPriority2;
			});
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Sorted mods:");
			foreach (ModInfo modInfo in ModManager.modInfos)
			{
				stringBuilder.AppendLine(modInfo.name);
			}
			Debug.Log(stringBuilder);
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06001382 RID: 4994 RVA: 0x00048F24 File Offset: 0x00047124
		private static ES3Settings settings
		{
			get
			{
				if (ModManager._settings == null)
				{
					ModManager._settings = new ES3Settings(null, null)
					{
						location = ES3.Location.File,
						path = "Saves/Mods.ES3"
					};
				}
				return ModManager._settings;
			}
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00048F50 File Offset: 0x00047150
		private static void Save<T>(string key, T value)
		{
			try
			{
				ES3.Save<T>(key, value, ModManager.settings);
				ES3.CreateBackup(ModManager.settings);
			}
			catch (Exception exception)
			{
				Debug.LogError("Failed saving mod info.");
				Debug.LogException(exception);
			}
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x00048F98 File Offset: 0x00047198
		private static T Load<T>(string key, T defaultValue = default(T))
		{
			T result;
			try
			{
				result = ES3.Load<T>(key, defaultValue, ModManager.settings);
			}
			catch (Exception exception)
			{
				Debug.LogError("Failed loading mod info.");
				ES3.RestoreBackup(ModManager.settings);
				Debug.LogException(exception);
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00048FE4 File Offset: 0x000471E4
		public static void SetModPriority(string name, int priority)
		{
			ModManager.Save<int>("priority_" + name, priority);
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00048FF7 File Offset: 0x000471F7
		public static int GetModPriority(string name)
		{
			return ModManager.Load<int>("priority_" + name, 0);
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x0004900A File Offset: 0x0004720A
		private void SetShouldActivateMod(ModInfo info, bool value)
		{
			SavesSystem.SaveGlobal<bool>("ModActive_" + info.name, value);
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x00049022 File Offset: 0x00047222
		private bool ShouldActivateMod(ModInfo info)
		{
			return SavesSystem.LoadGlobal<bool>("ModActive_" + info.name, false);
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06001389 RID: 5001 RVA: 0x0004903A File Offset: 0x0004723A
		public static string DefaultModFolderPath
		{
			get
			{
				return Path.Combine(Application.dataPath, "Mods");
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x0600138A RID: 5002 RVA: 0x0004904B File Offset: 0x0004724B
		public static ModManager Instance
		{
			get
			{
				return GameManager.ModManager;
			}
		}

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x0600138B RID: 5003 RVA: 0x00049054 File Offset: 0x00047254
		// (remove) Token: 0x0600138C RID: 5004 RVA: 0x00049088 File Offset: 0x00047288
		public static event Action<List<ModInfo>> OnScan;

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x0600138D RID: 5005 RVA: 0x000490BC File Offset: 0x000472BC
		// (remove) Token: 0x0600138E RID: 5006 RVA: 0x000490F0 File Offset: 0x000472F0
		public static event Action<ModInfo, ModBehaviour> OnModActivated;

		// Token: 0x14000083 RID: 131
		// (add) Token: 0x0600138F RID: 5007 RVA: 0x00049124 File Offset: 0x00047324
		// (remove) Token: 0x06001390 RID: 5008 RVA: 0x00049158 File Offset: 0x00047358
		public static event Action<ModInfo, ModBehaviour> OnModWillBeDeactivated;

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06001391 RID: 5009 RVA: 0x0004918C File Offset: 0x0004738C
		// (remove) Token: 0x06001392 RID: 5010 RVA: 0x000491C0 File Offset: 0x000473C0
		public static event Action OnModStatusChanged;

		// Token: 0x06001393 RID: 5011 RVA: 0x000491F4 File Offset: 0x000473F4
		public static void Rescan()
		{
			ModManager.modInfos.Clear();
			if (Directory.Exists(ModManager.DefaultModFolderPath))
			{
				string[] directories = Directory.GetDirectories(ModManager.DefaultModFolderPath);
				for (int i = 0; i < directories.Length; i++)
				{
					ModInfo item;
					if (ModManager.TryProcessModFolder(directories[i], out item, false, 0UL))
					{
						ModManager.modInfos.Add(item);
					}
				}
			}
			Action<List<ModInfo>> onScan = ModManager.OnScan;
			if (onScan != null)
			{
				onScan(ModManager.modInfos);
			}
			ModManager.SortModInfosByPriority();
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00049264 File Offset: 0x00047464
		private static void RegeneratePriorities()
		{
			for (int i = 0; i < ModManager.modInfos.Count; i++)
			{
				string name = ModManager.modInfos[i].name;
				if (!string.IsNullOrWhiteSpace(name))
				{
					ModManager.SetModPriority(name, i);
				}
			}
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x000492A8 File Offset: 0x000474A8
		public static bool Reorder(int fromIndex, int toIndex)
		{
			if (fromIndex == toIndex)
			{
				return false;
			}
			if (fromIndex < 0 || fromIndex >= ModManager.modInfos.Count)
			{
				return false;
			}
			if (toIndex < 0 || toIndex >= ModManager.modInfos.Count)
			{
				return false;
			}
			ModInfo item = ModManager.modInfos[fromIndex];
			ModManager.modInfos.RemoveAt(fromIndex);
			ModManager.modInfos.Insert(toIndex, item);
			ModManager.RegeneratePriorities();
			Action onReorder = ModManager.OnReorder;
			if (onReorder != null)
			{
				onReorder();
			}
			return true;
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x0004931C File Offset: 0x0004751C
		public static bool TryProcessModFolder(string path, out ModInfo info, bool isSteamItem = false, ulong publishedFileId = 0UL)
		{
			info = default(ModInfo);
			info.path = path;
			string path2 = Path.Combine(path, "info.ini");
			if (!File.Exists(path2))
			{
				return false;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			using (StreamReader streamReader = File.OpenText(path2))
			{
				while (!streamReader.EndOfStream)
				{
					string text = streamReader.ReadLine().Trim();
					if (!string.IsNullOrWhiteSpace(text) && !text.StartsWith('['))
					{
						string[] array = text.Split('=', StringSplitOptions.None);
						if (array.Length == 2)
						{
							string key = array[0].Trim();
							string value = array[1].Trim();
							dictionary[key] = value;
						}
					}
				}
			}
			string text2;
			if (!dictionary.TryGetValue("name", out text2))
			{
				Debug.LogError("Failed to get name value in mod info.ini file. Aborting.\n" + path);
				return false;
			}
			string displayName;
			if (!dictionary.TryGetValue("displayName", out displayName))
			{
				displayName = text2;
				Debug.LogError("Failed to get displayName value in mod info.ini file.\n" + path);
			}
			string text3;
			if (!dictionary.TryGetValue("description", out text3))
			{
				text3 = "?";
				Debug.LogError("Failed to get description value in mod info.ini file.\n" + path);
			}
			ulong num = 0UL;
			string s;
			if (dictionary.TryGetValue("publishedFileId", out s) && !ulong.TryParse(s, out num))
			{
				Debug.LogError("Invalid publishedFileId");
			}
			if (!isSteamItem)
			{
				publishedFileId = num;
			}
			else if (publishedFileId != num)
			{
				Debug.LogError("PublishFileId not match.\npath:" + path);
			}
			info.name = text2;
			info.displayName = displayName;
			try
			{
				info.description = Regex.Unescape(text3);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				info.description = text3;
			}
			info.publishedFileId = publishedFileId;
			info.isSteamItem = isSteamItem;
			string dllPath = info.dllPath;
			info.dllFound = File.Exists(dllPath);
			if (!info.dllFound)
			{
				Debug.LogError("Dll for mod " + text2 + " not found.\nExpecting: " + dllPath);
			}
			string path3 = Path.Combine(path, "preview.png");
			if (File.Exists(path3))
			{
				using (FileStream fileStream = File.OpenRead(path3))
				{
					Texture2D texture2D = new Texture2D(256, 256);
					byte[] array2 = new byte[fileStream.Length];
					fileStream.Read(array2);
					if (texture2D.LoadImage(array2))
					{
						info.preview = texture2D;
					}
				}
			}
			return true;
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x00049580 File Offset: 0x00047780
		public static bool IsModActive(ModInfo info, out ModBehaviour instance)
		{
			instance = null;
			return !(ModManager.Instance == null) && ModManager.Instance.activeMods.TryGetValue(info.name, out instance) && instance != null;
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x000495B8 File Offset: 0x000477B8
		public ModBehaviour GetActiveModBehaviour(ModInfo info)
		{
			ModBehaviour result;
			if (this.activeMods.TryGetValue(info.name, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x000495E0 File Offset: 0x000477E0
		public void DeactivateMod(ModInfo info)
		{
			ModBehaviour activeModBehaviour = this.GetActiveModBehaviour(info);
			if (activeModBehaviour == null)
			{
				return;
			}
			try
			{
				activeModBehaviour.NotifyBeforeDeactivate();
				Action<ModInfo, ModBehaviour> onModWillBeDeactivated = ModManager.OnModWillBeDeactivated;
				if (onModWillBeDeactivated != null)
				{
					onModWillBeDeactivated(info, activeModBehaviour);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			this.activeMods.Remove(info.name);
			try
			{
				UnityEngine.Object.Destroy(activeModBehaviour.gameObject);
				Action onModStatusChanged = ModManager.OnModStatusChanged;
				if (onModStatusChanged != null)
				{
					onModStatusChanged();
				}
			}
			catch (Exception exception2)
			{
				Debug.LogException(exception2);
			}
			this.SetShouldActivateMod(info, false);
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x0004967C File Offset: 0x0004787C
		public ModBehaviour ActivateMod(ModInfo info)
		{
			if (!ModManager.AllowActivatingMod)
			{
				Debug.LogError("Activating mod not allowed! \nUser must first interact with the agreement UI in order to allow activating mods.");
				return null;
			}
			string dllPath = info.dllPath;
			string name = info.name;
			ModBehaviour context;
			if (ModManager.IsModActive(info, out context))
			{
				Debug.LogError("Mod " + info.name + " instance already exists! Abort. Path: " + info.path, context);
				return null;
			}
			Debug.Log("Loading mod dll at path: " + dllPath);
			Type type;
			try
			{
				type = Assembly.LoadFrom(dllPath).GetType(name + ".ModBehaviour");
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				string arg = "Mod loading failed: " + name + "\n" + ex.Message;
				Action<string, string> onModLoadingFailed = ModManager.OnModLoadingFailed;
				if (onModLoadingFailed != null)
				{
					onModLoadingFailed(info.dllPath, arg);
				}
				return null;
			}
			if (type == null || !type.InheritsFrom<ModBehaviour>())
			{
				Debug.LogError("Cannot load mod.\nA type named " + name + ".ModBehaviour is expected, and it should inherit from Duckov.Modding.ModBehaviour.");
				return null;
			}
			GameObject gameObject = new GameObject(name);
			ModBehaviour modBehaviour;
			try
			{
				modBehaviour = (gameObject.AddComponent(type) as ModBehaviour);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				Debug.LogError("Failed to create component for mod " + name);
				return null;
			}
			if (modBehaviour == null)
			{
				UnityEngine.Object.Destroy(gameObject);
				Debug.LogError("Failed to create component for mod " + name);
				return null;
			}
			gameObject.transform.SetParent(base.transform);
			Debug.Log("Mod Loaded: " + info.name);
			try
			{
				modBehaviour.Setup(this, info);
			}
			catch (Exception exception2)
			{
				Debug.LogException(exception2);
				return null;
			}
			this.activeMods[info.name] = modBehaviour;
			try
			{
				Action<ModInfo, ModBehaviour> onModActivated = ModManager.OnModActivated;
				if (onModActivated != null)
				{
					onModActivated(info, modBehaviour);
				}
				Action onModStatusChanged = ModManager.OnModStatusChanged;
				if (onModStatusChanged != null)
				{
					onModStatusChanged();
				}
			}
			catch (Exception exception3)
			{
				Debug.LogException(exception3);
			}
			this.SetShouldActivateMod(info, true);
			return modBehaviour;
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x00049884 File Offset: 0x00047A84
		internal static void WriteModInfoINI(ModInfo modInfo)
		{
			string path = Path.Combine(modInfo.path, "info.ini");
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			using (FileStream fileStream = File.Create(path))
			{
				StreamWriter streamWriter = new StreamWriter(fileStream);
				streamWriter.WriteLine("name = " + modInfo.name);
				streamWriter.WriteLine("displayName = " + modInfo.displayName);
				string text = modInfo.description;
				try
				{
					text = Regex.Escape(text);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					text = "Cannot escape description";
				}
				streamWriter.WriteLine("description = " + text);
				streamWriter.WriteLine("");
				streamWriter.WriteLine(string.Format("publishedFileId = {0}", modInfo.publishedFileId));
				streamWriter.Close();
			}
		}

		// Token: 0x04000E7D RID: 3709
		[SerializeField]
		private Transform modParent;

		// Token: 0x04000E7F RID: 3711
		public static Action<string, string> OnModLoadingFailed;

		// Token: 0x04000E80 RID: 3712
		private static ES3Settings _settings;

		// Token: 0x04000E81 RID: 3713
		public static List<ModInfo> modInfos = new List<ModInfo>();

		// Token: 0x04000E82 RID: 3714
		private Dictionary<string, ModBehaviour> activeMods = new Dictionary<string, ModBehaviour>();
	}
}
