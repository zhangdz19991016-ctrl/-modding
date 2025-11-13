using System;
using System.IO;
using Saves;
using UnityEngine;

namespace Duckov.Options
{
	// Token: 0x02000261 RID: 609
	public class OptionsManager : MonoBehaviour
	{
		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06001307 RID: 4871 RVA: 0x00047F53 File Offset: 0x00046153
		private static string Folder
		{
			get
			{
				return SavesSystem.SavesFolder;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06001308 RID: 4872 RVA: 0x00047F5A File Offset: 0x0004615A
		public static string FilePath
		{
			get
			{
				return Path.Combine(OptionsManager.Folder, "Options.ES3");
			}
		}

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x06001309 RID: 4873 RVA: 0x00047F6C File Offset: 0x0004616C
		// (remove) Token: 0x0600130A RID: 4874 RVA: 0x00047FA0 File Offset: 0x000461A0
		public static event Action<string> OnOptionsChanged;

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x0600130B RID: 4875 RVA: 0x00047FD3 File Offset: 0x000461D3
		private static ES3Settings SaveSettings
		{
			get
			{
				if (OptionsManager._saveSettings == null)
				{
					OptionsManager._saveSettings = new ES3Settings(true);
					OptionsManager._saveSettings.path = OptionsManager.FilePath;
					OptionsManager._saveSettings.location = ES3.Location.File;
				}
				return OptionsManager._saveSettings;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x0600130C RID: 4876 RVA: 0x00048006 File Offset: 0x00046206
		// (set) Token: 0x0600130D RID: 4877 RVA: 0x00048017 File Offset: 0x00046217
		public static float MouseSensitivity
		{
			get
			{
				return OptionsManager.Load<float>("MouseSensitivity", 10f);
			}
			set
			{
				OptionsManager.Save<float>("MouseSensitivity", value);
			}
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x00048024 File Offset: 0x00046224
		public static void Save<T>(string key, T obj)
		{
			if (string.IsNullOrEmpty(key))
			{
				return;
			}
			try
			{
				ES3.Save<T>(key, obj, OptionsManager.SaveSettings);
				Action<string> onOptionsChanged = OptionsManager.OnOptionsChanged;
				if (onOptionsChanged != null)
				{
					onOptionsChanged(key);
				}
				ES3.CreateBackup(OptionsManager.SaveSettings);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				Debug.LogError("Error: Failed saving options: " + key);
			}
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x0004808C File Offset: 0x0004628C
		public static T Load<T>(string key, T defaultValue = default(T))
		{
			T result;
			if (string.IsNullOrEmpty(key))
			{
				result = default(T);
				return result;
			}
			try
			{
				if (ES3.KeyExists(key, OptionsManager.SaveSettings))
				{
					result = ES3.Load<T>(key, OptionsManager.SaveSettings);
				}
				else
				{
					ES3.Save<T>(key, defaultValue, OptionsManager.SaveSettings);
					result = defaultValue;
				}
			}
			catch
			{
				if (ES3.RestoreBackup(OptionsManager.SaveSettings))
				{
					try
					{
						if (ES3.KeyExists(key, OptionsManager.SaveSettings))
						{
							return ES3.Load<T>(key, OptionsManager.SaveSettings);
						}
						ES3.Save<T>(key, defaultValue, OptionsManager.SaveSettings);
						return defaultValue;
					}
					catch
					{
						Debug.LogError("[OPTIONS MANAGER] Failed restoring backup");
					}
				}
				ES3.DeleteFile(OptionsManager.SaveSettings);
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x04000E54 RID: 3668
		public const string FileName = "Options.ES3";

		// Token: 0x04000E56 RID: 3670
		private static ES3Settings _saveSettings;
	}
}
