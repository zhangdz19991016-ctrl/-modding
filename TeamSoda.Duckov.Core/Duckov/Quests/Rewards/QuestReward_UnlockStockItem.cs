using System;
using Duckov.Economy;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Rewards
{
	// Token: 0x02000362 RID: 866
	public class QuestReward_UnlockStockItem : Reward
	{
		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001E76 RID: 7798 RVA: 0x0006C17B File Offset: 0x0006A37B
		public int UnlockItem
		{
			get
			{
				return this.unlockItem;
			}
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x0006C183 File Offset: 0x0006A383
		private ItemMetaData GetItemMeta()
		{
			return ItemAssetsCollection.GetMetaData(this.unlockItem);
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001E78 RID: 7800 RVA: 0x0006C190 File Offset: 0x0006A390
		public override Sprite Icon
		{
			get
			{
				return ItemAssetsCollection.GetMetaData(this.unlockItem).icon;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001E79 RID: 7801 RVA: 0x0006C1A2 File Offset: 0x0006A3A2
		private string descriptionFormatKey
		{
			get
			{
				return "Reward_UnlockStockItem";
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001E7A RID: 7802 RVA: 0x0006C1A9 File Offset: 0x0006A3A9
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001E7B RID: 7803 RVA: 0x0006C1B8 File Offset: 0x0006A3B8
		private string ItemDisplayName
		{
			get
			{
				return ItemAssetsCollection.GetMetaData(this.unlockItem).DisplayName;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001E7C RID: 7804 RVA: 0x0006C1D8 File Offset: 0x0006A3D8
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.ItemDisplayName
				});
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001E7D RID: 7805 RVA: 0x0006C1F0 File Offset: 0x0006A3F0
		public override bool Claimed
		{
			get
			{
				return this.claimed;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001E7E RID: 7806 RVA: 0x0006C1F8 File Offset: 0x0006A3F8
		public override bool AutoClaim
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x0006C1FB File Offset: 0x0006A3FB
		public override object GenerateSaveData()
		{
			return this.claimed;
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x0006C208 File Offset: 0x0006A408
		public override void OnClaim()
		{
			EconomyManager.Unlock(this.unlockItem, true, true);
			this.claimed = true;
			base.ReportStatusChanged();
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x0006C224 File Offset: 0x0006A424
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.claimed = flag;
			}
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x0006C247 File Offset: 0x0006A447
		public override void NotifyReload(Quest questInstance)
		{
			if (questInstance.Complete)
			{
				EconomyManager.Unlock(this.unlockItem, true, true);
			}
		}

		// Token: 0x040014D4 RID: 5332
		[SerializeField]
		[ItemTypeID]
		private int unlockItem;

		// Token: 0x040014D5 RID: 5333
		private bool claimed;
	}
}
