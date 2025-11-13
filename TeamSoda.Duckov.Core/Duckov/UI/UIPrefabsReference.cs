using System;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x0200037C RID: 892
	[CreateAssetMenu]
	public class UIPrefabsReference : ScriptableObject
	{
		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001EFB RID: 7931 RVA: 0x0006D4B5 File Offset: 0x0006B6B5
		public ItemDisplay ItemDisplay
		{
			get
			{
				return this.itemDisplay;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001EFC RID: 7932 RVA: 0x0006D4BD File Offset: 0x0006B6BD
		public SlotIndicator SlotIndicator
		{
			get
			{
				return this.slotIndicator;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001EFD RID: 7933 RVA: 0x0006D4C5 File Offset: 0x0006B6C5
		public SlotDisplay SlotDisplay
		{
			get
			{
				return this.slotDisplay;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001EFE RID: 7934 RVA: 0x0006D4CD File Offset: 0x0006B6CD
		public InventoryEntry InventoryEntry
		{
			get
			{
				return this.inventoryEntry;
			}
		}

		// Token: 0x04001521 RID: 5409
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x04001522 RID: 5410
		[SerializeField]
		private SlotIndicator slotIndicator;

		// Token: 0x04001523 RID: 5411
		[SerializeField]
		private SlotDisplay slotDisplay;

		// Token: 0x04001524 RID: 5412
		[SerializeField]
		private InventoryEntry inventoryEntry;
	}
}
