using System;
using Duckov.MiniGames.SnakeForces;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000348 RID: 840
	public class QuestTask_Snake_Score : Task
	{
		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001D11 RID: 7441 RVA: 0x00068EED File Offset: 0x000670ED
		// (set) Token: 0x06001D12 RID: 7442 RVA: 0x00068EF4 File Offset: 0x000670F4
		[LocalizationKey("Default")]
		private string descriptionKey
		{
			get
			{
				return "Task_Snake_Score";
			}
			set
			{
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001D13 RID: 7443 RVA: 0x00068EF6 File Offset: 0x000670F6
		public override string Description
		{
			get
			{
				return this.descriptionKey.ToPlainText().Format(new
				{
					score = this.targetScore
				});
			}
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x00068F13 File Offset: 0x00067113
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x00068F20 File Offset: 0x00067120
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x00068F22 File Offset: 0x00067122
		protected override bool CheckFinished()
		{
			return SnakeForce.HighScore >= this.targetScore;
		}

		// Token: 0x04001428 RID: 5160
		[SerializeField]
		private int targetScore;

		// Token: 0x04001429 RID: 5161
		private bool finished;
	}
}
