using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x0200005B RID: 91
public class CharacterItemControl : MonoBehaviour
{
	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06000297 RID: 663 RVA: 0x0000BB93 File Offset: 0x00009D93
	private Inventory inventory
	{
		get
		{
			return this.characterMainControl.CharacterItem.Inventory;
		}
	}

	// Token: 0x06000298 RID: 664 RVA: 0x0000BBA8 File Offset: 0x00009DA8
	public bool PickupItem(Item item)
	{
		if (item == null)
		{
			return false;
		}
		if (this.inventory != null)
		{
			item.AgentUtilities.ReleaseActiveAgent();
			item.Detach();
			bool? flag = new bool?(this.characterMainControl.CharacterItem.TryPlug(item, true, null, 0));
			bool flag2;
			if (flag == null || !flag.Value)
			{
				if (this.characterMainControl.IsMainCharacter)
				{
					flag2 = ItemUtilities.SendToPlayerCharacterInventory(item, false);
				}
				else
				{
					flag2 = this.characterMainControl.CharacterItem.Inventory.AddAndMerge(item, 0);
				}
			}
			else
			{
				flag2 = true;
			}
			if (flag2)
			{
				return true;
			}
		}
		item.Drop(base.transform.position, true, Vector3.forward, 360f);
		return false;
	}

	// Token: 0x04000216 RID: 534
	public CharacterMainControl characterMainControl;
}
