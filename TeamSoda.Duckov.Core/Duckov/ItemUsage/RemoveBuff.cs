using System;
using ItemStatsSystem;
using UnityEngine.Serialization;

namespace Duckov.ItemUsage
{
	// Token: 0x02000373 RID: 883
	public class RemoveBuff : UsageBehavior
	{
		// Token: 0x06001ECA RID: 7882 RVA: 0x0006CCDC File Offset: 0x0006AEDC
		public override bool CanBeUsed(Item item, object user)
		{
			if (!item)
			{
				return false;
			}
			if (this.useDurability && item.Durability < (float)this.durabilityUsage)
			{
				return false;
			}
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			return !(characterMainControl == null) && characterMainControl.HasBuff(this.buffID);
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x0006CD2C File Offset: 0x0006AF2C
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (characterMainControl == null)
			{
				return;
			}
			if (!this.litmitRemoveLayerCount)
			{
				characterMainControl.RemoveBuff(this.buffID, false);
			}
			for (int i = 0; i < this.removeLayerCount; i++)
			{
				characterMainControl.RemoveBuff(this.buffID, this.litmitRemoveLayerCount);
			}
			if (this.useDurability && item.Durability > 0f)
			{
				item.Durability -= (float)this.durabilityUsage;
			}
		}

		// Token: 0x040014F8 RID: 5368
		public int buffID;

		// Token: 0x040014F9 RID: 5369
		[FormerlySerializedAs("removeOneLayer")]
		public bool litmitRemoveLayerCount;

		// Token: 0x040014FA RID: 5370
		public int removeLayerCount = 2;

		// Token: 0x040014FB RID: 5371
		public bool useDurability;

		// Token: 0x040014FC RID: 5372
		public int durabilityUsage = 1;
	}
}
