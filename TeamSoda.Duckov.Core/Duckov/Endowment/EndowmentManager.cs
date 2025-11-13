using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Duckov.Achievements;
using Saves;
using UnityEngine;

namespace Duckov.Endowment
{
	// Token: 0x020002F9 RID: 761
	public class EndowmentManager : MonoBehaviour
	{
		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x060018E0 RID: 6368 RVA: 0x0005B189 File Offset: 0x00059389
		// (set) Token: 0x060018E1 RID: 6369 RVA: 0x0005B190 File Offset: 0x00059390
		private static EndowmentManager _instance { get; set; }

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x060018E2 RID: 6370 RVA: 0x0005B198 File Offset: 0x00059398
		public static EndowmentManager Instance
		{
			get
			{
				if (EndowmentManager._instance == null)
				{
					GameManager instance = GameManager.Instance;
				}
				return EndowmentManager._instance;
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x060018E3 RID: 6371 RVA: 0x0005B1B2 File Offset: 0x000593B2
		// (set) Token: 0x060018E4 RID: 6372 RVA: 0x0005B1BE File Offset: 0x000593BE
		public static EndowmentIndex SelectedIndex
		{
			get
			{
				return SavesSystem.Load<EndowmentIndex>("Endowment_SelectedIndex");
			}
			private set
			{
				SavesSystem.Save<EndowmentIndex>("Endowment_SelectedIndex", value);
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x060018E5 RID: 6373 RVA: 0x0005B1CB File Offset: 0x000593CB
		public ReadOnlyCollection<EndowmentEntry> Entries
		{
			get
			{
				if (this._entries_ReadOnly == null)
				{
					this._entries_ReadOnly = new ReadOnlyCollection<EndowmentEntry>(this.entries);
				}
				return this._entries_ReadOnly;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x060018E6 RID: 6374 RVA: 0x0005B1EC File Offset: 0x000593EC
		public static EndowmentEntry Current
		{
			get
			{
				if (EndowmentManager._instance == null)
				{
					return null;
				}
				return EndowmentManager._instance.entries.Find((EndowmentEntry e) => e != null && e.Index == EndowmentManager.SelectedIndex);
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x060018E7 RID: 6375 RVA: 0x0005B22B File Offset: 0x0005942B
		public static EndowmentIndex CurrentIndex
		{
			get
			{
				if (EndowmentManager.Current == null)
				{
					return EndowmentIndex.None;
				}
				return EndowmentManager.Current.Index;
			}
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x0005B248 File Offset: 0x00059448
		private EndowmentEntry GetEntry(EndowmentIndex index)
		{
			return this.entries.Find((EndowmentEntry e) => e != null && e.Index == index);
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x0005B279 File Offset: 0x00059479
		private static string GetUnlockKey(EndowmentIndex index)
		{
			return string.Format("Endowment_Unlock_R_{0}", index);
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x0005B28B File Offset: 0x0005948B
		public static bool GetEndowmentUnlocked(EndowmentIndex index)
		{
			if (EndowmentManager.Instance != null)
			{
				if (EndowmentManager.Instance.GetEntry(index).UnlockedByDefault)
				{
					return true;
				}
			}
			else
			{
				Debug.LogError("Endowment Manager 不存在。");
			}
			return SavesSystem.LoadGlobal<bool>(EndowmentManager.GetUnlockKey(index), false);
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x0005B2C4 File Offset: 0x000594C4
		private static void SetEndowmentUnlocked(EndowmentIndex index, bool value = true)
		{
			SavesSystem.SaveGlobal<bool>(EndowmentManager.GetUnlockKey(index), value);
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x0005B2D4 File Offset: 0x000594D4
		public static bool UnlockEndowment(EndowmentIndex index)
		{
			try
			{
				Action<EndowmentIndex> onEndowmentUnlock = EndowmentManager.OnEndowmentUnlock;
				if (onEndowmentUnlock != null)
				{
					onEndowmentUnlock(index);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			if (EndowmentManager.GetEndowmentUnlocked(index))
			{
				Debug.Log("尝试解锁天赋，但天赋已经解锁");
				return false;
			}
			EndowmentManager.SetEndowmentUnlocked(index, true);
			return true;
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x0005B328 File Offset: 0x00059528
		private void Awake()
		{
			if (EndowmentManager._instance != null)
			{
				Debug.LogError("检测到多个Endowment Manager");
				return;
			}
			EndowmentManager._instance = this;
			if (LevelManager.LevelInited)
			{
				this.ApplyCurrentEndowment();
			}
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x0005B366 File Offset: 0x00059566
		private void OnDestroy()
		{
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x0005B379 File Offset: 0x00059579
		private void OnLevelInitialized()
		{
			this.ApplyCurrentEndowment();
			this.MakeSureEndowmentAchievementsUnlocked();
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x0005B388 File Offset: 0x00059588
		private void MakeSureEndowmentAchievementsUnlocked()
		{
			for (int i = 0; i < 5; i++)
			{
				EndowmentIndex index = (EndowmentIndex)i;
				EndowmentEntry entry = EndowmentManager.Instance.GetEntry(index);
				if (!(entry == null) && !entry.UnlockedByDefault && EndowmentManager.GetEndowmentUnlocked(index))
				{
					AchievementManager.UnlockEndowmentAchievement(index);
				}
			}
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x0005B3D0 File Offset: 0x000595D0
		private void ApplyCurrentEndowment()
		{
			if (!LevelManager.LevelInited)
			{
				return;
			}
			foreach (EndowmentEntry endowmentEntry in this.entries)
			{
				if (!(endowmentEntry == null))
				{
					endowmentEntry.Deactivate();
				}
			}
			EndowmentEntry endowmentEntry2 = EndowmentManager.Current;
			if (endowmentEntry2 == null)
			{
				return;
			}
			endowmentEntry2.Activate();
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x0005B44C File Offset: 0x0005964C
		internal void SelectIndex(EndowmentIndex index)
		{
			EndowmentManager.SelectedIndex = index;
			this.ApplyCurrentEndowment();
			Action<EndowmentIndex> onEndowmentChanged = EndowmentManager.OnEndowmentChanged;
			if (onEndowmentChanged == null)
			{
				return;
			}
			onEndowmentChanged(index);
		}

		// Token: 0x04001214 RID: 4628
		private const string SaveKey = "Endowment_SelectedIndex";

		// Token: 0x04001215 RID: 4629
		public static Action<EndowmentIndex> OnEndowmentChanged;

		// Token: 0x04001216 RID: 4630
		public static Action<EndowmentIndex> OnEndowmentUnlock;

		// Token: 0x04001217 RID: 4631
		[SerializeField]
		private List<EndowmentEntry> entries = new List<EndowmentEntry>();

		// Token: 0x04001218 RID: 4632
		private ReadOnlyCollection<EndowmentEntry> _entries_ReadOnly;
	}
}
