using System;
using System.Collections.Generic;
using Duckov.Scenes;
using Duckov.Utilities;
using Eflatun.SceneReference;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000359 RID: 857
	public class QuestTask_KillCount : Task
	{
		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001DDE RID: 7646 RVA: 0x0006AF4B File Offset: 0x0006914B
		// (set) Token: 0x06001DDF RID: 7647 RVA: 0x0006AF52 File Offset: 0x00069152
		[LocalizationKey("TasksAndRewards")]
		private string defaultEnemyNameKey
		{
			get
			{
				return "Task_Desc_AnyEnemy";
			}
			set
			{
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001DE0 RID: 7648 RVA: 0x0006AF54 File Offset: 0x00069154
		// (set) Token: 0x06001DE1 RID: 7649 RVA: 0x0006AF5B File Offset: 0x0006915B
		[LocalizationKey("TasksAndRewards")]
		private string defaultWeaponNameKey
		{
			get
			{
				return "Task_Desc_AnyWeapon";
			}
			set
			{
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001DE2 RID: 7650 RVA: 0x0006AF60 File Offset: 0x00069160
		private string weaponName
		{
			get
			{
				if (this.withWeapon)
				{
					return ItemAssetsCollection.GetMetaData(this.weaponTypeID).DisplayName;
				}
				return this.defaultWeaponNameKey.ToPlainText();
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001DE3 RID: 7651 RVA: 0x0006AF94 File Offset: 0x00069194
		private string enemyName
		{
			get
			{
				if (this.requireEnemyType == null)
				{
					return this.defaultEnemyNameKey.ToPlainText();
				}
				return this.requireEnemyType.DisplayName;
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001DE4 RID: 7652 RVA: 0x0006AFBB File Offset: 0x000691BB
		// (set) Token: 0x06001DE5 RID: 7653 RVA: 0x0006AFC2 File Offset: 0x000691C2
		[LocalizationKey("TasksAndRewards")]
		private string descriptionFormatKey
		{
			get
			{
				return "Task_KillCount";
			}
			set
			{
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001DE6 RID: 7654 RVA: 0x0006AFC4 File Offset: 0x000691C4
		// (set) Token: 0x06001DE7 RID: 7655 RVA: 0x0006AFCB File Offset: 0x000691CB
		[LocalizationKey("TasksAndRewards")]
		private string withWeaponDescriptionFormatKey
		{
			get
			{
				return "Task_Desc_WithWeapon";
			}
			set
			{
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001DE8 RID: 7656 RVA: 0x0006AFCD File Offset: 0x000691CD
		// (set) Token: 0x06001DE9 RID: 7657 RVA: 0x0006AFD4 File Offset: 0x000691D4
		[LocalizationKey("TasksAndRewards")]
		private string requireSceneDescriptionFormatKey
		{
			get
			{
				return "Task_Desc_RequireScene";
			}
			set
			{
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001DEA RID: 7658 RVA: 0x0006AFD6 File Offset: 0x000691D6
		// (set) Token: 0x06001DEB RID: 7659 RVA: 0x0006AFDD File Offset: 0x000691DD
		[LocalizationKey("TasksAndRewards")]
		private string RequireHeadShotDescriptionKey
		{
			get
			{
				return "Task_Desc_RequireHeadShot";
			}
			set
			{
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001DEC RID: 7660 RVA: 0x0006AFDF File Offset: 0x000691DF
		// (set) Token: 0x06001DED RID: 7661 RVA: 0x0006AFE6 File Offset: 0x000691E6
		[LocalizationKey("TasksAndRewards")]
		private string WithoutHeadShotDescriptionKey
		{
			get
			{
				return "Task_Desc_WithoutHeadShot";
			}
			set
			{
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001DEE RID: 7662 RVA: 0x0006AFE8 File Offset: 0x000691E8
		// (set) Token: 0x06001DEF RID: 7663 RVA: 0x0006AFEF File Offset: 0x000691EF
		[LocalizationKey("TasksAndRewards")]
		private string RequireBuffDescriptionFormatKey
		{
			get
			{
				return "Task_Desc_WithBuff";
			}
			set
			{
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001DF0 RID: 7664 RVA: 0x0006AFF1 File Offset: 0x000691F1
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001DF1 RID: 7665 RVA: 0x0006B000 File Offset: 0x00069200
		public override string[] ExtraDescriptsions
		{
			get
			{
				List<string> list = new List<string>();
				if (this.withWeapon)
				{
					list.Add(this.WithWeaponDescription);
				}
				if (!string.IsNullOrEmpty(this.requireSceneID))
				{
					list.Add(this.RequireSceneDescription);
				}
				if (this.requireHeadShot)
				{
					list.Add(this.RequireHeadShotDescription);
				}
				if (this.withoutHeadShot)
				{
					list.Add(this.WithoutHeadShotDescription);
				}
				if (this.requireBuff)
				{
					list.Add(this.RequireBuffDescription);
				}
				return list.ToArray();
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001DF2 RID: 7666 RVA: 0x0006B082 File Offset: 0x00069282
		private string WithWeaponDescription
		{
			get
			{
				return this.withWeaponDescriptionFormatKey.ToPlainText().Format(new
				{
					this.weaponName
				});
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001DF3 RID: 7667 RVA: 0x0006B09F File Offset: 0x0006929F
		private string RequireSceneDescription
		{
			get
			{
				return this.requireSceneDescriptionFormatKey.ToPlainText().Format(new
				{
					this.requireSceneName
				});
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001DF4 RID: 7668 RVA: 0x0006B0BC File Offset: 0x000692BC
		private string RequireHeadShotDescription
		{
			get
			{
				return this.RequireHeadShotDescriptionKey.ToPlainText();
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001DF5 RID: 7669 RVA: 0x0006B0C9 File Offset: 0x000692C9
		private string WithoutHeadShotDescription
		{
			get
			{
				return this.WithoutHeadShotDescriptionKey.ToPlainText();
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001DF6 RID: 7670 RVA: 0x0006B0D8 File Offset: 0x000692D8
		private string RequireBuffDescription
		{
			get
			{
				string buffDisplayName = GameplayDataSettings.Buffs.GetBuffDisplayName(this.requireBuffID);
				return this.RequireBuffDescriptionFormatKey.ToPlainText().Format(new
				{
					buffName = buffDisplayName
				});
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001DF7 RID: 7671 RVA: 0x0006B10C File Offset: 0x0006930C
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.weaponName,
					this.enemyName,
					this.requireAmount,
					this.amount,
					this.requireSceneName
				});
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001DF8 RID: 7672 RVA: 0x0006B13C File Offset: 0x0006933C
		public SceneInfoEntry RequireSceneInfo
		{
			get
			{
				return SceneInfoCollection.GetSceneInfo(this.requireSceneID);
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001DF9 RID: 7673 RVA: 0x0006B14C File Offset: 0x0006934C
		public SceneReference RequireScene
		{
			get
			{
				SceneInfoEntry requireSceneInfo = this.RequireSceneInfo;
				if (requireSceneInfo == null)
				{
					return null;
				}
				return requireSceneInfo.SceneReference;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001DFA RID: 7674 RVA: 0x0006B16B File Offset: 0x0006936B
		public string requireSceneName
		{
			get
			{
				if (string.IsNullOrEmpty(this.requireSceneID))
				{
					return "Task_Desc_AnyScene".ToPlainText();
				}
				return this.RequireSceneInfo.DisplayName;
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001DFB RID: 7675 RVA: 0x0006B190 File Offset: 0x00069390
		public bool SceneRequirementSatisfied
		{
			get
			{
				if (string.IsNullOrEmpty(this.requireSceneID))
				{
					return true;
				}
				SceneReference requireScene = this.RequireScene;
				return requireScene == null || requireScene.UnsafeReason == SceneReferenceUnsafeReason.Empty || requireScene.UnsafeReason != SceneReferenceUnsafeReason.None || requireScene.LoadedScene.isLoaded;
			}
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x0006B1DB File Offset: 0x000693DB
		private void OnEnable()
		{
			Health.OnDead += this.Health_OnDead;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x0006B1FF File Offset: 0x000693FF
		private void OnDisable()
		{
			Health.OnDead -= this.Health_OnDead;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x0006B223 File Offset: 0x00069423
		private void OnLevelInitialized()
		{
			if (this.resetOnLevelInitialized)
			{
				this.amount = 0;
			}
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x0006B234 File Offset: 0x00069434
		private void Health_OnDead(Health health, DamageInfo info)
		{
			if (health.team == Teams.player)
			{
				return;
			}
			bool flag = false;
			CharacterMainControl fromCharacter = info.fromCharacter;
			if (fromCharacter != null && info.fromCharacter.IsMainCharacter())
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			if (this.withWeapon && info.fromWeaponItemID != this.weaponTypeID)
			{
				return;
			}
			if (!this.SceneRequirementSatisfied)
			{
				return;
			}
			if (this.requireHeadShot && info.crit <= 0)
			{
				return;
			}
			if (this.withoutHeadShot && info.crit > 0)
			{
				return;
			}
			if (this.requireBuff && !fromCharacter.HasBuff(this.requireBuffID))
			{
				return;
			}
			if (this.requireEnemyType != null)
			{
				CharacterMainControl characterMainControl = health.TryGetCharacter();
				if (characterMainControl == null)
				{
					return;
				}
				CharacterRandomPreset characterPreset = characterMainControl.characterPreset;
				if (characterPreset == null)
				{
					return;
				}
				if (characterPreset.nameKey != this.requireEnemyType.nameKey)
				{
					return;
				}
			}
			this.AddCount();
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x0006B319 File Offset: 0x00069519
		private void AddCount()
		{
			if (this.amount < this.requireAmount)
			{
				this.amount++;
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x0006B33D File Offset: 0x0006953D
		public override object GenerateSaveData()
		{
			return this.amount;
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x0006B34A File Offset: 0x0006954A
		protected override bool CheckFinished()
		{
			return this.amount >= this.requireAmount;
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x0006B360 File Offset: 0x00069560
		public override void SetupSaveData(object data)
		{
			if (data is int)
			{
				int num = (int)data;
				this.amount = num;
			}
		}

		// Token: 0x040014A8 RID: 5288
		[SerializeField]
		private int requireAmount = 1;

		// Token: 0x040014A9 RID: 5289
		[SerializeField]
		private bool resetOnLevelInitialized;

		// Token: 0x040014AA RID: 5290
		[SerializeField]
		private int amount;

		// Token: 0x040014AB RID: 5291
		[SerializeField]
		private bool withWeapon;

		// Token: 0x040014AC RID: 5292
		[SerializeField]
		[ItemTypeID]
		private int weaponTypeID;

		// Token: 0x040014AD RID: 5293
		[SerializeField]
		private bool requireHeadShot;

		// Token: 0x040014AE RID: 5294
		[SerializeField]
		private bool withoutHeadShot;

		// Token: 0x040014AF RID: 5295
		[SerializeField]
		private bool requireBuff;

		// Token: 0x040014B0 RID: 5296
		[SerializeField]
		private int requireBuffID;

		// Token: 0x040014B1 RID: 5297
		[SerializeField]
		private CharacterRandomPreset requireEnemyType;

		// Token: 0x040014B2 RID: 5298
		[SceneID]
		[SerializeField]
		private string requireSceneID;
	}
}
