using System;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public class FishingRod : MonoBehaviour
{
	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06000744 RID: 1860 RVA: 0x00020A8F File Offset: 0x0001EC8F
	private ItemAgent selfAgent
	{
		get
		{
			if (this._selfAgent == null)
			{
				this._selfAgent = base.GetComponent<ItemAgent>();
			}
			return this._selfAgent;
		}
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06000745 RID: 1861 RVA: 0x00020AB1 File Offset: 0x0001ECB1
	public Item Bait
	{
		get
		{
			if (this.baitSlot == null)
			{
				this.baitSlot = this.selfAgent.Item.Slots.GetSlot("Bait");
			}
			if (this.baitSlot != null)
			{
				return this.baitSlot.Content;
			}
			return null;
		}
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x00020AF0 File Offset: 0x0001ECF0
	public bool UseBait()
	{
		Item bait = this.Bait;
		if (bait == null)
		{
			return false;
		}
		if (bait.Stackable)
		{
			bait.StackCount--;
		}
		else
		{
			bait.DestroyTree();
		}
		return true;
	}

	// Token: 0x040006EF RID: 1775
	[SerializeField]
	private ItemAgent _selfAgent;

	// Token: 0x040006F0 RID: 1776
	private Slot baitSlot;

	// Token: 0x040006F1 RID: 1777
	public Transform lineStart;
}
