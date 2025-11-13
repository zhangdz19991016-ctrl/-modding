using System;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002F0 RID: 752
	[Serializable]
	public struct CropData
	{
		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x0600183E RID: 6206 RVA: 0x000591D2 File Offset: 0x000573D2
		public ProductRanking Ranking
		{
			get
			{
				if (this.score < 33)
				{
					return ProductRanking.Poor;
				}
				if (this.score < 66)
				{
					return ProductRanking.Normal;
				}
				return ProductRanking.Good;
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x0600183F RID: 6207 RVA: 0x000591ED File Offset: 0x000573ED
		public TimeSpan GrowTime
		{
			get
			{
				return TimeSpan.FromTicks(this.growTicks);
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001840 RID: 6208 RVA: 0x000591FA File Offset: 0x000573FA
		// (set) Token: 0x06001841 RID: 6209 RVA: 0x00059207 File Offset: 0x00057407
		public DateTime LastUpdateDateTime
		{
			get
			{
				return DateTime.FromBinary(this.lastUpdateDateTimeRaw);
			}
			set
			{
				this.lastUpdateDateTimeRaw = value.ToBinary();
			}
		}

		// Token: 0x040011A8 RID: 4520
		public string gardenID;

		// Token: 0x040011A9 RID: 4521
		public Vector2Int coord;

		// Token: 0x040011AA RID: 4522
		public string cropID;

		// Token: 0x040011AB RID: 4523
		public int score;

		// Token: 0x040011AC RID: 4524
		public bool watered;

		// Token: 0x040011AD RID: 4525
		[TimeSpan]
		public long growTicks;

		// Token: 0x040011AE RID: 4526
		[DateTime]
		public long lastUpdateDateTimeRaw;
	}
}
