using System;
using Duckov.UI;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020001F9 RID: 505
public class DebugUISetup : MonoBehaviour
{
	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x06000EE2 RID: 3810 RVA: 0x0003BD51 File Offset: 0x00039F51
	private CharacterMainControl Character
	{
		get
		{
			return LevelManager.Instance.MainCharacter;
		}
	}

	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x0003BD5D File Offset: 0x00039F5D
	private Item CharacterItem
	{
		get
		{
			return this.Character.CharacterItem;
		}
	}

	// Token: 0x06000EE4 RID: 3812 RVA: 0x0003BD6A File Offset: 0x00039F6A
	public void Setup()
	{
		this.slotCollectionDisplay.Setup(this.CharacterItem, false);
		this.inventoryDisplay.Setup(this.CharacterItem.Inventory, null, null, false, null);
	}

	// Token: 0x04000C53 RID: 3155
	[SerializeField]
	private ItemSlotCollectionDisplay slotCollectionDisplay;

	// Token: 0x04000C54 RID: 3156
	[SerializeField]
	private InventoryDisplay inventoryDisplay;
}
