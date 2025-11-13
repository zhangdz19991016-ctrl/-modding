using System;
using Duckov.MiniGames.GoldMiner;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000347 RID: 839
	public class QuestTask_GoldMiner_Level : Task
	{
		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001D0A RID: 7434 RVA: 0x00068E9C File Offset: 0x0006709C
		// (set) Token: 0x06001D0B RID: 7435 RVA: 0x00068EA3 File Offset: 0x000670A3
		[LocalizationKey("Default")]
		private string descriptionKey
		{
			get
			{
				return "Task_GoldMiner_Level";
			}
			set
			{
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001D0C RID: 7436 RVA: 0x00068EA5 File Offset: 0x000670A5
		public override string Description
		{
			get
			{
				return this.descriptionKey.ToPlainText().Format(new
				{
					level = this.targetLevel
				});
			}
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x00068EC2 File Offset: 0x000670C2
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x00068ECF File Offset: 0x000670CF
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x00068ED1 File Offset: 0x000670D1
		protected override bool CheckFinished()
		{
			return GoldMiner.HighLevel + 1 >= this.targetLevel;
		}

		// Token: 0x04001426 RID: 5158
		[SerializeField]
		private int targetLevel;

		// Token: 0x04001427 RID: 5159
		private bool finished;
	}
}
