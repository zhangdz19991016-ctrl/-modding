using System;
using System.Collections.Generic;

namespace Duckov.PerkTrees.Behaviours
{
	// Token: 0x0200025F RID: 607
	public class UnlockFormula : PerkBehaviour
	{
		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06001301 RID: 4865 RVA: 0x00047ECB File Offset: 0x000460CB
		private IEnumerable<string> FormulasToUnlock
		{
			get
			{
				if (!CraftingFormulaCollection.Instance)
				{
					yield break;
				}
				string matchKey = base.Master.Master.ID + "/" + base.Master.name;
				foreach (CraftingFormula craftingFormula in CraftingFormulaCollection.Instance.Entries)
				{
					if (craftingFormula.requirePerk == matchKey)
					{
						yield return craftingFormula.id;
					}
				}
				IEnumerator<CraftingFormula> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x00047EDC File Offset: 0x000460DC
		protected override void OnUnlocked()
		{
			foreach (string formulaID in this.FormulasToUnlock)
			{
				CraftingManager.UnlockFormula(formulaID);
			}
		}
	}
}
