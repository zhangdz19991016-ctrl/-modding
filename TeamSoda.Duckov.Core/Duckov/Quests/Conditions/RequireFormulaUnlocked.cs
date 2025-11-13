using System;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x02000369 RID: 873
	public class RequireFormulaUnlocked : Condition
	{
		// Token: 0x06001EA5 RID: 7845 RVA: 0x0006C529 File Offset: 0x0006A729
		public override bool Evaluate()
		{
			return CraftingManager.IsFormulaUnlocked(this.formulaID);
		}

		// Token: 0x040014DD RID: 5341
		[ItemTypeID]
		[SerializeField]
		private int itemID;

		// Token: 0x040014DE RID: 5342
		[SerializeField]
		private string formulaID;

		// Token: 0x040014DF RID: 5343
		public Item setFromItem;
	}
}
