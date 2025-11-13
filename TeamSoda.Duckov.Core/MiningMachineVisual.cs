using System;
using System.Collections.Generic;
using Duckov.Bitcoins;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x02000183 RID: 387
public class MiningMachineVisual : MonoBehaviour
{
	// Token: 0x06000BB1 RID: 2993 RVA: 0x00031D24 File Offset: 0x0002FF24
	private void Update()
	{
		if (!this.inited && BitcoinMiner.Instance && BitcoinMiner.Instance.Item != null)
		{
			this.inited = true;
			this.minnerItem = BitcoinMiner.Instance.Item;
			this.minnerItem.onSlotContentChanged += this.OnSlotContentChanged;
			this.slots = this.minnerItem.Slots;
			this.OnSlotContentChanged(this.minnerItem, null);
			return;
		}
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x00031DA4 File Offset: 0x0002FFA4
	private void OnDestroy()
	{
		if (this.minnerItem)
		{
			this.minnerItem.onSlotContentChanged -= this.OnSlotContentChanged;
		}
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x00031DCC File Offset: 0x0002FFCC
	private void OnSlotContentChanged(Item minnerItem, Slot changedSlot)
	{
		for (int i = 0; i < this.slots.Count; i++)
		{
			if (!(this.cardsDisplay[i] == null))
			{
				Item content = this.slots[i].Content;
				MiningMachineCardDisplay.CardTypes cardType = MiningMachineCardDisplay.CardTypes.normal;
				if (content != null)
				{
					ItemSetting_GPU component = content.GetComponent<ItemSetting_GPU>();
					if (component)
					{
						cardType = component.cardType;
					}
				}
				this.cardsDisplay[i].SetVisualActive(content != null, cardType);
			}
		}
	}

	// Token: 0x04000A04 RID: 2564
	public List<MiningMachineCardDisplay> cardsDisplay;

	// Token: 0x04000A05 RID: 2565
	private bool inited;

	// Token: 0x04000A06 RID: 2566
	private SlotCollection slots;

	// Token: 0x04000A07 RID: 2567
	private Item minnerItem;
}
