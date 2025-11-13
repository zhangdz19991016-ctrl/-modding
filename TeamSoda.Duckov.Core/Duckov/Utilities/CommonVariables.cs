using System;
using Saves;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x020003FE RID: 1022
	public class CommonVariables : MonoBehaviour
	{
		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06002508 RID: 9480 RVA: 0x00080D13 File Offset: 0x0007EF13
		public CustomDataCollection Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x00080D1C File Offset: 0x0007EF1C
		private void Awake()
		{
			if (CommonVariables.instance == null)
			{
				CommonVariables.instance = this;
			}
			else
			{
				Debug.LogWarning("检测到多个Common Variables");
			}
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			SavesSystem.OnSetFile += this.OnSetSaveFile;
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x00080D6A File Offset: 0x0007EF6A
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
			SavesSystem.OnSetFile -= this.OnSetSaveFile;
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x00080D8E File Offset: 0x0007EF8E
		private void OnSetSaveFile()
		{
			this.Load();
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x00080D96 File Offset: 0x0007EF96
		private void OnCollectSaveData()
		{
			this.Save();
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x00080D9E File Offset: 0x0007EF9E
		private void Start()
		{
			this.Load();
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x00080DA6 File Offset: 0x0007EFA6
		private void Save()
		{
			SavesSystem.Save<CustomDataCollection>("CommonVariables", "Data", this.data);
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x00080DBD File Offset: 0x0007EFBD
		private void Load()
		{
			this.data = SavesSystem.Load<CustomDataCollection>("CommonVariables", "Data");
			if (this.data == null)
			{
				this.data = new CustomDataCollection();
			}
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x00080DE7 File Offset: 0x0007EFE7
		public static void SetFloat(string key, float value)
		{
			if (CommonVariables.instance)
			{
				CommonVariables.instance.Data.SetFloat(key, value, true);
			}
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x00080E07 File Offset: 0x0007F007
		public static void SetInt(string key, int value)
		{
			if (CommonVariables.instance)
			{
				CommonVariables.instance.Data.SetInt(key, value, true);
			}
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x00080E27 File Offset: 0x0007F027
		public static void SetBool(string key, bool value)
		{
			if (CommonVariables.instance)
			{
				CommonVariables.instance.Data.SetBool(key, value, true);
			}
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x00080E47 File Offset: 0x0007F047
		public static void SetString(string key, string value)
		{
			if (CommonVariables.instance)
			{
				CommonVariables.instance.Data.SetString(key, value, true);
			}
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x00080E67 File Offset: 0x0007F067
		public static float GetFloat(string key, float defaultValue = 0f)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetFloat(key, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x00080E88 File Offset: 0x0007F088
		public static int GetInt(string key, int defaultValue = 0)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetInt(key, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x00080EA9 File Offset: 0x0007F0A9
		public static bool GetBool(string key, bool defaultValue = false)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetBool(key, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x00080ECA File Offset: 0x0007F0CA
		public static string GetString(string key, string defaultValue = "")
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetString(key, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x00080EEB File Offset: 0x0007F0EB
		public static float GetFloat(int hash, float defaultValue = 0f)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetFloat(hash, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x00080F0C File Offset: 0x0007F10C
		public static int GetInt(int hash, int defaultValue = 0)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetInt(hash, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x00080F2D File Offset: 0x0007F12D
		public static bool GetBool(int hash, bool defaultValue = false)
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetBool(hash, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x00080F4E File Offset: 0x0007F14E
		public static string GetString(int hash, string defaultValue = "")
		{
			if (CommonVariables.instance)
			{
				return CommonVariables.instance.Data.GetString(hash, defaultValue);
			}
			return defaultValue;
		}

		// Token: 0x04001931 RID: 6449
		private static CommonVariables instance;

		// Token: 0x04001932 RID: 6450
		[SerializeField]
		private CustomDataCollection data;

		// Token: 0x04001933 RID: 6451
		private const string saves_prefix = "CommonVariables";

		// Token: 0x04001934 RID: 6452
		private const string saves_key = "Data";
	}
}
