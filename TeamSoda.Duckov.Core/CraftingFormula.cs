using System;
using Duckov.Economy;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020001A6 RID: 422
[Serializable]
public struct CraftingFormula
{
	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06000C92 RID: 3218 RVA: 0x0003573F File Offset: 0x0003393F
	public bool IDValid
	{
		get
		{
			return !string.IsNullOrEmpty(this.id);
		}
	}

	// Token: 0x04000AF4 RID: 2804
	public string id;

	// Token: 0x04000AF5 RID: 2805
	public CraftingFormula.ItemEntry result;

	// Token: 0x04000AF6 RID: 2806
	public string[] tags;

	// Token: 0x04000AF7 RID: 2807
	[SerializeField]
	public Cost cost;

	// Token: 0x04000AF8 RID: 2808
	public bool unlockByDefault;

	// Token: 0x04000AF9 RID: 2809
	public bool lockInDemo;

	// Token: 0x04000AFA RID: 2810
	public string requirePerk;

	// Token: 0x04000AFB RID: 2811
	public bool hideInIndex;

	// Token: 0x020004C8 RID: 1224
	[Serializable]
	public struct ItemEntry
	{
		// Token: 0x04001CE1 RID: 7393
		[ItemTypeID]
		public int id;

		// Token: 0x04001CE2 RID: 7394
		public int amount;
	}
}
