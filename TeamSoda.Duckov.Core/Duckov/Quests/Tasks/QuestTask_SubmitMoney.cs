using System;
using Duckov.Economy;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x0200035B RID: 859
	public class QuestTask_SubmitMoney : Task
	{
		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001E16 RID: 7702 RVA: 0x0006B59A File Offset: 0x0006979A
		public string DescriptionFormat
		{
			get
			{
				return this.decriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001E17 RID: 7703 RVA: 0x0006B5A7 File Offset: 0x000697A7
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.money
				});
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001E18 RID: 7704 RVA: 0x0006B5BF File Offset: 0x000697BF
		public override bool Interactable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001E19 RID: 7705 RVA: 0x0006B5C2 File Offset: 0x000697C2
		public override bool PossibleValidInteraction
		{
			get
			{
				return this.CheckMoneyEnough();
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001E1A RID: 7706 RVA: 0x0006B5CA File Offset: 0x000697CA
		public override string InteractText
		{
			get
			{
				return this.interactTextKey.ToPlainText();
			}
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x0006B5D8 File Offset: 0x000697D8
		public override void Interact()
		{
			Cost cost = new Cost((long)this.money);
			if (cost.Pay(true, true))
			{
				this.submitted = true;
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x0006B60B File Offset: 0x0006980B
		private bool CheckMoneyEnough()
		{
			return EconomyManager.Money >= (long)this.money;
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x0006B61E File Offset: 0x0006981E
		public override object GenerateSaveData()
		{
			return this.submitted;
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x0006B62C File Offset: 0x0006982C
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.submitted = flag;
			}
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x0006B64F File Offset: 0x0006984F
		protected override bool CheckFinished()
		{
			return this.submitted;
		}

		// Token: 0x040014B8 RID: 5304
		[SerializeField]
		private int money;

		// Token: 0x040014B9 RID: 5305
		[SerializeField]
		[LocalizationKey("Default")]
		private string decriptionFormatKey = "QuestTask_SubmitMoney";

		// Token: 0x040014BA RID: 5306
		[SerializeField]
		[LocalizationKey("Default")]
		private string interactTextKey = "QuestTask_SubmitMoney_Interact";

		// Token: 0x040014BB RID: 5307
		private bool submitted;
	}
}
