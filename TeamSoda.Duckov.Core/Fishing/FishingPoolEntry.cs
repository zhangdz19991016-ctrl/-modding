using System;
using ItemStatsSystem;
using UnityEngine;

namespace Fishing
{
	// Token: 0x02000217 RID: 535
	[Serializable]
	internal struct FishingPoolEntry
	{
		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06001003 RID: 4099 RVA: 0x0003F3D5 File Offset: 0x0003D5D5
		public int ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06001004 RID: 4100 RVA: 0x0003F3DD File Offset: 0x0003D5DD
		public float Weight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x04000CE8 RID: 3304
		[SerializeField]
		[ItemTypeID]
		private int id;

		// Token: 0x04000CE9 RID: 3305
		[SerializeField]
		private float weight;
	}
}
