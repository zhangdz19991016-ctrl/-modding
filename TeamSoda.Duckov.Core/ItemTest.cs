using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020001B0 RID: 432
public class ItemTest : MonoBehaviour
{
	// Token: 0x06000CDD RID: 3293 RVA: 0x000364D5 File Offset: 0x000346D5
	public void DoInstantiate()
	{
		this.characterInstance = this.characterTemplate.CreateInstance();
		this.swordInstance = this.swordTemplate.CreateInstance();
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x000364FC File Offset: 0x000346FC
	public void EquipSword()
	{
		Item item;
		this.characterInstance.Slots["Weapon"].Plug(this.swordInstance, out item);
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x0003652C File Offset: 0x0003472C
	public void UequipSword()
	{
		this.characterInstance.Slots["Weapon"].Unplug();
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x00036549 File Offset: 0x00034749
	public void DestroyInstances()
	{
		if (this.characterInstance)
		{
			this.characterInstance.DestroyTreeImmediate();
		}
		if (this.swordInstance)
		{
			this.swordInstance.DestroyTreeImmediate();
		}
	}

	// Token: 0x04000B2D RID: 2861
	public Item characterTemplate;

	// Token: 0x04000B2E RID: 2862
	public Item swordTemplate;

	// Token: 0x04000B2F RID: 2863
	public Item characterInstance;

	// Token: 0x04000B30 RID: 2864
	public Item swordInstance;
}
