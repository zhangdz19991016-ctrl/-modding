using System;
using Duckov.Economy;
using ItemStatsSystem;

// Token: 0x020001AA RID: 426
[Serializable]
public struct DecomposeFormula
{
	// Token: 0x04000B05 RID: 2821
	[ItemTypeID]
	public int item;

	// Token: 0x04000B06 RID: 2822
	public bool valid;

	// Token: 0x04000B07 RID: 2823
	public Cost result;
}
