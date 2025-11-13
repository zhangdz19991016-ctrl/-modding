using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Economy;
using Duckov.Endowment;
using Duckov.Quests;
using Duckov.Rules.UI;
using Duckov.Scenes;
using Saves;
using UnityEngine;

namespace Duckov.Achievements
{
	// Token: 0x02000327 RID: 807
	public class AchievementManager : MonoBehaviour
	{
		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001AF8 RID: 6904 RVA: 0x00061E5F File Offset: 0x0006005F
		public static AchievementManager Instance
		{
			get
			{
				return GameManager.AchievementManager;
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001AF9 RID: 6905 RVA: 0x00061E66 File Offset: 0x00060066
		public static bool CanUnlockAchievement
		{
			get
			{
				return !DifficultySelection.CustomDifficultyMarker;
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001AFA RID: 6906 RVA: 0x00061E72 File Offset: 0x00060072
		public List<string> UnlockedAchievements
		{
			get
			{
				return this._unlockedAchievements;
			}
		}

		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x06001AFB RID: 6907 RVA: 0x00061E7C File Offset: 0x0006007C
		// (remove) Token: 0x06001AFC RID: 6908 RVA: 0x00061EB0 File Offset: 0x000600B0
		public static event Action<AchievementManager> OnAchievementDataLoaded;

		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x06001AFD RID: 6909 RVA: 0x00061EE4 File Offset: 0x000600E4
		// (remove) Token: 0x06001AFE RID: 6910 RVA: 0x00061F18 File Offset: 0x00060118
		public static event Action<string> OnAchievementUnlocked;

		// Token: 0x06001AFF RID: 6911 RVA: 0x00061F4B File Offset: 0x0006014B
		private void Awake()
		{
			this.Load();
			this.RegisterEvents();
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x00061F59 File Offset: 0x00060159
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x00061F61 File Offset: 0x00060161
		private void Start()
		{
			this.MakeSureMoneyAchievementsUnlocked();
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x00061F6C File Offset: 0x0006016C
		private void RegisterEvents()
		{
			Quest.onQuestCompleted += this.OnQuestCompleted;
			SavesCounter.OnKillCountChanged = (Action<string, int>)Delegate.Combine(SavesCounter.OnKillCountChanged, new Action<string, int>(this.OnKillCountChanged));
			MultiSceneCore.OnSetSceneVisited += this.OnSetSceneVisited;
			LevelManager.OnEvacuated += this.OnEvacuated;
			EconomyManager.OnMoneyChanged += this.OnMoneyChanged;
			EndowmentManager.OnEndowmentUnlock = (Action<EndowmentIndex>)Delegate.Combine(EndowmentManager.OnEndowmentUnlock, new Action<EndowmentIndex>(this.OnEndowmentUnlocked));
			EconomyManager.OnEconomyManagerLoaded += this.OnEconomyManagerLoaded;
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x00062010 File Offset: 0x00060210
		private void UnregisterEvents()
		{
			Quest.onQuestCompleted -= this.OnQuestCompleted;
			SavesCounter.OnKillCountChanged = (Action<string, int>)Delegate.Remove(SavesCounter.OnKillCountChanged, new Action<string, int>(this.OnKillCountChanged));
			MultiSceneCore.OnSetSceneVisited -= this.OnSetSceneVisited;
			LevelManager.OnEvacuated -= this.OnEvacuated;
			EconomyManager.OnMoneyChanged -= this.OnMoneyChanged;
			EndowmentManager.OnEndowmentUnlock = (Action<EndowmentIndex>)Delegate.Remove(EndowmentManager.OnEndowmentUnlock, new Action<EndowmentIndex>(this.OnEndowmentUnlocked));
			EconomyManager.OnEconomyManagerLoaded -= this.OnEconomyManagerLoaded;
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x000620B2 File Offset: 0x000602B2
		private void OnEconomyManagerLoaded()
		{
			this.MakeSureMoneyAchievementsUnlocked();
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x000620BA File Offset: 0x000602BA
		private void OnEndowmentUnlocked(EndowmentIndex index)
		{
			this.Unlock(string.Format("Endowmment_{0}", index));
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x000620D2 File Offset: 0x000602D2
		public static void UnlockEndowmentAchievement(EndowmentIndex index)
		{
			if (AchievementManager.Instance == null)
			{
				return;
			}
			AchievementManager.Instance.Unlock(string.Format("Endowmment_{0}", index));
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x000620FC File Offset: 0x000602FC
		private void OnMoneyChanged(long oldValue, long newValue)
		{
			if (oldValue < 10000L && newValue >= 10000L)
			{
				this.Unlock("Money_10K");
			}
			if (oldValue < 100000L && newValue >= 100000L)
			{
				this.Unlock("Money_100K");
			}
			if (oldValue < 1000000L && newValue >= 1000000L)
			{
				this.Unlock("Money_1M");
			}
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x00062160 File Offset: 0x00060360
		private void MakeSureMoneyAchievementsUnlocked()
		{
			long money = EconomyManager.Money;
			if (money >= 10000L)
			{
				this.Unlock("Money_10K");
			}
			if (money >= 100000L)
			{
				this.Unlock("Money_100K");
			}
			if (money >= 1000000L)
			{
				this.Unlock("Money_1M");
			}
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x000621B0 File Offset: 0x000603B0
		private void OnEvacuated(EvacuationInfo info)
		{
			string mainSceneID = MultiSceneCore.MainSceneID;
			if (!this.evacuateSceneIDs.Contains(mainSceneID))
			{
				return;
			}
			this.Unlock("Evacuate_" + mainSceneID);
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x000621E3 File Offset: 0x000603E3
		private void OnSetSceneVisited(string id)
		{
			if (!this.achievementSceneIDs.Contains(id))
			{
				return;
			}
			this.Unlock("Arrive_" + id);
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x00062208 File Offset: 0x00060408
		private void OnKillCountChanged(string key, int value)
		{
			this.Unlock("FirstBlood");
			if (AchievementDatabase.Instance == null)
			{
				return;
			}
			Debug.Log("COUNTING " + key);
			foreach (AchievementManager.KillCountAchievement killCountAchievement in this.KillCountAchivements)
			{
				if (killCountAchievement.key == key && value >= killCountAchievement.value)
				{
					this.Unlock(string.Format("Kill_{0}_{1}", key, killCountAchievement.value));
				}
			}
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x00062290 File Offset: 0x00060490
		private void OnQuestCompleted(Quest quest)
		{
			if (AchievementDatabase.Instance == null)
			{
				return;
			}
			string id = string.Format("Quest_{0}", quest.ID);
			AchievementDatabase.Achievement achievement;
			if (!AchievementDatabase.TryGetAchievementData(id, out achievement))
			{
				return;
			}
			this.Unlock(id);
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x000622D3 File Offset: 0x000604D3
		private void Save()
		{
			SavesSystem.SaveGlobal<List<string>>("Achievements", this.UnlockedAchievements);
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x000622E8 File Offset: 0x000604E8
		private void Load()
		{
			this.UnlockedAchievements.Clear();
			List<string> list = SavesSystem.LoadGlobal<List<string>>("Achievements", null);
			if (list != null)
			{
				this.UnlockedAchievements.AddRange(list);
			}
			Action<AchievementManager> onAchievementDataLoaded = AchievementManager.OnAchievementDataLoaded;
			if (onAchievementDataLoaded == null)
			{
				return;
			}
			onAchievementDataLoaded(this);
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x0006232C File Offset: 0x0006052C
		public void Unlock(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				Debug.LogError("Trying to unlock a empty acheivement.", this);
				return;
			}
			id = id.Trim();
			AchievementDatabase.Achievement achievement;
			if (!AchievementDatabase.TryGetAchievementData(id, out achievement))
			{
				Debug.LogError("Invalid acheivement id: " + id);
			}
			if (this.UnlockedAchievements.Contains(id))
			{
				return;
			}
			if (!AchievementManager.CanUnlockAchievement)
			{
				return;
			}
			this.UnlockedAchievements.Add(id);
			this.Save();
			Action<string> onAchievementUnlocked = AchievementManager.OnAchievementUnlocked;
			if (onAchievementUnlocked == null)
			{
				return;
			}
			onAchievementUnlocked(id);
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x000623A8 File Offset: 0x000605A8
		public static bool IsIDValid(string id)
		{
			return !(AchievementDatabase.Instance == null) && AchievementDatabase.Instance.IsIDValid(id);
		}

		// Token: 0x04001347 RID: 4935
		private List<string> _unlockedAchievements = new List<string>();

		// Token: 0x0400134A RID: 4938
		private readonly string[] evacuateSceneIDs = new string[]
		{
			"Level_GroundZero_Main"
		};

		// Token: 0x0400134B RID: 4939
		private readonly string[] achievementSceneIDs = new string[]
		{
			"Base",
			"Level_GroundZero_Main",
			"Level_HiddenWarehouse_Main",
			"Level_Farm_Main",
			"Level_JLab_Main",
			"Level_StormZone_Main"
		};

		// Token: 0x0400134C RID: 4940
		private readonly AchievementManager.KillCountAchievement[] KillCountAchivements = new AchievementManager.KillCountAchievement[]
		{
			new AchievementManager.KillCountAchievement("Cname_ShortEagle", 10),
			new AchievementManager.KillCountAchievement("Cname_ShortEagle", 1),
			new AchievementManager.KillCountAchievement("Cname_Speedy", 1),
			new AchievementManager.KillCountAchievement("Cname_StormBoss1", 1),
			new AchievementManager.KillCountAchievement("Cname_StormBoss2", 1),
			new AchievementManager.KillCountAchievement("Cname_StormBoss3", 1),
			new AchievementManager.KillCountAchievement("Cname_StormBoss4", 1),
			new AchievementManager.KillCountAchievement("Cname_StormBoss5", 1),
			new AchievementManager.KillCountAchievement("Cname_Boss_Sniper", 1),
			new AchievementManager.KillCountAchievement("Cname_Vida", 1),
			new AchievementManager.KillCountAchievement("Cname_Roadblock", 1),
			new AchievementManager.KillCountAchievement("Cname_SchoolBully", 1),
			new AchievementManager.KillCountAchievement("Cname_Boss_Fly", 1),
			new AchievementManager.KillCountAchievement("Cname_Boss_Arcade", 1),
			new AchievementManager.KillCountAchievement("Cname_UltraMan", 1),
			new AchievementManager.KillCountAchievement("Cname_LabTestObjective", 1)
		};

		// Token: 0x020005CC RID: 1484
		private struct KillCountAchievement
		{
			// Token: 0x06002971 RID: 10609 RVA: 0x00099974 File Offset: 0x00097B74
			public KillCountAchievement(string key, int value)
			{
				this.key = key;
				this.value = value;
			}

			// Token: 0x040020C8 RID: 8392
			public string key;

			// Token: 0x040020C9 RID: 8393
			public int value;
		}
	}
}
