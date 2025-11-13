using System;
using Duckov.Economy;
using Duckov.PerkTrees;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

// Token: 0x020001E5 RID: 485
public class UnlockStockShopItem : PerkBehaviour
{
	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06000E75 RID: 3701 RVA: 0x0003A9ED File Offset: 0x00038BED
	private string DescriptionFormat
	{
		get
		{
			return "PerkBehaviour_UnlockStockShopItem".ToPlainText();
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0003A9F9 File Offset: 0x00038BF9
	public override string Description
	{
		get
		{
			return this.DescriptionFormat.Format(new
			{
				this.ItemDisplayName
			});
		}
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06000E77 RID: 3703 RVA: 0x0003AA14 File Offset: 0x00038C14
	private string ItemDisplayName
	{
		get
		{
			return ItemAssetsCollection.GetMetaData(this.itemTypeID).DisplayName;
		}
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x0003AA34 File Offset: 0x00038C34
	private void Start()
	{
		if (base.Master.Unlocked && !EconomyManager.IsUnlocked(this.itemTypeID))
		{
			EconomyManager.Unlock(this.itemTypeID, false, false);
		}
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x0003AA5D File Offset: 0x00038C5D
	protected override void OnUnlocked()
	{
		base.OnUnlocked();
		EconomyManager.Unlock(this.itemTypeID, false, true);
	}

	// Token: 0x04000BFA RID: 3066
	[ItemTypeID]
	[SerializeField]
	private int itemTypeID;
}
