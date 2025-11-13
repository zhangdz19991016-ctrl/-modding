using System;
using Duckov.PerkTrees;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x0200036C RID: 876
	public class RequirePerkUnlocked : Condition
	{
		// Token: 0x06001EAD RID: 7853 RVA: 0x0006C5AA File Offset: 0x0006A7AA
		public override bool Evaluate()
		{
			return this.GetUnlocked();
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x0006C5B4 File Offset: 0x0006A7B4
		private bool GetUnlocked()
		{
			if (this.perk)
			{
				return this.perk.Unlocked;
			}
			PerkTree perkTree = PerkTreeManager.GetPerkTree(this.perkTreeID);
			if (perkTree)
			{
				foreach (Perk perk in perkTree.perks)
				{
					if (perk.gameObject.name == this.perkObjectName)
					{
						this.perk = perk;
						return this.perk.Unlocked;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x040014E1 RID: 5345
		[SerializeField]
		private string perkTreeID;

		// Token: 0x040014E2 RID: 5346
		[SerializeField]
		private string perkObjectName;

		// Token: 0x040014E3 RID: 5347
		private Perk perk;
	}
}
