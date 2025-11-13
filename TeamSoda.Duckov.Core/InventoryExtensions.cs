using System;
using ItemStatsSystem;

// Token: 0x020000EB RID: 235
public static class InventoryExtensions
{
	// Token: 0x060007DB RID: 2011 RVA: 0x000236F0 File Offset: 0x000218F0
	private static void Sort(this Inventory inventory, Comparison<Item> comparison)
	{
		inventory.Content.Sort(comparison);
	}
}
