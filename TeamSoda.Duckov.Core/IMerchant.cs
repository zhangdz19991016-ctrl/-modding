using System;
using ItemStatsSystem;

// Token: 0x02000202 RID: 514
public interface IMerchant
{
	// Token: 0x06000F28 RID: 3880
	int ConvertPrice(Item item, bool selling = false);
}
