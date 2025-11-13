using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Duckov.UI;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000236 RID: 566
	public class EXPManager : MonoBehaviour, ISaveDataProvider
	{
		// Token: 0x1700030A RID: 778
		// (get) Token: 0x060011B0 RID: 4528 RVA: 0x00044F05 File Offset: 0x00043105
		public static EXPManager Instance
		{
			get
			{
				return EXPManager.instance;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x060011B1 RID: 4529 RVA: 0x00044F0C File Offset: 0x0004310C
		private string LevelChangeNotificationFormat
		{
			get
			{
				return this.levelChangeNotificationFormatKey.ToPlainText();
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x060011B2 RID: 4530 RVA: 0x00044F19 File Offset: 0x00043119
		// (set) Token: 0x060011B3 RID: 4531 RVA: 0x00044F38 File Offset: 0x00043138
		public static long EXP
		{
			get
			{
				if (EXPManager.instance == null)
				{
					return 0L;
				}
				return EXPManager.instance.point;
			}
			private set
			{
				if (EXPManager.instance == null)
				{
					return;
				}
				int level = EXPManager.Level;
				EXPManager.instance.point = value;
				Action<long> action = EXPManager.onExpChanged;
				if (action != null)
				{
					action(value);
				}
				int level2 = EXPManager.Level;
				if (level != level2)
				{
					EXPManager.OnLevelChanged(level, level2);
				}
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x00044F86 File Offset: 0x00043186
		public static int Level
		{
			get
			{
				if (EXPManager.instance == null)
				{
					return 0;
				}
				return EXPManager.instance.LevelFromExp(EXPManager.EXP);
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x060011B5 RID: 4533 RVA: 0x00044FA6 File Offset: 0x000431A6
		public static long CachedExp
		{
			get
			{
				if (EXPManager.instance == null)
				{
					return 0L;
				}
				return EXPManager.instance.cachedExp;
			}
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x00044FC2 File Offset: 0x000431C2
		private static void OnLevelChanged(int oldLevel, int newLevel)
		{
			Action<int, int> action = EXPManager.onLevelChanged;
			if (action != null)
			{
				action(oldLevel, newLevel);
			}
			if (EXPManager.Instance == null)
			{
				return;
			}
			NotificationText.Push(EXPManager.Instance.LevelChangeNotificationFormat.Format(new
			{
				level = newLevel
			}));
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00044FFE File Offset: 0x000431FE
		public static bool AddExp(int amount)
		{
			if (EXPManager.instance == null)
			{
				return false;
			}
			EXPManager.EXP += (long)amount;
			return true;
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x0004501D File Offset: 0x0004321D
		private void CacheExp()
		{
			this.cachedExp = this.point;
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0004502B File Offset: 0x0004322B
		public object GenerateSaveData()
		{
			return this.point;
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x00045038 File Offset: 0x00043238
		public void SetupSaveData(object data)
		{
			if (data is long)
			{
				long num = (long)data;
				this.point = num;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x060011BB RID: 4539 RVA: 0x0004505B File Offset: 0x0004325B
		private string realKey
		{
			get
			{
				return "EXP_Value";
			}
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x00045064 File Offset: 0x00043264
		private void Load()
		{
			if (SavesSystem.KeyExisits(this.realKey))
			{
				long num = SavesSystem.Load<long>(this.realKey);
				this.SetupSaveData(num);
			}
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x00045098 File Offset: 0x00043298
		private void Save()
		{
			object obj = this.GenerateSaveData();
			SavesSystem.Save<long>(this.realKey, (long)obj);
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x000450C0 File Offset: 0x000432C0
		private void Awake()
		{
			if (EXPManager.instance == null)
			{
				EXPManager.instance = this;
			}
			else
			{
				Debug.LogWarning("检测到多个ExpManager");
			}
			SavesSystem.OnSetFile += this.Load;
			SavesSystem.OnCollectSaveData += this.Save;
			RaidUtilities.OnNewRaid += this.OnNewRaid;
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0004511F File Offset: 0x0004331F
		private void Start()
		{
			this.Load();
			this.CacheExp();
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0004512D File Offset: 0x0004332D
		private void OnDestroy()
		{
			SavesSystem.OnSetFile -= this.Load;
			SavesSystem.OnCollectSaveData -= this.Save;
			RaidUtilities.OnNewRaid -= this.OnNewRaid;
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00045162 File Offset: 0x00043362
		private void OnNewRaid(RaidUtilities.RaidInfo info)
		{
			this.CacheExp();
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0004516C File Offset: 0x0004336C
		public int LevelFromExp(long exp)
		{
			for (int i = 0; i < this.levelExpDefinition.Count; i++)
			{
				long num = this.levelExpDefinition[i];
				if (exp < num)
				{
					return i - 1;
				}
			}
			return this.levelExpDefinition.Count - 1;
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x000451B4 File Offset: 0x000433B4
		[return: TupleElementNames(new string[]
		{
			"from",
			"to"
		})]
		public ValueTuple<long, long> GetLevelExpRange(int level)
		{
			int num = this.levelExpDefinition.Count - 1;
			if (level >= num)
			{
				List<long> list = this.levelExpDefinition;
				return new ValueTuple<long, long>(list[list.Count - 1], long.MaxValue);
			}
			long item = this.levelExpDefinition[level];
			long item2 = this.levelExpDefinition[level + 1];
			return new ValueTuple<long, long>(item, item2);
		}

		// Token: 0x04000DB5 RID: 3509
		private static EXPManager instance;

		// Token: 0x04000DB6 RID: 3510
		[SerializeField]
		private string levelChangeNotificationFormatKey = "UI_LevelChangeNotification";

		// Token: 0x04000DB7 RID: 3511
		[SerializeField]
		private List<long> levelExpDefinition;

		// Token: 0x04000DB8 RID: 3512
		[SerializeField]
		private long point;

		// Token: 0x04000DB9 RID: 3513
		public static Action<long> onExpChanged;

		// Token: 0x04000DBA RID: 3514
		public static Action<int, int> onLevelChanged;

		// Token: 0x04000DBB RID: 3515
		private long cachedExp;

		// Token: 0x04000DBC RID: 3516
		private const string prefixKey = "EXP";

		// Token: 0x04000DBD RID: 3517
		private const string key = "Value";
	}
}
