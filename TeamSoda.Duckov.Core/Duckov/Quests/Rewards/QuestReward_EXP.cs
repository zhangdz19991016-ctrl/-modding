using System;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Rewards
{
	// Token: 0x02000360 RID: 864
	public class QuestReward_EXP : Reward
	{
		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001E62 RID: 7778 RVA: 0x0006C04C File Offset: 0x0006A24C
		public int Amount
		{
			get
			{
				return this.amount;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001E63 RID: 7779 RVA: 0x0006C054 File Offset: 0x0006A254
		public override bool Claimed
		{
			get
			{
				return this.claimed;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001E64 RID: 7780 RVA: 0x0006C05C File Offset: 0x0006A25C
		private string descriptionFormatKey
		{
			get
			{
				return "Reward_Exp";
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001E65 RID: 7781 RVA: 0x0006C063 File Offset: 0x0006A263
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001E66 RID: 7782 RVA: 0x0006C070 File Offset: 0x0006A270
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

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001E67 RID: 7783 RVA: 0x0006C088 File Offset: 0x0006A288
		public override bool AutoClaim
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x0006C08B File Offset: 0x0006A28B
		public override object GenerateSaveData()
		{
			return this.claimed;
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x0006C098 File Offset: 0x0006A298
		public override void OnClaim()
		{
			if (this.Claimed)
			{
				return;
			}
			if (!EXPManager.AddExp(this.amount))
			{
				return;
			}
			this.claimed = true;
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x0006C0B8 File Offset: 0x0006A2B8
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.claimed = flag;
			}
		}

		// Token: 0x040014D0 RID: 5328
		[SerializeField]
		private int amount;

		// Token: 0x040014D1 RID: 5329
		[SerializeField]
		private bool claimed;
	}
}
