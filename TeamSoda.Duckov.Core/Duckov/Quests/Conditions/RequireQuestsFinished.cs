using System;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x0200036E RID: 878
	public class RequireQuestsFinished : Condition
	{
		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001EB3 RID: 7859 RVA: 0x0006C685 File Offset: 0x0006A885
		public int[] RequiredQuestIDs
		{
			get
			{
				return this.requiredQuestIDs;
			}
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x0006C68D File Offset: 0x0006A88D
		public override bool Evaluate()
		{
			return QuestManager.AreQuestFinished(this.requiredQuestIDs);
		}

		// Token: 0x040014E5 RID: 5349
		[SerializeField]
		private int[] requiredQuestIDs;
	}
}
