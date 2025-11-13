using System;
using Duckov.MiniGames.BubblePoppers;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000346 RID: 838
	public class QuestTask_BubblePopper_Level : Task
	{
		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001D03 RID: 7427 RVA: 0x00068E4D File Offset: 0x0006704D
		// (set) Token: 0x06001D04 RID: 7428 RVA: 0x00068E54 File Offset: 0x00067054
		[LocalizationKey("Default")]
		private string descriptionKey
		{
			get
			{
				return "Task_BubblePopper_Level";
			}
			set
			{
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001D05 RID: 7429 RVA: 0x00068E56 File Offset: 0x00067056
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

		// Token: 0x06001D06 RID: 7430 RVA: 0x00068E73 File Offset: 0x00067073
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x00068E80 File Offset: 0x00067080
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x00068E82 File Offset: 0x00067082
		protected override bool CheckFinished()
		{
			return BubblePopper.HighLevel >= this.targetLevel;
		}

		// Token: 0x04001424 RID: 5156
		[SerializeField]
		private int targetLevel;

		// Token: 0x04001425 RID: 5157
		private bool finished;
	}
}
