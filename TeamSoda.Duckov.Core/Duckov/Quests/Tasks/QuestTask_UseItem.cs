using System;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x0200035E RID: 862
	public class QuestTask_UseItem : Task
	{
		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001E38 RID: 7736 RVA: 0x0006B9F6 File Offset: 0x00069BF6
		private ItemMetaData CachedMeta
		{
			get
			{
				if (this._cachedMeta == null)
				{
					this._cachedMeta = new ItemMetaData?(ItemAssetsCollection.GetMetaData(this.itemTypeID));
				}
				return this._cachedMeta.Value;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001E39 RID: 7737 RVA: 0x0006BA26 File Offset: 0x00069C26
		private string descriptionFormatKey
		{
			get
			{
				return "Task_UseItem";
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001E3A RID: 7738 RVA: 0x0006BA2D File Offset: 0x00069C2D
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001E3B RID: 7739 RVA: 0x0006BA3C File Offset: 0x00069C3C
		private string ItemDisplayName
		{
			get
			{
				return this.CachedMeta.DisplayName;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001E3C RID: 7740 RVA: 0x0006BA57 File Offset: 0x00069C57
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.ItemDisplayName,
					this.amount,
					this.requireAmount
				});
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001E3D RID: 7741 RVA: 0x0006BA7B File Offset: 0x00069C7B
		public override Sprite Icon
		{
			get
			{
				return this.CachedMeta.icon;
			}
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x0006BA88 File Offset: 0x00069C88
		private void OnEnable()
		{
			Item.onUseStatic += this.OnItemUsed;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x0006BAAC File Offset: 0x00069CAC
		private void OnDisable()
		{
			Item.onUseStatic -= this.OnItemUsed;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x0006BAD0 File Offset: 0x00069CD0
		private void OnLevelInitialized()
		{
			if (this.resetOnLevelInitialized)
			{
				this.amount = 0;
			}
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x0006BAE1 File Offset: 0x00069CE1
		private void OnItemUsed(Item item, object user)
		{
			if (!LevelManager.Instance)
			{
				return;
			}
			if (user as CharacterMainControl == LevelManager.Instance.MainCharacter && item.TypeID == this.itemTypeID)
			{
				this.AddCount();
			}
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x0006BB1B File Offset: 0x00069D1B
		private void AddCount()
		{
			if (this.amount < this.requireAmount)
			{
				this.amount++;
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x0006BB3F File Offset: 0x00069D3F
		public override object GenerateSaveData()
		{
			return this.amount;
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x0006BB4C File Offset: 0x00069D4C
		protected override bool CheckFinished()
		{
			return this.amount >= this.requireAmount;
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x0006BB60 File Offset: 0x00069D60
		public override void SetupSaveData(object data)
		{
			if (data is int)
			{
				int num = (int)data;
				this.amount = num;
			}
		}

		// Token: 0x040014C5 RID: 5317
		[SerializeField]
		private int requireAmount = 1;

		// Token: 0x040014C6 RID: 5318
		[ItemTypeID]
		[SerializeField]
		private int itemTypeID;

		// Token: 0x040014C7 RID: 5319
		[SerializeField]
		private bool resetOnLevelInitialized;

		// Token: 0x040014C8 RID: 5320
		[SerializeField]
		private int amount;

		// Token: 0x040014C9 RID: 5321
		private ItemMetaData? _cachedMeta;
	}
}
