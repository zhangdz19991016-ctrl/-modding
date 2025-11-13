using System;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002EE RID: 750
	[Serializable]
	public struct CropInfo
	{
		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x0600183B RID: 6203 RVA: 0x00059124 File Offset: 0x00057324
		public string DisplayName
		{
			get
			{
				if (this._normalMetaData == null)
				{
					this._normalMetaData = new ItemMetaData?(ItemAssetsCollection.GetMetaData(this.resultNormal));
				}
				return this._normalMetaData.Value.DisplayName;
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x0600183C RID: 6204 RVA: 0x00059167 File Offset: 0x00057367
		public TimeSpan GrowTime
		{
			get
			{
				return TimeSpan.FromTicks(this.totalGrowTicks);
			}
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x00059174 File Offset: 0x00057374
		public int GetProduct(ProductRanking ranking)
		{
			int num = 0;
			switch (ranking)
			{
			case ProductRanking.Poor:
				num = this.resultPoor;
				break;
			case ProductRanking.Normal:
				num = this.resultNormal;
				break;
			case ProductRanking.Good:
				num = this.resultGood;
				break;
			}
			if (num == 0)
			{
				if (this.resultNormal != 0)
				{
					return this.resultNormal;
				}
				if (this.resultPoor != 0)
				{
					return this.resultPoor;
				}
			}
			return num;
		}

		// Token: 0x0400119C RID: 4508
		public string id;

		// Token: 0x0400119D RID: 4509
		public GameObject displayPrefab;

		// Token: 0x0400119E RID: 4510
		[ItemTypeID]
		public int resultPoor;

		// Token: 0x0400119F RID: 4511
		[ItemTypeID]
		public int resultNormal;

		// Token: 0x040011A0 RID: 4512
		[ItemTypeID]
		public int resultGood;

		// Token: 0x040011A1 RID: 4513
		private ItemMetaData? _normalMetaData;

		// Token: 0x040011A2 RID: 4514
		public int resultAmount;

		// Token: 0x040011A3 RID: 4515
		[TimeSpan]
		public long totalGrowTicks;
	}
}
