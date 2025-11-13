using System;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x0200036D RID: 877
	public class RequireQuestsActive : Condition
	{
		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001EB0 RID: 7856 RVA: 0x0006C668 File Offset: 0x0006A868
		public int[] RequiredQuestIDs
		{
			get
			{
				return this.requiredQuestIDs;
			}
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x0006C670 File Offset: 0x0006A870
		public override bool Evaluate()
		{
			return QuestManager.AreQuestsActive(this.requiredQuestIDs);
		}

		// Token: 0x040014E4 RID: 5348
		[SerializeField]
		private int[] requiredQuestIDs;
	}
}
