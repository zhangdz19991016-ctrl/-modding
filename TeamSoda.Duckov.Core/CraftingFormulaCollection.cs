using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020001A7 RID: 423
[CreateAssetMenu]
public class CraftingFormulaCollection : ScriptableObject
{
	// Token: 0x1700023D RID: 573
	// (get) Token: 0x06000C93 RID: 3219 RVA: 0x0003574F File Offset: 0x0003394F
	public static CraftingFormulaCollection Instance
	{
		get
		{
			return GameplayDataSettings.CraftingFormulas;
		}
	}

	// Token: 0x1700023E RID: 574
	// (get) Token: 0x06000C94 RID: 3220 RVA: 0x00035756 File Offset: 0x00033956
	public ReadOnlyCollection<CraftingFormula> Entries
	{
		get
		{
			if (this._entries_ReadOnly == null)
			{
				this._entries_ReadOnly = new ReadOnlyCollection<CraftingFormula>(this.list);
			}
			return this._entries_ReadOnly;
		}
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x00035778 File Offset: 0x00033978
	public static bool TryGetFormula(string id, out CraftingFormula formula)
	{
		if (!(CraftingFormulaCollection.Instance == null))
		{
			CraftingFormula craftingFormula = CraftingFormulaCollection.Instance.list.FirstOrDefault((CraftingFormula e) => e.id == id);
			if (!string.IsNullOrEmpty(craftingFormula.id))
			{
				formula = craftingFormula;
				return true;
			}
		}
		formula = default(CraftingFormula);
		return false;
	}

	// Token: 0x04000AFC RID: 2812
	[SerializeField]
	private List<CraftingFormula> list;

	// Token: 0x04000AFD RID: 2813
	private ReadOnlyCollection<CraftingFormula> _entries_ReadOnly;
}
