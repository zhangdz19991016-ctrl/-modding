using System;
using Duckov.Economy;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Rewards
{
	// Token: 0x02000361 RID: 865
	public class QuestReward_Money : Reward
	{
		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001E6C RID: 7788 RVA: 0x0006C0E3 File Offset: 0x0006A2E3
		public int Amount
		{
			get
			{
				return this.amount;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001E6D RID: 7789 RVA: 0x0006C0EB File Offset: 0x0006A2EB
		public override bool Claimed
		{
			get
			{
				return this.claimed;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001E6E RID: 7790 RVA: 0x0006C0F3 File Offset: 0x0006A2F3
		[SerializeField]
		private string descriptionFormatKey
		{
			get
			{
				return "Reward_Money";
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001E6F RID: 7791 RVA: 0x0006C0FA File Offset: 0x0006A2FA
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001E70 RID: 7792 RVA: 0x0006C107 File Offset: 0x0006A307
		public override bool AutoClaim
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001E71 RID: 7793 RVA: 0x0006C10A File Offset: 0x0006A30A
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.amount
				});
			}
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x0006C122 File Offset: 0x0006A322
		public override object GenerateSaveData()
		{
			return this.claimed;
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x0006C12F File Offset: 0x0006A32F
		public override void OnClaim()
		{
			if (this.Claimed)
			{
				return;
			}
			if (!EconomyManager.Add((long)this.amount))
			{
				return;
			}
			this.claimed = true;
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x0006C150 File Offset: 0x0006A350
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.claimed = flag;
			}
		}

		// Token: 0x040014D2 RID: 5330
		[Min(0f)]
		[SerializeField]
		private int amount;

		// Token: 0x040014D3 RID: 5331
		[SerializeField]
		private bool claimed;
	}
}
