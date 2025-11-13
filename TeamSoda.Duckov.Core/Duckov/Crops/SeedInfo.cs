using System;
using Duckov.Utilities;
using ItemStatsSystem;

namespace Duckov.Crops
{
	// Token: 0x020002ED RID: 749
	[Serializable]
	public struct SeedInfo
	{
		// Token: 0x0600183A RID: 6202 RVA: 0x00059112 File Offset: 0x00057312
		public string GetRandomCropID()
		{
			return this.cropIDs.GetRandom(0f);
		}

		// Token: 0x0400119A RID: 4506
		[ItemTypeID]
		public int itemTypeID;

		// Token: 0x0400119B RID: 4507
		public RandomContainer<string> cropIDs;
	}
}
