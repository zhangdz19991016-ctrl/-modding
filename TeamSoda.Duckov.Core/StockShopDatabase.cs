using System;
using System.Collections.Generic;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000154 RID: 340
[CreateAssetMenu(menuName = "Duckov/Stock Shop Database")]
public class StockShopDatabase : ScriptableObject
{
	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06000A86 RID: 2694 RVA: 0x0002E954 File Offset: 0x0002CB54
	public static StockShopDatabase Instance
	{
		get
		{
			return GameplayDataSettings.StockshopDatabase;
		}
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x0002E95C File Offset: 0x0002CB5C
	public StockShopDatabase.MerchantProfile GetMerchantProfile(string merchantID)
	{
		return this.merchantProfiles.Find((StockShopDatabase.MerchantProfile e) => e.merchantID == merchantID);
	}

	// Token: 0x04000943 RID: 2371
	public List<StockShopDatabase.MerchantProfile> merchantProfiles;

	// Token: 0x020004B1 RID: 1201
	[Serializable]
	public class MerchantProfile
	{
		// Token: 0x04001C7F RID: 7295
		public string merchantID;

		// Token: 0x04001C80 RID: 7296
		public List<StockShopDatabase.ItemEntry> entries = new List<StockShopDatabase.ItemEntry>();
	}

	// Token: 0x020004B2 RID: 1202
	[Serializable]
	public class ItemEntry
	{
		// Token: 0x04001C81 RID: 7297
		[ItemTypeID]
		public int typeID;

		// Token: 0x04001C82 RID: 7298
		public int maxStock;

		// Token: 0x04001C83 RID: 7299
		public bool forceUnlock;

		// Token: 0x04001C84 RID: 7300
		public float priceFactor;

		// Token: 0x04001C85 RID: 7301
		public float possibility;

		// Token: 0x04001C86 RID: 7302
		public bool lockInDemo;
	}
}
