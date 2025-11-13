using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002EC RID: 748
	[CreateAssetMenu]
	public class CropDatabase : ScriptableObject
	{
		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001834 RID: 6196 RVA: 0x00058FC5 File Offset: 0x000571C5
		public static CropDatabase Instance
		{
			get
			{
				return GameplayDataSettings.CropDatabase;
			}
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x00058FCC File Offset: 0x000571CC
		public static CropInfo? GetCropInfo(string id)
		{
			CropDatabase instance = CropDatabase.Instance;
			for (int i = 0; i < instance.entries.Count; i++)
			{
				CropInfo cropInfo = instance.entries[i];
				if (cropInfo.id == id)
				{
					return new CropInfo?(cropInfo);
				}
			}
			return null;
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x00059020 File Offset: 0x00057220
		internal static bool IsIdValid(string id)
		{
			return !(CropDatabase.Instance == null) && CropDatabase.Instance.entries.Any((CropInfo e) => e.id == id);
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x00059064 File Offset: 0x00057264
		internal static bool IsSeed(int itemTypeID)
		{
			return !(CropDatabase.Instance == null) && CropDatabase.Instance.seedInfos.Any((SeedInfo e) => e.itemTypeID == itemTypeID);
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x000590A8 File Offset: 0x000572A8
		internal static SeedInfo GetSeedInfo(int seedItemTypeID)
		{
			if (CropDatabase.Instance == null)
			{
				return default(SeedInfo);
			}
			return CropDatabase.Instance.seedInfos.FirstOrDefault((SeedInfo e) => e.itemTypeID == seedItemTypeID);
		}

		// Token: 0x04001198 RID: 4504
		[SerializeField]
		public List<CropInfo> entries = new List<CropInfo>();

		// Token: 0x04001199 RID: 4505
		[SerializeField]
		public List<SeedInfo> seedInfos = new List<SeedInfo>();
	}
}
